// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace GraphMAUI.Services
{
    public interface IAlertService
    {
        /// <summary>
        /// Show an alert to the user
        /// </summary>
        Task ShowAlertAsync(string message, string title, string buttonLabel);
    }
}
