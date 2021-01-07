// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using GraphTutorial.Models;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeZoneConverter;

namespace GraphTutorial
{
    public partial class App : Xamarin.Forms.Application, INotifyPropertyChanged
    {
        // <AuthPropertiesSnippet>
        // UIParent used by Android version of the app
        public static object AuthUIParent = null;

        // Keychain security group used by iOS version of the app
        public static string iOSKeychainSecurityGroup = null;

        // Microsoft Authentication client for native/mobile apps
        public static IPublicClientApplication PCA;

        // Microsoft Graph client
        public static GraphServiceClient GraphClient;

        // Microsoft Graph permissions used by app
        private readonly string[] Scopes = OAuthSettings.Scopes.Split(' ');
        // </AuthPropertiesSnippet>

        // <GlobalPropertiesSnippet>
        // Is a user signed in?
        private bool isSignedIn;
        public bool IsSignedIn
        {
            get { return isSignedIn; }
            set
            {
                isSignedIn = value;
                OnPropertyChanged("IsSignedIn");
                OnPropertyChanged("IsSignedOut");
            }
        }

        public bool IsSignedOut { get { return !isSignedIn; } }

        // The user's display name
        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }

        // The user's email address
        private string userEmail;
        public string UserEmail
        {
            get { return userEmail; }
            set
            {
                userEmail = value;
                OnPropertyChanged("UserEmail");
            }
        }

        // The user's profile photo
        private ImageSource userPhoto;
        public ImageSource UserPhoto
        {
            get { return userPhoto; }
            set
            {
                userPhoto = value;
                OnPropertyChanged("UserPhoto");
            }
        }

        // The user's time zone
        public static TimeZoneInfo UserTimeZone;
        // </GlobalPropertiesSnippet>

        // <AppConstructorSnippet>
        public App()
        {
            InitializeComponent();

            var builder = PublicClientApplicationBuilder
                .Create(OAuthSettings.ApplicationId)
                .WithRedirectUri(OAuthSettings.RedirectUri);

            if (!string.IsNullOrEmpty(iOSKeychainSecurityGroup))
            {
                builder = builder.WithIosKeychainSecurityGroup(iOSKeychainSecurityGroup);
            }

            PCA = builder.Build();

            MainPage = new MainPage();
        }
        // </AppConstructorSnippet>

        public async Task SignIn()
        {
            // <GetTokenSnippet>
            // First, attempt silent sign in
            // If the user's information is already in the app's cache,
            // they won't have to sign in again.
            try
            {
                var accounts = await PCA.GetAccountsAsync();

                var silentAuthResult = await PCA
                    .AcquireTokenSilent(Scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();

                Debug.WriteLine("User already signed in.");
                Debug.WriteLine($"Successful silent authentication for: {silentAuthResult.Account.Username}");
                Debug.WriteLine($"Access token: {silentAuthResult.AccessToken}");
            }
            catch (MsalUiRequiredException msalEx)
            {
                // This exception is thrown when an interactive sign-in is required.
                Debug.WriteLine("Silent token request failed, user needs to sign-in: " + msalEx.Message);
                // Prompt the user to sign-in
                var interactiveRequest = PCA.AcquireTokenInteractive(Scopes);

                if (AuthUIParent != null)
                {
                    interactiveRequest = interactiveRequest
                        .WithParentActivityOrWindow(AuthUIParent);
                }

                var interactiveAuthResult = await interactiveRequest.ExecuteAsync();
                Debug.WriteLine($"Successful interactive authentication for: {interactiveAuthResult.Account.Username}");
                Debug.WriteLine($"Access token: {interactiveAuthResult.AccessToken}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Authentication failed. See exception messsage for more details: " + ex.Message);
            }
            // </GetTokenSnippet>

            await InitializeGraphClientAsync();
        }

        public async Task SignOut()
        {
            // <RemoveAccountSnippet>
            // Get all cached accounts for the app
            // (Should only be one)
            var accounts = await PCA.GetAccountsAsync();
            while (accounts.Any())
            {
                // Remove the account info from the cache
                await PCA.RemoveAsync(accounts.First());
                accounts = await PCA.GetAccountsAsync();
            }
            // </RemoveAccountSnippet>

            UserPhoto = null;
            UserName = string.Empty;
            UserEmail = string.Empty;
            IsSignedIn = false;
        }

        // <InitializeGraphClientSnippet>
        private async Task InitializeGraphClientAsync()
        {
            var currentAccounts = await PCA.GetAccountsAsync();
            try
            {
                if (currentAccounts.Count() > 0)
                {
                    // Initialize Graph client
                    GraphClient = new GraphServiceClient(new DelegateAuthenticationProvider(
                        async (requestMessage) =>
                        {
                            var result = await PCA.AcquireTokenSilent(Scopes, currentAccounts.FirstOrDefault())
                                .ExecuteAsync();

                            requestMessage.Headers.Authorization =
                                new AuthenticationHeaderValue("Bearer", result.AccessToken);
                        }));

                    await GetUserInfo();

                    IsSignedIn = true;
                }
                else
                {
                    IsSignedIn = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to initialized graph client.");
                Debug.WriteLine($"Accounts in the msal cache: {currentAccounts.Count()}.");
                Debug.WriteLine($"See exception message for details: {ex.Message}");
            }
        }
        // </InitializeGraphClientSnippet>

        // <GetUserInfoSnippet>
        private async Task GetUserInfo()
        {
            // Get the logged on user's profile (/me)
            var user = await GraphClient.Me.Request()
                .Select(u => new
                {
                    u.DisplayName,
                    u.Mail,
                    u.MailboxSettings,
                    u.UserPrincipalName
                })
                .GetAsync();

            UserPhoto = ImageSource.FromStream(() => GetUserPhoto());
            UserName = user.DisplayName;
            UserEmail = string.IsNullOrEmpty(user.Mail) ? user.UserPrincipalName : user.Mail;
            try
            {
                UserTimeZone = TZConvert.GetTimeZoneInfo(user.MailboxSettings.TimeZone);
            } catch
            {
                // Default to local time zone
                UserTimeZone = TimeZoneInfo.Local;
            }
        }
        // </GetUserInfoSnippet>

        private Stream GetUserPhoto()
        {
            // Return the default photo
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("GraphTutorial.no-profile-pic.png");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
