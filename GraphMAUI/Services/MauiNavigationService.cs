// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace GraphMAUI.Services
{
    public class MauiNavigationService : INavigationService
    {
        private IAuthenticationService _authenticationService;

        public MauiNavigationService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task InitializeAsync()
        {
            var isSignedIn = await _authenticationService.IsAuthenticatedAsync();

            await NavigateToAsync(isSignedIn ? "//Calendar" : "//Login");
        }

        public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null)
        {
            var shellNavigation = new ShellNavigationState(route);

            return routeParameters != null
                ? Shell.Current.GoToAsync(shellNavigation, routeParameters)
                : Shell.Current.GoToAsync(shellNavigation);
        }
        public Task NavigatePostSignIn()
        {
            // Navigate to the calendar view after sign in
            return NavigateToAsync("//Calendar");
        }

        public Task NavigatePostSignOut()
        {
            // Navigate to the welcome view after sign in
            return NavigateToAsync("//Login");
        }
    }
}
