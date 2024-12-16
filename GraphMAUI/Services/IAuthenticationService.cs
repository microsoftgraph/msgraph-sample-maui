// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.ComponentModel;
using Microsoft.Graph;

namespace GraphMAUI.Services
{
    /// <summary>
    /// Service that provides authentication methods and access to
    /// an authenticated GraphServiceClient.
    /// </summary>
    public interface IAuthenticationService : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets an authenticated GraphServiceClient for the signed-in user.
        /// </summary>
        public GraphServiceClient GraphClient { get; }

        /// <summary>
        /// Gets a value indicating whether a user is signed in.
        /// </summary>
        public bool IsSignedIn { get; }

        /// <summary>
        /// Check if a user has signed in.
        /// </summary>
        /// <returns>true if a user has signed in, false if not.</returns>
        public Task<bool> IsAuthenticatedAsync();

        /// <summary>
        /// Attempts to sign in a user.
        /// </summary>
        /// <returns>true if authentication succeeds, false if not.</returns>
        public Task<bool> SignInAsync();

        /// <summary>
        /// Sign out the user.
        /// </summary>
        /// <returns>An asynchronous task indicating the status of the operation.</returns>
        public Task SignOutAsync();
    }
}
