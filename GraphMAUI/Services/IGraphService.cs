// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Graph.Models;

namespace GraphMAUI.Services
{
    /// <summary>
    /// Service that provides access to data in Microsoft Graph.
    /// </summary>
    public interface IGraphService
    {
        /// <summary>
        /// Get the authenticated user's profile.
        /// </summary>
        /// <returns>The authenticated user.</returns>
        public Task<User?> GetUserInfoAsync();

        /// <summary>
        /// Get the authenticated user's photo.
        /// </summary>
        /// /// <returns>A <see cref="Stream"/> containing the authenticated user's photo.</returns>
        public Task<Stream?> GetUserPhotoAsync();

        /// <summary>
        /// Get the authenticated user's time zone from their mailbox settings.
        /// </summary>
        /// <returns>The authenticated user's time zone.</returns>
        public Task<TimeZoneInfo?> GetUserTimeZoneAsync();

        /// <summary>
        /// Get all events on the authenticated user's calendar in a given time period.
        /// </summary>
        /// <param name="start">The start of the time period.</param>
        /// <param name="end">The end of the time period.</param>
        /// <param name="timeZone">The time zone to return date/time values in.</param>
        /// <returns>A collection of events within the specified time period.</returns>
        public Task<EventCollectionResponse?> GetCalendarForDateTimeRangeAsync(DateTime start, DateTime end, TimeZoneInfo timeZone);

        /// <summary>
        /// Create an event on the authenticated user's calendar.
        /// </summary>
        /// <param name="newEvent">The event to create on the user's calendar.</param>
        /// <returns>An asynchronous task indicating the status of the operation.</returns>
        public Task CreateEventAsync(Event newEvent);
    }
}
