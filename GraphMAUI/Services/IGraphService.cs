// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Graph;

namespace GraphMAUI.Services
{
    /// <summary>
    /// Service that provides access to data in Microsoft Graph
    /// </summary>
    public interface IGraphService
    {
        /// <summary>
        /// Get the authenticated user's profile
        /// </summary>
        public Task<User> GetUserInfoAsync();

        /// <summary>
        /// Get the authenticated user's photo
        /// </summary>
        public Task<Stream> GetUserPhotoAsync();

        /// <summary>
        /// Get the authenticated user's time zone from their mailbox settings
        /// </summary>
        public Task<TimeZoneInfo> GetUserTimeZoneAsync();

        /// <summary>
        /// Get all events on the authenticated user's calendar in a given time period
        /// </summary>
        /// <param name="start">The start of the time period</param>
        /// <param name="end">The end of the time period</param>
        /// <param name="timeZone">The time zone to return date/time values in</param>
        public Task<IUserCalendarViewCollectionPage> GetCalendarForDateTimeRangeAsync(DateTime start, DateTime end, TimeZoneInfo timeZone);

        /// <summary>
        /// Create an event on the authenticated user's calendar
        /// </summary>
        public Task CreateEventAsync(Event newEvent);
    }
}
