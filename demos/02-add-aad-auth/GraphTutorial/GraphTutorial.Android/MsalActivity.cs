using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GraphTutorial.Droid
{
    // This class only exists to create the necessary activity in the Android
    // manifest. Doing it this way allows the value of the RedirectUri constant
    // to be inserted at build.
    [Activity(Name = "microsoft.identity.client.BrowserTabActivity")]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = Models.OAuthSettings.RedirectUri, DataHost = "auth")]
    public class MsalActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }
    }
}