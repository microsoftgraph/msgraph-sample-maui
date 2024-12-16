// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace GraphMAUI.Services
{
    /// <summary>
    /// Service to display alerts via MAUI DisplayAlert.
    /// </summary>
    public class AlertService : IAlertService
    {
        /// <inheritdoc/>
        public Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            var currentPage = Application.Current?.Windows[0].Page ??
                throw new ApplicationException("Could not get the main window");

            return currentPage.DisplayAlert(title, message, buttonLabel);
        }
    }
}
