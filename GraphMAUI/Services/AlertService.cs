// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace GraphMAUI.Services
{
    /// <summary>
    /// Service to display alerts via MAUI DisplayAlert
    /// </summary>
    public class AlertService : IAlertService
    {
        public Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, buttonLabel);
        }
    }
}
