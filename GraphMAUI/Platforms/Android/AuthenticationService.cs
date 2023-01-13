// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Identity.Client;

namespace GraphMAUI.Services
{
    public partial class AuthenticationService
    {
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
