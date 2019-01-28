using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GraphTutorial
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignInPage : ContentPage
	{
		public SignInPage ()
		{
			InitializeComponent ();
		}

        private async void SignIn(object sender, EventArgs e)
        {
            try
            {
                App.SignIn();
                await Navigation.PushModalAsync(new NavigationPage(new MainPage()), true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Sign in error", ex.Message, "OK");
            }
        }
    }
}