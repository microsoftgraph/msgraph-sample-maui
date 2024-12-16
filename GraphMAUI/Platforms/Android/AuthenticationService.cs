// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Identity.Client;

namespace GraphMAUI.Services
{
    /// <summary>
    /// Android-specific cache registration and platform configuration.
    /// </summary>
    public partial class AuthenticationService
    {
        /// <summary>
        /// Gets the Android-specific redirect URI.
        /// </summary>
        public partial string RedirectUri => settingsService.RedirectUri;

        private partial Task RegisterMsalCacheAsync(ITokenCache tokenCache)
        {
            // MSAL registers it's own cache on Android
            return Task.CompletedTask;
        }

        private partial PublicClientApplicationBuilder AddPlatformConfiguration(PublicClientApplicationBuilder builder)
        {
            // MSAL on Android needs the current activity
            return builder.WithParentActivityOrWindow(() => Platform.CurrentActivity);
        }
    }
}
