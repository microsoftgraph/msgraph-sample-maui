// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Microsoft.Identity.Client;

namespace GraphMAUI;

/// <summary>
/// The main activity for Android.
/// </summary>
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    /// <inheritdoc/>
    /// <remarks>
    /// This allows the auth flow from the device browser to pass off to MSAL to
    /// complete the authentication.
    /// </remarks>
    protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent? data)
    {
        base.OnActivityResult(requestCode, resultCode, data);

        // Return control to MSAL
        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
    }
}
