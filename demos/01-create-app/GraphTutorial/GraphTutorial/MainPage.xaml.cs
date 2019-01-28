using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GraphTutorial
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            lblUserName.Text = "Test User";
            lblUserEmail.Text = "test@contoso.com";

            imgProfilePhoto.Source = ImageSource.FromStream(() => GetUserPhoto());
        }

        private Stream GetUserPhoto()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("GraphTutorial.no-profile-pic.png");
        }

        private async void OnUserTapped(object sender, EventArgs e)
        {
            var signout = await DisplayAlert("Sign out?", "Do you want to sign out?", "Yes", "No");
            if (signout)
            {
                App.SignOut();

                await Navigation.PushModalAsync(new NavigationPage(new SignInPage()), true);
            }
        }
    }
}
