using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GraphTutorial.Models;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;

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

        // Microsoft Authentication client for native/mobile apps
        public static IPublicClientApplication PCA;

        // Microsoft Graph client
        public static GraphServiceClient GraphClient;

        public App()
        {
            InitializeComponent();

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
            await GetUserInfo();

            IsSignedIn = true;
        }

        public async Task SignOut()
        {
            UserPhoto = null;
            UserName = string.Empty;
            UserEmail = string.Empty;
            IsSignedIn = false;
        }

        private async Task GetUserInfo()
        {
            UserPhoto = ImageSource.FromStream(() => GetUserPhoto());
            UserName = "Adele Vance";
            UserEmail = "adelev@contoso.com";
        }

        private Stream GetUserPhoto()
        {
            // Return the default photo
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("GraphTutorial.no-profile-pic.png");
        }
    }
}
