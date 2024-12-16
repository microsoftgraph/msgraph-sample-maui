// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Windows.Input;
using GraphMAUI.Services;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace GraphMAUI.ViewModels
{
    /// <summary>
    /// The view model for the new event page.
    /// </summary>
    public class NewEventViewModel : AuthenticatedViewModel
    {
        private readonly IAlertService alertService;
        private readonly TimeZoneInfo userTimeZone;

        private bool isBusy = false;
        private string subject = string.Empty;
        private string? attendees;
        private DateTime startDate = DateTime.Today;
        private TimeSpan startTime = TimeSpan.Zero;
        private DateTime endDate = DateTime.Today;
        private TimeSpan endTime = TimeSpan.Zero;
        private string? body;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewEventViewModel"/> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service to use.</param>
        /// <param name="graphService">The Graph service to use.</param>
        /// <param name="alertService">The alert service to use.</param>
        /// <param name="navigationService">The navigation service to use.</param>
        public NewEventViewModel(
            IAuthenticationService authenticationService,
            IGraphService graphService,
            IAlertService alertService,
            INavigationService navigationService)
            : base(authenticationService, graphService, navigationService)
        {
            this.alertService = alertService;

            userTimeZone = GraphService.GetUserTimeZoneAsync().Result ??
                TimeZoneInfo.Utc;

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
                    await NavigationService.NavigateToAsync("//Calendar");
                });
        }

        /// <summary>
        /// Gets or sets a value indicating whether an event is currently being created.
        /// </summary>
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        /// <summary>
        /// Gets or sets the subject for the new event. Required.
        /// </summary>
        public string Subject
        {
            get => subject;
            set
            {
                if (SetProperty(ref subject, value))
                {
                    RequiredPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a semi-colon delimited list of attendee email addresses. Optional.
        /// </summary>
        public string? Attendees
        {
            get => attendees;
            set => SetProperty(ref attendees, value);
        }

        /// <summary>
        /// Gets or sets the date portion of the start date and time. Required.
        /// </summary>
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                if (SetProperty(ref startDate, value))
                {
                    RequiredPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the time portion of the start date and time. Required.
        /// </summary>
        public TimeSpan StartTime
        {
            get => startTime;
            set
            {
                if (SetProperty(ref startTime, value))
                {
                    RequiredPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the start date and time.
        /// </summary>
        public DateTimeOffset Start
        {
            get => CombineDateAndTime(StartDate, StartTime);
        }

        /// <summary>
        /// Gets or sets the date portion of the end date and time. Required.
        /// </summary>
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                if (SetProperty(ref endDate, value))
                {
                    RequiredPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the time portion of the end date and time. Required.
        /// </summary>
        public TimeSpan EndTime
        {
            get => endTime;
            set
            {
                if (SetProperty(ref endTime, value))
                {
                    RequiredPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the end date and time.
        /// </summary>
        public DateTimeOffset End
        {
            get => CombineDateAndTime(EndDate, EndTime);
        }

        /// <summary>
        /// Gets or sets the body of the new event. Optional.
        /// </summary>
        public string? Body
        {
            get => body;
            set => SetProperty(ref body, value);
        }

        /// <summary>
        /// Gets the command to execute when the "Create" button is activated.
        /// </summary>
        public ICommand CreateEventCommand { get; private set; }

        /// <summary>
        /// Gets the command to execute when the "Cancel" button is activated.
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// Signal to listeners to re-check if the create event command can be executed.
        /// </summary>
        private void RequiredPropertyChanged()
        {
            if (CreateEventCommand != null && CreateEventCommand is Command command)
            {
                command.ChangeCanExecute();
            }
        }

        /// <summary>
        /// Generate a DateTimeOffset from a DateTime and TimeSpan.
        /// </summary>
        /// <param name="date">The <see cref="DateTime"/> with the date only information.</param>
        /// <param name="time">The <see cref="TimeSpan"/> with the time information.</param>
        /// <returns>A <see cref="DateTimeOffset"/> based on the supplied values.</returns>
        private DateTimeOffset CombineDateAndTime(DateTime date, TimeSpan time)
        {
            // Use the year, month, and day from the supplied DateTime
            // to create a new DateTime at midnight
            var dt = new DateTime(date.Year, date.Month, date.Day);

            // Add the TimeSpan, and use the user's timezone offset
            return new DateTimeOffset(dt + time, userTimeZone.BaseUtcOffset);
        }

        /// <summary>
        /// Create an event on the user's calendar based on the values in the form.
        /// </summary>
        /// <returns>An asynchronous task indicating the status of the operation.</returns>
        private async Task CreateEventAsync()
        {
            IsBusy = true;

            var timeZoneString = DeviceInfo.Current.Platform == DevicePlatform.WinUI ?
                userTimeZone.StandardName : userTimeZone.Id;

            // Initialize the new event with required fields
            var newEvent = new Event
            {
                Subject = Subject,
                Start = new DateTimeTimeZone
                {
                    DateTime = Start.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                    TimeZone = timeZoneString,
                },
                End = new DateTimeTimeZone
                {
                    DateTime = End.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                    TimeZone = timeZoneString,
                },
            };

            // If there's a body, add it
            if (!string.IsNullOrEmpty(Body))
            {
                newEvent.Body = new ItemBody
                {
                    Content = Body,
                    ContentType = BodyType.Text,
                };
            }

            // If there are attendees, add them
            if (!string.IsNullOrEmpty(Attendees))
            {
                var attendeeList = new List<Attendee>();

                var emails = Attendees.Split(
                    [';', ',', ' '],
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
                                    Address = email,
                                },
                            });
                        }
                    }
                    catch
                    {
                        /* Invalid, skip */
                    }
                }

                if (attendeeList.Count > 0)
                {
                    newEvent.Attendees = attendeeList;
                }
            }

            try
            {
                await GraphService.CreateEventAsync(newEvent);
                IsBusy = false;
                await alertService.ShowAlertAsync("Event created.", "Success", "OK");
                await NavigationService.NavigateToAsync("//Calendar");
                Reset();
            }
            catch (ServiceException ex)
            {
                IsBusy = false;
                await alertService.ShowAlertAsync(ex.Message, "Error", "OK");
            }
        }

        /// <summary>
        /// Clear the form fields.
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
