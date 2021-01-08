// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// <UsingStatementsSnippet>
using System.ComponentModel;
using Microsoft.Graph;
// </UsingStatementsSnippet>

namespace GraphTutorial
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewEventPage : ContentPage, INotifyPropertyChanged
    {
        // <PropertiesSnippet>
        // Value of the Subject text box
        private string _subject = "";
        public string Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                OnPropertyChanged();
                IsValid = true;
            }
        }

        // Value of the Attendees text box
        private string _attendees = "";
        public string Attendees
        {
            get { return _attendees; }
            set
            {
                _attendees = value;
                OnPropertyChanged();
            }
        }

        // Value of the Start date picker
        private DateTime _startDate = DateTime.Today;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged();
                IsValid = true;
            }
        }

        // Value of the Start time picker
        private TimeSpan _startTime = TimeSpan.Zero;
        public TimeSpan StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnPropertyChanged();
                IsValid = true;
            }
        }

        // Value of the End date picker
        private DateTime _endDate = DateTime.Today;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged();
                IsValid = true;
            }
        }

        // Value of the End time picker
        private TimeSpan _endTime = TimeSpan.Zero;
        public TimeSpan EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                OnPropertyChanged();
                IsValid = true;
            }
        }

        // Value of the Body text box
        private string _body = "";
        public string Body
        {
            get { return _body; }
            set
            {
                _body = value;
                OnPropertyChanged();
            }
        }

        // Combine the date from date picker with time from time picker
        private DateTimeOffset CombineDateAndTime(DateTime date, TimeSpan time)
        {
            // Use the year, month, and day from the supplied DateTimeOffset
            // to create a new DateTime at midnight
            var dt = new DateTime(date.Year, date.Month, date.Day);

            // Add the TimeSpan, and use the user's timezone offset
            return new DateTimeOffset(dt + time, App.UserTimeZone.BaseUtcOffset);
        }

        // Combined value of Start date and time pickers
        public DateTimeOffset Start
        {
            get
            {
                return CombineDateAndTime(StartDate, StartTime);
            }
        }

        // Combined value of End date and time pickers
        public DateTimeOffset End
        {
            get
            {
                return CombineDateAndTime(EndDate, EndTime);
            }
        }

        public bool IsValid
        {
            get
            {
                // Subject is required, Start must be before
                // End
                return !string.IsNullOrWhiteSpace(Subject) &&
                       DateTimeOffset.Compare(Start, End) < 0;
            }
            private set
            {
                // Only used to fire event, value
                // is always calculated
                OnPropertyChanged();
            }
        }

        private bool _isWorking = false;
        public bool IsWorking
        {
            get { return _isWorking; }
            set
            {
                _isWorking = value;
                OnPropertyChanged();
            }
        }
        // </PropertiesSnippet>

        public NewEventPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        // <CreateEventSnippet>
        private async void CreateEvent(object sender, EventArgs e)
        {
            IsWorking = true;

            var timeZoneString =
                Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.UWP ?
                    App.UserTimeZone.StandardName : App.UserTimeZone.DisplayName;

            // Initialize a new Event object with the required fields
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

            if (!string.IsNullOrEmpty(Attendees))
            {
                var attendeeList = new List<Attendee>();

                // Treat any unrecognized text as a list of email addresses
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

            await App.GraphClient.Me.Events.Request().AddAsync(newEvent);

            await DisplayAlert("Success", "Event created.", "OK");

            IsWorking = false;
        }
        // </CreateEventSnippet>
    }
}