// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace GraphMAUI.Services
{
    /// <summary>
    /// Represents the application settings for the app.
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Gets the client ID (aka application ID) from the app registration in the Azure portal.
        /// </summary>
        public string? ClientId { get; }

        /// <summary>
        /// Gets the redirect URI configured on the app registration in the Azure portal.
        /// </summary>
        public string RedirectUri { get; }

        /// <summary>
        /// Gets an array of permission scopes required by the application (ex. "User.Read").
        /// </summary>
        public string[]? GraphScopes { get; }

        // Token cache properties

        /// <summary>
        /// Gets the file name for the MSAL cache on the device.
        /// </summary>
        /// <remarks>
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache.
        /// </remarks>
        public string CacheFileName { get; }

        /// <summary>
        /// Gets the directory to contain the MSAL cache on the device.
        /// </summary>
        /// <remarks>
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache.
        /// </remarks>
        public string CacheDirectory { get; }

        // iOS/MacOS

        /// <summary>
        /// Gets the KeyChain service name.
        /// </summary>
        /// <remarks>
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache.
        /// </remarks>
        public string KeyChainServiceName { get; }

        /// <summary>
        /// Gets the KeyChain account name.
        /// </summary>
        /// <remarks>
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache.
        /// </remarks>
        public string KeyChainAccountName { get; }

        // Linux

        /// <summary>
        /// Gets the KeyRing schema name.
        /// </summary>
        /// <remarks>
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache.
        /// </remarks>
        public string LinuxKeyRingSchema { get; }

        /// <summary>
        /// Gets the KeyRing collection name.
        /// </summary>
        /// <remarks>
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache.
        /// </remarks>
        public string LinuxKeyRingCollection { get; }

        /// <summary>
        /// Gets the KeyRing label.
        /// </summary>
        /// <remarks>
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache.
        /// </remarks>
        public string LinuxKeyRingLabel { get; }

        /// <summary>
        /// Gets the first KeyRing attribute. Recommended to be "Version".
        /// </summary>
        /// <remarks>
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache.
        /// </remarks>
        public KeyValuePair<string, string> LinuxKeyRingAttr1 { get; }

        /// <summary>
        /// Gets the second KeyRing attribute. Recommended to be "ProductGroup".
        /// </summary>
        /// <remarks>
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache.
        /// </remarks>
        public KeyValuePair<string, string> LinuxKeyRingAttr2 { get; }
    }
}
