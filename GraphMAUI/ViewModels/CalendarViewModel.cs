// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Globalization;
using System.Windows.Input;
using GraphMAUI.Services;
using Microsoft.Graph.Models;

namespace GraphMAUI.ViewModels
{
    /// <summary>
    /// The view model for the calendar page.
    /// </summary>
    public class CalendarViewModel : AuthenticatedViewModel
    {
        private bool isRefreshing = false;
        private List<Event> events = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarViewModel"/> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service to use.</param>
        /// <param name="graphService">The Graph service to use.</param>
        /// <param name="navigationService">The navigation service to use.</param>
        public CalendarViewModel(
            IAuthenticationService authenticationService,
            IGraphService graphService,
            INavigationService navigationService)
            : base(authenticationService, graphService, navigationService)
        {
            NewEventCommand = new Command(
                execute: () =>
                {
                    NavigationService.NavigateToAsync("//NewEvent");
                });

            RefreshCommand = new Command(
                execute: async () =>
                {
                    await LoadEventsAsync().ConfigureAwait(false);
                });

            HandleSignInChange().Wait();
        }

        /// <summary>
        /// Gets a value indicating whether the view is refreshing.
        /// </summary>
        public bool IsRefreshing
        {
            get => isRefreshing;
            private set => SetProperty(ref isRefreshing, value);
        }

        /// <summary>
        /// Gets the events to display in the view.
        /// </summary>
        public List<Event> Events
        {
            get => events;
            private set => SetProperty(ref events, value);
        }

        /// <summary>
        /// Gets the command to execute when the "New event" button is activated.
        /// </summary>
        public ICommand NewEventCommand { get; private set; }

        /// <summary>
        /// Gets the command to execute when the refresh view is refreshed.
        /// </summary>
        public ICommand RefreshCommand { get; private set; }

        /// <summary>
        /// Loads events from the user's calendar if signed in.
        /// </summary>
        /// <returns>An asynchronous task indicating the status of the operation.</returns>
        protected override async Task HandleSignInChange()
        {
            if (IsSignedIn)
            {
                await LoadEventsAsync().ConfigureAwait(false);
            }
            else
            {
                Events = [];
            }
        }

        /// <summary>
        /// Get the start of the current week adjusted for user's timezone.
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

        /// <summary>
        /// Load events from the user's calendar for the current week.
        /// </summary>
        /// <returns>An asynchronous task indicating the status of the operation.</returns>
        private async Task LoadEventsAsync()
        {
            IsRefreshing = true;

            // Get user's timezone
            var timeZone = await GraphService.GetUserTimeZoneAsync().ConfigureAwait(false) ??
                TimeZoneInfo.Utc;

            // Get the start of the current week
            var startOfWeek = GetUtcStartOfWeekInTimeZone(DateTime.Today, timeZone);
            var endOfWeek = startOfWeek.AddDays(7);

            var eventPage = await GraphService.GetCalendarForDateTimeRangeAsync(startOfWeek, endOfWeek, timeZone).ConfigureAwait(false);
            Events = eventPage?.Value ?? [];

            IsRefreshing = false;
        }
    }
}
