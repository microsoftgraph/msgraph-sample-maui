// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Identity.Client;

namespace GraphMAUI.Services
{
    /// <summary>
    /// MacCatalyst-specific cache registration and platform configuration.
    /// </summary>
    /// <remarks>
    /// This is un-tested and is just a copy from iOS.
    /// </remarks>
    public partial class AuthenticationService
    {
        /// <summary>
        /// Gets the MacCatalyst-specific redirect URI.
        /// </summary>
        public partial string RedirectUri => settingsService.RedirectUri;

        private partial Task RegisterMsalCacheAsync(ITokenCache tokenCache)
        {
            // TODO
            return Task.CompletedTask;
        }

        private partial PublicClientApplicationBuilder AddPlatformConfiguration(PublicClientApplicationBuilder builder)
        {
            // TODO
            return builder.WithIosKeychainSecurityGroup("com.microsoft.adalcache");
        }
    }
}
