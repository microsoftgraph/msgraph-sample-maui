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
    /// <remarks>
    /// Initializes a new instance of the <see cref="SettingsService"/> class.
    /// </remarks>
    /// <param name="configuration">The configuration loaded from appsettings.json.</param>
    public class SettingsService(IConfiguration configuration) : ISettingsService
    {
        private readonly Settings settings = configuration.GetRequiredSection("settings").Get<Settings>() ??
                throw new ApplicationException("Could not get required settings from appsettings.json");

        /// <inheritdoc/>
        /// <remarks>
        /// Configurable in appSettings.json (or appSettings.Development.json).
        /// </remarks>
        public string? ClientId { get => settings.ClientId; }

        /// <inheritdoc/>
        /// <remarks>
        /// Using the MSAL public app redirect format.
        /// </remarks>
        public string RedirectUri { get => $"msal{settings.ClientId}://auth"; }

        /// <inheritdoc/>
        /// <remarks>
        /// Configurable in appSettings.json (or appSettings.Development.json).
        /// </remarks>
        public string[]? GraphScopes { get => settings.GraphScopes; }

        // Token cache properties

        /// <inheritdoc/>
        public string CacheFileName => "graphmaui_msal_cache.txt";

        /// <inheritdoc/>
        public string CacheDirectory => MsalCacheHelper.UserRootDirectory;

        // iOS

        /// <inheritdoc/>
        public string KeyChainServiceName => "graphmaui_msal_service";

        /// <inheritdoc/>
        public string KeyChainAccountName => "graphmaui_msal_account";

        // Linux

        /// <inheritdoc/>
        public string LinuxKeyRingSchema => "com.contoso.graphmaui.tokencache";

        /// <inheritdoc/>
        public string LinuxKeyRingCollection => MsalCacheHelper.LinuxKeyRingDefaultCollection;

        /// <inheritdoc/>
        public string LinuxKeyRingLabel => "MSAL token cache for GraphMAUI app";

        /// <inheritdoc/>
        public KeyValuePair<string, string> LinuxKeyRingAttr1 => new("Version", "1");

        /// <inheritdoc/>
        public KeyValuePair<string, string> LinuxKeyRingAttr2 => new("ProductGroup", "GraphMAUI");
    }
}
