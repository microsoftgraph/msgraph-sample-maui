// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace GraphMAUI.Services
{
    /// <summary>
    /// Service to manage authentication state.
    /// </summary>
    public partial class AuthenticationService : ObservableObject, IAuthenticationService, IAuthenticationProvider
    {
        private readonly Lazy<Task<IPublicClientApplication>> pca;
        private readonly ISettingsService settingsService;
        private string userIdentifier = string.Empty;
        private bool isSignedIn = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="settingsService">The settings service to get auth settings from.</param>
        public AuthenticationService(ISettingsService settingsService)
        {
            pca = new Lazy<Task<IPublicClientApplication>>(InitializeMsalWithCache);
            this.settingsService = settingsService;
        }

        /// <inheritdoc/>
        public GraphServiceClient GraphClient => new(this);

        /// <inheritdoc/>
        public bool IsSignedIn
        {
            get => isSignedIn;
            private set
            {
                isSignedIn = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the redirect URI.
        /// </summary>
        public partial string RedirectUri { get; }

        /// <inheritdoc/>
        /// <remarks>
        /// Attempts to get a token silently from the cache. If this fails, the user needs to sign in.
        /// </remarks>
        public async Task<bool> IsAuthenticatedAsync()
        {
            var silentResult = await GetTokenSilentlyAsync().ConfigureAwait(false);
            IsSignedIn = silentResult is not null;
            return IsSignedIn;
        }

        /// <inheritdoc/>
        public async Task<bool> SignInAsync()
        {
            // First attempt to get a token silently
            var result = await GetTokenSilentlyAsync().ConfigureAwait(false);

            // If silent attempt didn't work, try an
            // interactive sign in
            result ??= await GetTokenInteractivelyAsync().ConfigureAwait(false);

            IsSignedIn = result is not null;
            return IsSignedIn;
        }

        /// <inheritdoc/>
        public async Task SignOutAsync()
        {
            var pca = await this.pca.Value;

            // Get all accounts (there should only be one)
            // and remove them from the cache
            var accounts = await pca.GetAccountsAsync().ConfigureAwait(false);
            foreach (var account in accounts)
            {
                await pca.RemoveAsync(account);
            }

            // Clear the user identifier
            userIdentifier = string.Empty;
            IsSignedIn = false;
        }

        /// <inheritdoc/>
        public async Task AuthenticateRequestAsync(
            RequestInformation request,
            Dictionary<string, object>? additionalAuthenticationContext = null,
            CancellationToken cancellationToken = default)
        {
            if (request.URI.Host == "graph.microsoft.com")
            {
                // First try to get the token silently
                var result = await GetTokenSilentlyAsync(cancellationToken).ConfigureAwait(false);

                // If silent acquisition fails, try interactive
                result ??= await GetTokenInteractivelyAsync(cancellationToken).ConfigureAwait(false);

                request.Headers.Add("Authorization", $"Bearer {result.AccessToken}");
            }
        }

        /// <summary>
        /// Initializes a PublicClientApplication with a secure serialized cache.
        /// </summary>
        private async Task<IPublicClientApplication> InitializeMsalWithCache()
        {
            // Initialize the PublicClientApplication
            var builder = PublicClientApplicationBuilder
                .Create(settingsService.ClientId)
                .WithRedirectUri(RedirectUri);

            builder = AddPlatformConfiguration(builder);

            var pca = builder.Build();

            await RegisterMsalCacheAsync(pca.UserTokenCache).ConfigureAwait(false);

            return pca;
        }

        private partial PublicClientApplicationBuilder AddPlatformConfiguration(PublicClientApplicationBuilder builder);

        private partial Task RegisterMsalCacheAsync(ITokenCache tokenCache);

        /// <summary>
        /// Get the user account from the MSAL cache.
        /// </summary>
        private async Task<IAccount?> GetUserAccountAsync()
        {
            try
            {
                var pca = await this.pca.Value.ConfigureAwait(false);

                if (string.IsNullOrEmpty(userIdentifier))
                {
                    // If no saved user ID, then get the first account.
                    // There should only be one account in the cache anyway.
                    var accounts = await pca.GetAccountsAsync().ConfigureAwait(false);
                    var account = accounts.FirstOrDefault();

                    // Save the user ID so this is easier next time
                    userIdentifier = account?.HomeAccountId.Identifier ?? string.Empty;
                    return account;
                }

                // If there's a saved user ID use it to get the account
                return await pca.GetAccountAsync(userIdentifier).ConfigureAwait(false);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Attempt to acquire a token silently (no prompts).
        /// </summary>
        private async Task<AuthenticationResult?> GetTokenSilentlyAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                var pca = await this.pca.Value.ConfigureAwait(false);

                var account = await GetUserAccountAsync().ConfigureAwait(false);
                if (account == null)
                {
                    return null;
                }

                var result = await pca.AcquireTokenSilent(settingsService.GraphScopes, account)
                    .ExecuteAsync(cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch (MsalUiRequiredException)
            {
                return null;
            }
            catch (Exception ex)
            {
                var foo = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Attempts to get a token interactively using the device's browser.
        /// </summary>
        private async Task<AuthenticationResult> GetTokenInteractivelyAsync(CancellationToken cancellationToken = default)
        {
            var pca = await this.pca.Value.ConfigureAwait(false);

            var result = await pca.AcquireTokenInteractive(settingsService.GraphScopes)
                .ExecuteAsync(cancellationToken).ConfigureAwait(false);

            // Store the user ID to make account retrieval easier
            userIdentifier = result.Account.HomeAccountId.Identifier;
            return result;
        }
    }
}
