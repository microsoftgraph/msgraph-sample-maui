// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Globalization;
using System.Windows.Input;
using GraphMAUI.Services;
using Microsoft.Graph.Models;

namespace GraphMAUI.ViewModels
{
    public class CalendarViewModel : AuthenticatedViewModel
    {
        private IGraphService _graphService;
        private INavigationService _navigationService;

        private bool _isRefreshing = false;
        /// <summary>
        /// Indicates that the view is refreshing
        /// </summary>
        public bool IsRefreshing
        {
            get => _isRefreshing;
            private set => SetProperty(ref _isRefreshing, value);
        }

        private List<Event> _events = null;
        /// <summary>
        /// The events from the user's calendar
        /// </summary>
        public List<Event> Events
        {
            get => _events;
            private set => SetProperty(ref _events, value);
        }

        /// <summary>
        /// The command to execute when the "New event" button is activated
        /// </summary>
        public ICommand NewEventCommand { get; private set; }

        /// <summary>
        /// The command to execute when the refresh view is refreshed
        /// </summary>
        public ICommand RefreshCommand { get; private set; }

        public CalendarViewModel(
            IAuthenticationService authenticationService,
            IGraphService graphService,
            INavigationService navigationService)
            : base(authenticationService)
        {
            _graphService = graphService;
            _navigationService = navigationService;

            NewEventCommand = new Command(
                execute: () =>
                {
                    _navigationService.NavigateToAsync("//NewEvent");
                });

            RefreshCommand = new Command(
                execute: async () =>
                {
                    await LoadEventsAsync();
                });

            HandleSignInChange();
        }

        protected override async void HandleSignInChange()
        {
            if (IsSignedIn)
            {
                await LoadEventsAsync();
            }
            else
            {
                Events = null;
            }
        }

        /// <summary>
        /// Load events from the user's calendar for the current week
        /// </summary>
        private async Task LoadEventsAsync()
        {
            IsRefreshing = true;

            // Get user's timezone
            var timeZone = await _graphService.GetUserTimeZoneAsync();

            // Get the start of the current week
            var startOfWeek = GetUtcStartOfWeekInTimeZone(DateTime.Today, timeZone);
            var endOfWeek = startOfWeek.AddDays(7);

            var eventPage = await _graphService.GetCalendarForDateTimeRangeAsync(startOfWeek, endOfWeek, timeZone);
            Events = eventPage.Value;

            IsRefreshing = false;
        }

        /// <summary>
        /// Get the start of the current week adjusted for user's timezone
        /// </summary>
        private static DateTime GetUtcStartOfWeekInTimeZone(DateTime today, TimeZoneInfo timeZone)
        {
            // Get first day of week from system
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

            int diff = firstDayOfWeek - today.DayOfWeek;

            // create date as unspecified kind
            var unspecifiedStart = DateTime.SpecifyKind(today.AddDays(diff), DateTimeKind.Unspecified);

            // convert to UTC
            return TimeZoneInfo.ConvertTimeToUtc(unspecifiedStart, timeZone);
        }
    }
}
