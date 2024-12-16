// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace GraphMAUI.Services
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MauiNavigationService"/> class.
    /// </summary>
    /// <param name="authenticationService">The authentication service to use.</param>
    public class MauiNavigationService(IAuthenticationService authenticationService) : INavigationService
    {
        private readonly IAuthenticationService authenticationService = authenticationService;

        /// <inheritdoc/>
        public async Task InitializeAsync()
        {
            var isSignedIn = await authenticationService.IsAuthenticatedAsync();

            await NavigateToAsync(isSignedIn ? "//Calendar" : "//Login");
        }

        /// <inheritdoc/>
        public Task NavigateToAsync(string route, IDictionary<string, object>? routeParameters = null)
        {
            var shellNavigation = new ShellNavigationState(route);

            return routeParameters != null
                ? Shell.Current.GoToAsync(shellNavigation, routeParameters)
                : Shell.Current.GoToAsync(shellNavigation);
        }

        /// <inheritdoc/>
        public Task NavigatePostSignIn()
        {
            // Navigate to the calendar view after sign in
            return NavigateToAsync("//Calendar");
        }

        /// <inheritdoc/>
        public Task NavigatePostSignOut()
        {
            // Navigate to the welcome view after sign in
            return NavigateToAsync("//Login");
        }
    }
}
