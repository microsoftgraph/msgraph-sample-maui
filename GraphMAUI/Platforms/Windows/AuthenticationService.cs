// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Broker;
using Microsoft.Identity.Client.Extensions.Msal;

namespace GraphMAUI.Services
{
    /// <summary>
    /// Windows-specific cache registration and platform configuration.
    /// </summary>
    public partial class AuthenticationService
    {
        /// <summary>
        /// Gets the Windows-specific redirect URI.
        /// </summary>
        public partial string RedirectUri => $"ms-appx-web://Microsoft.AAD.BrokerPlugin/{settingsService.ClientId}";

        private partial async Task RegisterMsalCacheAsync(ITokenCache tokenCache)
        {
            // Configure storage properties for cross-platform
            // See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache
            var storageProperties =
                new StorageCreationPropertiesBuilder(settingsService.CacheFileName, settingsService.CacheDirectory)
                .WithLinuxKeyring(
                    settingsService.LinuxKeyRingSchema,
                    settingsService.LinuxKeyRingCollection,
                    settingsService.LinuxKeyRingLabel,
                    settingsService.LinuxKeyRingAttr1,
                    settingsService.LinuxKeyRingAttr2)
                .WithMacKeyChain(
                    settingsService.KeyChainServiceName,
                    settingsService.KeyChainAccountName)
                .Build();

            // Create a cache helper
            var cacheHelper = await MsalCacheHelper.CreateAsync(storageProperties);

            // Connect the PublicClientApplication's cache with the cacheHelper.
            // This will cause the cache to persist into secure storage on the device.
            cacheHelper.RegisterCache(tokenCache);
        }

        private partial PublicClientApplicationBuilder AddPlatformConfiguration(PublicClientApplicationBuilder builder)
        {
            if (Application.Current?.Windows[0].Handler.PlatformView is MauiWinUIWindow parent)
            {
                return builder
                    .WithBroker(new BrokerOptions(BrokerOptions.OperatingSystems.Windows))
                    .WithParentActivityOrWindow(() => parent.WindowHandle);
            }

            return builder;
        }
    }
}
