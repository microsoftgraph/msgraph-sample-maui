// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using GraphMAUI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client.Extensions.Msal;

namespace GraphMAUI.Services
{
    /// <summary>
    /// Settings for the GraphMAUI application.
    /// </summary>
    public class SettingsService : ISettingsService
    {
        private Settings _settings;

        /// <inheritdoc/>
        /// <remarks>
        /// Configurable in appSettings.json (or appSettings.Development.json)
        /// </remarks>
        public string ClientId { get => _settings.ClientId; }

        /// <inheritdoc/>
        /// <remarks>
        /// Using the MSAL public app redirect format
        /// </remarks>
        public string RedirectUri { get => $"msal{_settings.ClientId}://auth"; }

        /// <inheritdoc/>
        /// <remarks>
        /// Configurable in appSettings.json (or appSettings.Development.json)
        /// </remarks>
        public string[] GraphScopes { get => _settings.GraphScopes; }

        // Token cache properties
        public string CacheFileName => "graphmaui_msal_cache.txt";
        public string CacheDirectory => MsalCacheHelper.UserRootDirectory;

        // iOS
        public string KeyChainServiceName => "graphmaui_msal_service";
        public string KeyChainAccountName => "graphmaui_msal_account";

        // Linux
        public string LinuxKeyRingSchema => "com.contoso.graphmaui.tokencache";
        public string LinuxKeyRingCollection => MsalCacheHelper.LinuxKeyRingDefaultCollection;
        public string LinuxKeyRingLabel => "MSAL token cache for GraphMAUI app";
        public KeyValuePair<string, string> LinuxKeyRingAttr1 => new KeyValuePair<string, string>("Version", "1");
        public KeyValuePair<string, string> LinuxKeyRingAttr2 => new KeyValuePair<string, string>("ProductGroup", "GraphMAUI");

        public SettingsService(IConfiguration configuration)
        {
            _settings = configuration.GetRequiredSection("settings").Get<Settings>();
        }
    }
}
