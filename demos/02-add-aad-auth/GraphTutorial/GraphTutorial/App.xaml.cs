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

namespace GraphTutorial
{
    public partial class App : Application, INotifyPropertyChanged
    {
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

        // UIParent used by Android version of the app
        public static object AuthUIParent = null;

        // Keychain security group used by iOS version of the app
        public static string iOSKeychainSecurityGroup = null;

        // Microsoft Authentication client for native/mobile apps
        public static IPublicClientApplication PCA;

        // Microsoft Graph client
        public static GraphServiceClient GraphClient;

        public App()
        {
            InitializeComponent();

            var builder = PublicClientApplicationBuilder
                .Create(OAuthSettings.ApplicationId);

            if (!string.IsNullOrEmpty(iOSKeychainSecurityGroup))
            {
                builder = builder.WithIosKeychainSecurityGroup(iOSKeychainSecurityGroup);
            }

            PCA = builder.Build();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public async Task SignIn()
        {
            var scopes = OAuthSettings.Scopes.Split(' ');

            // First, attempt silent sign in
            // If the user's information is already in the app's cache,
            // they won't have to sign in again.
            try
            {
                var accounts = await PCA.GetAccountsAsync();
                var silentAuthResult = await PCA.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();

                Debug.WriteLine("User already signed in.");
                Debug.WriteLine($"Access token: {silentAuthResult.AccessToken}");
            }
            catch (MsalUiRequiredException)
            {
                // This exception is thrown when an interactive sign-in is required.
                // Prompt the user to sign-in
                var interactiveRequest = PCA.AcquireTokenInteractive(scopes);

                if (AuthUIParent != null)
                {
                    interactiveRequest = interactiveRequest
                        .WithParentActivityOrWindow(AuthUIParent);
                }

                var authResult = await interactiveRequest.ExecuteAsync();
                Debug.WriteLine($"Access Token: {authResult.AccessToken}");
            }

            // Initialize Graph client
            GraphClient = new GraphServiceClient(new DelegateAuthenticationProvider(
                async (requestMessage) =>
                {
                    var accounts = await PCA.GetAccountsAsync();

                    var result = await PCA.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                        .ExecuteAsync();

                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", result.AccessToken);
                }));

            await GetUserInfo();

            IsSignedIn = true;
        }

        public async Task SignOut()
        {
            // Get all cached accounts for the app
            // (Should only be one)
            var accounts = await PCA.GetAccountsAsync();
            while (accounts.Any())
            {
                // Remove the account info from the cache
                await PCA.RemoveAsync(accounts.First());
                accounts = await PCA.GetAccountsAsync();
            }

            UserPhoto = null;
            UserName = string.Empty;
            UserEmail = string.Empty;
            IsSignedIn = false;
        }

        private async Task GetUserInfo()
        {
            // Get the logged on user's profile (/me)
            var user = await GraphClient.Me.Request().GetAsync();

            UserPhoto = ImageSource.FromStream(() => GetUserPhoto());
            UserName = user.DisplayName;
            UserEmail = string.IsNullOrEmpty(user.Mail) ? user.UserPrincipalName : user.Mail;
        }

        private Stream GetUserPhoto()
        {
            // Return the default photo
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("GraphTutorial.no-profile-pic.png");
        }
    }
}
