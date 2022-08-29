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
        /// The client ID (aka application ID) from the app registration in the Azure portal.
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// The redirect URI configured on the app registration in the Azure portal.
        /// </summary>
        string RedirectUri { get; }

        /// <summary>
        /// An array of permission scopes required by the application (ex. "User.Read").
        /// </summary>
        string[] GraphScopes { get; }

        // Token cache properties
        /// <summary>
        /// The file name for the MSAL cache on the device.
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache
        /// </summary>
        string CacheFileName { get; }

        /// <summary>
        /// The directory to contain the MSAL cache on the device.
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache
        /// </summary>
        string CacheDirectory { get; }

        // iOS/MacOS

        /// <summary>
        /// The KeyChain service name.
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache
        /// </summary>
        string KeyChainServiceName { get; }

        /// <summary>
        /// The KeyChain account name.
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache
        /// </summary>
        string KeyChainAccountName { get; }

        // Linux
        /// <summary>
        /// The KeyRing schema name.
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache
        /// </summary>
        string LinuxKeyRingSchema { get; }

        /// <summary>
        /// The KeyRing collection name.
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache
        /// </summary>
        string LinuxKeyRingCollection { get; }

        /// <summary>
        /// The KeyRing label.
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache
        /// </summary>
        string LinuxKeyRingLabel { get; }

        /// <summary>
        /// The first KeyRing attribute. Recommended to be "Version".
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache
        /// </summary>
        KeyValuePair<string, string> LinuxKeyRingAttr1 { get; }

        /// <summary>
        /// The second KeyRing attribute. Recommended to be "ProductGroup".
        /// See https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache
        /// </summary>
        KeyValuePair<string, string> LinuxKeyRingAttr2 { get; }
    }
}
