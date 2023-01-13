// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Identity.Client;

namespace GraphMAUI.Services
{
    public partial class AuthenticationService
    {
        private partial Task RegisterMsalCacheAsync(ITokenCache tokenCache)
        {
            // MSAL registers it's own cache on iOS
            return Task.CompletedTask;
        }

        private partial PublicClientApplicationBuilder AddPlatformConfiguration(PublicClientApplicationBuilder builder)
        {
            // Configure keychain access group
            return builder.WithIosKeychainSecurityGroup("com.microsoft.adalcache");
        }
    }
}
