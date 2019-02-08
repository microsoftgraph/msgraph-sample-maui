using GraphTutorial.Models;
using Microsoft.Graph;
using Microsoft.Identity.Client;
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

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
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

        // Microsoft Authentication client
        public static PublicClientApplication PCA;

        // Microsoft Graph client
        public static GraphServiceClient GraphClient;

        public App()
        {
            InitializeComponent();

            isSignedIn = false;
            userPhoto = null;
            UserName = string.Empty;
            UserEmail = string.Empty;

            PCA = new PublicClientApplication(OAuthSettings.ApplicationId);

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
                var silentAuthResult = await PCA.AcquireTokenSilentAsync(
                    scopes, accounts.FirstOrDefault());

                Debug.WriteLine("User already signed in.");
                Debug.WriteLine($"Access token: {silentAuthResult.AccessToken}");
            }
            catch (MsalUiRequiredException)
            {
                // This exception is thrown when an interactive sign-in is required.
                var authResult = await PCA.AcquireTokenAsync(scopes);
                Debug.WriteLine($"Access Token: {authResult.AccessToken}");
            }

            // Initialize Graph client
            //GraphClient = new GraphServiceClient(new DelegateAuthenticationProvider(
            //    async (requestMessage) =>
            //    {
            //        var accounts = await PCA.GetAccountsAsync();

            //        var result = await PCA.AcquireTokenSilentAsync(scopes, accounts.FirstOrDefault());

            //        requestMessage.Headers.Authorization =
            //            new AuthenticationHeaderValue("Bearer", result.AccessToken);
            //    }));

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

            // Clear user information
            UserPhoto = null;
            UserName = string.Empty;
            UserEmail = string.Empty;
            IsSignedIn = false;
        }

        private async Task GetUserInfo()
        {
            // Get the logged on user's profile (/me)
            //var user = await GraphClient.Me.Request().GetAsync();

            UserPhoto = ImageSource.FromStream(() => GetUserPhoto());
            UserName = "Adele Vance";
            UserEmail = "adelev@contoso.com";
            //UserName = user.DisplayName;
            //UserEmail = string.IsNullOrEmpty(user.Mail) ? user.UserPrincipalName : user.Mail;
        }

        private Stream GetUserPhoto()
        {
            // Return the default photo
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("GraphTutorial.no-profile-pic.png");
        }
    }
}
