// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Windows.Input;
using GraphMAUI.Services;
using Microsoft.Graph;

namespace GraphMAUI.ViewModels
{
    public class NewEventViewModel : AuthenticatedViewModel
    {
        private IGraphService _graphService;
        private IAlertService _alertService;
        private INavigationService _navigationService;
        private TimeZoneInfo _userTimeZone;

        private bool _isBusy = false;
        /// <summary>
        /// Indicates that an event is currently being created
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _subject;
        /// <summary>
        /// The subject for the new event. Required.
        /// </summary>
        public string Subject
        {
            get => _subject;
            set
            {
                if (SetProperty(ref _subject, value))
                    RequiredPropertyChanged();
            }
        }

        private string _attendees;
        /// <summary>
        /// A semi-colon delimited list of attendee email addresses. Optional.
        /// </summary>
        public string Attendees
        {
            get => _attendees;
            set => SetProperty(ref _attendees, value);
        }

        private DateTime _startDate = DateTime.Today;
        /// <summary>
        /// The date portion of the start date and time. Required.
        /// </summary>
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (SetProperty(ref _startDate, value))
                    RequiredPropertyChanged();
            }
        }

        private TimeSpan _startTime = TimeSpan.Zero;
        /// <summary>
        /// The time portion of the start date and time. Required.
        /// </summary>
        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                if (SetProperty(ref _startTime, value))
                    RequiredPropertyChanged();
            }
        }

        /// <summary>
        /// The start date and time
        /// </summary>
        public DateTimeOffset Start
        {
            get => CombineDateAndTime(StartDate, StartTime);
        }
        
        private DateTime _endDate = DateTime.Today;
        /// <summary>
        /// The date portion of the end date and time. Required.
        /// </summary>
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (SetProperty(ref _endDate, value))
                    RequiredPropertyChanged();
            }
        }

        private TimeSpan _endTime = TimeSpan.Zero;
        /// <summary>
        /// The time portion of the end date and time. Required.
        /// </summary>
        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                if (SetProperty(ref _endTime, value))
                    RequiredPropertyChanged();
            }
        }

        /// <summary>
        /// The end date and time
        /// </summary>
        public DateTimeOffset End
        {
            get => CombineDateAndTime(EndDate, EndTime);
        }

        private string _body;
        /// <summary>
        /// The body of the new event. Optional.
        /// </summary>
        public string Body
        {
            get => _body;
            set => SetProperty(ref _body, value);
        }

        /// <summary>
        /// The command to execute when the "Create" button is activated
        /// </summary>
        public ICommand CreateEventCommand { get; private set; }

        /// <summary>
        /// The command to execute when the "Cancel" button is activated
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        public NewEventViewModel(
            IAuthenticationService authenticationService,
            IGraphService graphService,
            IAlertService alertService,
            INavigationService navigationService)
            : base(authenticationService)
        {
            _graphService = graphService;
            _alertService = alertService;
            _navigationService = navigationService;
            _userTimeZone = _graphService.GetUserTimeZoneAsync().Result;

            CreateEventCommand = new Command(
                execute: async () =>
                {
                    await CreateEventAsync();
                },
                canExecute: () =>
                {
                    // Subject must be non-empty and End must be after Start
                    return !string.IsNullOrEmpty(Subject) &&
                           DateTimeOffset.Compare(Start, End) < 0;
                });

            CancelCommand = new Command(
                execute: async () =>
                {
                    Reset();
                    // Return to the calendar view
                    await _navigationService.NavigateToAsync("//Calendar");
                });
        }

        /// <summary>
        /// Signal to listeners to re-check if the create event command can be executed
        /// </summary>
        private void RequiredPropertyChanged()
        {
            if (CreateEventCommand != null)
            {
                (CreateEventCommand as Command).ChangeCanExecute();
            }
        }

        /// <summary>
        /// Generate a DateTimeOffset from a DateTime and TimeSpan
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private DateTimeOffset CombineDateAndTime(DateTime date, TimeSpan time)
        {
            // Use the year, month, and day from the supplied DateTime
            // to create a new DateTime at midnight
            var dt = new DateTime(date.Year, date.Month, date.Day);

            // Add the TimeSpan, and use the user's timezone offset
            return new DateTimeOffset(dt + time, _userTimeZone.BaseUtcOffset);
        }

        /// <summary>
        /// Create an event on the user's calendar based on the values in the form
        /// </summary>
        /// <returns></returns>
        private async Task CreateEventAsync()
        {
            IsBusy = true;

            var timeZoneString = DeviceInfo.Current.Platform == DevicePlatform.WinUI ?
                _userTimeZone.StandardName : _userTimeZone.DisplayName;

            // Initialize the new event with required fields
            var newEvent = new Event
            {
                Subject = Subject,
                Start = new DateTimeTimeZone
                {
                    DateTime = Start.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                    TimeZone = timeZoneString
                },
                End = new DateTimeTimeZone
                {
                    DateTime = End.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                    TimeZone = timeZoneString
                }
            };

            // If there's a body, add it
            if (!string.IsNullOrEmpty(Body))
            {
                newEvent.Body = new ItemBody
                {
                    Content = Body,
                    ContentType = BodyType.Text
                };    
            }

            // If there are attendees, add them
            if (!string.IsNullOrEmpty(Attendees))
            {
                var attendeeList = new List<Attendee>();

                var emails = Attendees.Split(new[] { ';', ',', ' ' },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var email in emails)
                {
                    try
                    {
                        // Validate the email address
                        var addr = new System.Net.Mail.MailAddress(email);
                        if (addr.Address == email)
                        {
                            attendeeList.Add(new Attendee
                            {
                                Type = AttendeeType.Required,
                                EmailAddress = new EmailAddress 
                                { 
                                    Address = email 
                                }
                            });
                        }
                    }
                    catch { /* Invalid, skip */ }
                }

                if (attendeeList.Count > 0)
                {
                    newEvent.Attendees = attendeeList;
                }
            }

            try
            {
                await _graphService.CreateEventAsync(newEvent);
                IsBusy = false;
                await _alertService.ShowAlertAsync("Event created.", "Success", "OK");
                await _navigationService.NavigateToAsync("//Calendar");
                Reset();
            }
            catch(ServiceException ex)
            {
                IsBusy = false;
                await _alertService.ShowAlertAsync(ex.Message, "Error", "OK");
            }
        }

        /// <summary>
        /// Clear the form fields
        /// </summary>
        private void Reset()
        {
            Subject = string.Empty;
            Attendees = string.Empty;
            Body = string.Empty;
            StartDate = DateTime.Today;
            StartTime = TimeSpan.Zero;
            EndDate = DateTime.Today;
            EndTime = TimeSpan.Zero;
        }
    }
}
