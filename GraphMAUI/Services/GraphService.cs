// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Graph;
using TimeZoneConverter;

namespace GraphMAUI.Services
{
    public class GraphService : IGraphService
    {
        private IAuthenticationService _authenticationService;

        private User _user;
        private Stream _userPhoto;
        private TimeZoneInfo _userTimeZone;

        public GraphService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public Task CreateEventAsync(Event newEvent)
        {
            var graphClient = _authenticationService.GraphClient;

            return graphClient.Me.Events.Request().AddAsync(newEvent);
        }

        public Task<IUserCalendarViewCollectionPage> GetCalendarForDateTimeRangeAsync(DateTime start, DateTime end, TimeZoneInfo timeZone)
        {
            var graphClient = _authenticationService.GraphClient;

            var timeZoneString = DeviceInfo.Current.Platform == DevicePlatform.WinUI ?
                timeZone.StandardName : timeZone.DisplayName;

            // Calendar view API sets the time period using query parameters
            // ?startDatetime={start}&endDateTime={end}
            var queryOptions = new List<QueryOption>
            {
                new QueryOption("startDateTime", start.ToString("o")),
                new QueryOption("endDateTime", end.ToString("o"))
            };

            return graphClient.Me
                .CalendarView
                .Request(queryOptions)
                // Set the "Prefer": "outlook.timezone=""" header so
                // date/time values are returned in the chosen time zone
                .Header("Prefer", $"outlook.timezone=\"{timeZoneString}\"")
                // Request only the values we use
                .Select(e => new
                {
                    e.Subject,
                    e.Organizer,
                    e.Start,
                    e.End
                })
                // Sort by the start time
                .OrderBy("start/DateTime")
                // Limit to 50 events
                .Top(50)
                .GetAsync();
        }

        public async Task<User> GetUserInfoAsync()
        {
            var graphClient = _authenticationService.GraphClient;

            if (_authenticationService.IsSignedIn)
            {
                if (_user == null)
                {
                    // Get the user, cache for subsequent calls
                    _user = await graphClient.Me
                        .Request()
                        .Select(u => new
                        {
                            u.DisplayName,
                            u.Mail,
                            u.MailboxSettings,
                            u.UserPrincipalName
                        })
                        .GetAsync();
                }
            }
            else
            {
                _user = null;
            }

            return _user;
        }

        public async Task<Stream> GetUserPhotoAsync()
        {
            var graphClient = _authenticationService.GraphClient;

            if (_authenticationService.IsSignedIn)
            {
                if (_userPhoto == null)
                {
                    // Get the user photo, cache for subsequent calls
                    _userPhoto = await graphClient.Me
                        .Photo
                        .Content
                        .Request()
                        .GetAsync();
                }
            }
            else
            {
                _userPhoto = null;
            }

            return _userPhoto;
        }

        public async Task<TimeZoneInfo> GetUserTimeZoneAsync()
        {
            if (_authenticationService.IsSignedIn)
            {
                if (_userTimeZone == null)
                {
                    var user = await GetUserInfoAsync();
                    // Default to UTC if time zone is not set
                    _userTimeZone = TZConvert.GetTimeZoneInfo(user.MailboxSettings?.TimeZone ?? "UTC");
                }
            }
            else
            {
                _userTimeZone = null;
            }

            return _userTimeZone;
        }
    }
}
