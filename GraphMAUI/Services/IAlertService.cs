// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace GraphMAUI.Services
{
    /// <summary>
    /// Service to display alerts.
    /// </summary>
    public interface IAlertService
    {
        /// <summary>
        /// Shows an alert.
        /// </summary>
        /// <param name="message">The message to display in the alert.</param>
        /// <param name="title">The title of the alert.</param>
        /// <param name="buttonLabel">The text for the alert button.</param>
        /// <returns>An asynchronous task indicating the status of the operation.</returns>
        public Task ShowAlertAsync(string message, string title, string buttonLabel);
    }
}
