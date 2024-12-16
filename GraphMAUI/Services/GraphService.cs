// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Graph.Models;
using TimeZoneConverter;

namespace GraphMAUI.Services
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphService"/> class.
    /// </summary>
    /// <param name="authenticationService">The authentication service to use.</param>
    public class GraphService(IAuthenticationService authenticationService) : IGraphService
    {
        private readonly IAuthenticationService authenticationService = authenticationService;

        private User? user;
        private Stream? userPhoto;
        private TimeZoneInfo? userTimeZone;

        /// <inheritdoc/>
        public async Task CreateEventAsync(Event newEvent)
        {
            var graphClient = authenticationService.GraphClient;

            await graphClient.Me.Events.PostAsync(newEvent).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<EventCollectionResponse?> GetCalendarForDateTimeRangeAsync(DateTime start, DateTime end, TimeZoneInfo timeZone)
        {
            var graphClient = authenticationService.GraphClient;

            var timeZoneString = DeviceInfo.Current.Platform == DevicePlatform.WinUI ?
                timeZone.StandardName : timeZone.Id;

            var events = await graphClient.Me
                .CalendarView
                .GetAsync(requestConfiguration =>
                {
                    requestConfiguration.Headers.Add(
                        "Prefer",
                        $"outlook.timezone=\"{timeZoneString}\"");

                    // Calendar view API sets the time period using query parameters
                    // ?startDatetime={start}&endDateTime={end}
                    requestConfiguration.QueryParameters.StartDateTime =
                        start.ToString("o");
                    requestConfiguration.QueryParameters.EndDateTime =
                        end.ToString("o");
                    requestConfiguration.QueryParameters.Select =
                        ["subject", "organizer", "start", "end"];
                    requestConfiguration.QueryParameters.Orderby =
                        ["start/DateTime"];
                    requestConfiguration.QueryParameters.Top = 50;
                })
                .ConfigureAwait(false);

            return events;
        }

        /// <inheritdoc/>
        public async Task<User?> GetUserInfoAsync()
        {
            var graphClient = authenticationService.GraphClient;

            if (authenticationService.IsSignedIn)
            {
                // Get the user, cache for subsequent calls
                user ??= await graphClient.Me.GetAsync(
                    requestConfiguration =>
                    {
                        requestConfiguration.QueryParameters.Select =
                            ["displayName", "mail", "mailboxSettings", "userPrincipalName"];
                    })
                    .ConfigureAwait(false);
            }
            else
            {
                user = null;
            }

            return user;
        }

        /// <inheritdoc/>
        public async Task<Stream?> GetUserPhotoAsync()
        {
            var graphClient = authenticationService.GraphClient;

            if (authenticationService.IsSignedIn)
            {
                // Get the user photo, cache for subsequent calls
                userPhoto ??= await graphClient.Me
                    .Photo
                    .Content
                    .GetAsync()
                    .ConfigureAwait(false);
            }
            else
            {
                userPhoto = null;
            }

            return userPhoto;
        }

        /// <inheritdoc/>
        public async Task<TimeZoneInfo?> GetUserTimeZoneAsync()
        {
            if (authenticationService.IsSignedIn)
            {
                if (userTimeZone == null)
                {
                    var user = await GetUserInfoAsync().ConfigureAwait(false);

                    // Default to UTC if time zone is not set
                    userTimeZone = TZConvert.GetTimeZoneInfo(user?.MailboxSettings?.TimeZone ?? "UTC");
                }
            }
            else
            {
                userTimeZone = null;
            }

            return userTimeZone;
        }
    }
}
