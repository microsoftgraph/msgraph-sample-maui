// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace GraphMAUI.Services
{
    /// <summary>
    /// Service that provide navigation between views in the app.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Initializes navigation and routes to the startup view.
        /// </summary>
        /// <returns>An asynchronous task indicating the status of the operation.</returns>
        public Task InitializeAsync();

        /// <summary>
        /// Navigates to a specified route.
        /// </summary>
        /// <param name="route">The route URI.</param>
        /// <param name="routeParameters">Optional route parameters.</param>
        /// <returns>An asynchronous task indicating the status of the operation.</returns>
        public Task NavigateToAsync(string route, IDictionary<string, object>? routeParameters = null);

        /// <summary>
        /// Navigate to the default view after sign in.
        /// </summary>
        /// <returns>An asynchronous task indicating the status of the operation.</returns>
        public Task NavigatePostSignIn();

        /// <summary>
        /// Navigate to the default view after sign out.
        /// </summary>
        /// <returns>An asynchronous task indicating the status of the operation.</returns>
        public Task NavigatePostSignOut();
    }
}
