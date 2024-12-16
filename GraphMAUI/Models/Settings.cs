// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace GraphMAUI.Models
{
    /// <summary>
    /// This class represents app settings from appsettings.json.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Gets or sets the client ID (aka application ID) from the app registration in the Azure portal.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Gets or sets an array of permission scopes required by the application (ex. "User.Read").
        /// </summary>
        public string[]? GraphScopes { get; set; }
    }
}
