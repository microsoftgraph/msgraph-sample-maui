// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Identity.Client;
using UIKit;

namespace GraphMAUI.Services
{
    /// <summary>
    /// iOS-specific cache registration and platform configuration.
    /// </summary>
    public partial class AuthenticationService
    {
        /// <summary>
        /// Gets the iOS-specific redirect URI.
        /// </summary>
        public partial string RedirectUri => settingsService.RedirectUri;

        private partial Task RegisterMsalCacheAsync(ITokenCache tokenCache)
        {
            // MSAL registers it's own cache on iOS
            return Task.CompletedTask;
        }

        private partial PublicClientApplicationBuilder AddPlatformConfiguration(PublicClientApplicationBuilder builder)
        {
            // Configure keychain access group
            return builder
                .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                .WithParentActivityOrWindow(() => new UIViewController());
        }
    }
}
