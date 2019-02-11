<!-- markdownlint-disable MD002 MD041 -->

In this exercise you will extend the application from the previous exercise to support authentication with Azure AD. This is required to obtain the necessary OAuth access token to call the Microsoft Graph. In this step you will integrate the [Microsoft Authentication Library for .NET (MSAL)](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet) into the application.

In **Solution Explorer**, expand the **GraphTutorial** project and right-click the **Models** folder. Choose **Add > Class...**. Name the class `OAuthSettings` and choose **Add**. Open the **OAuthSettings.cs** file and replace its contents with the following.

```cs
namespace GraphTutorial.Models
{
    public static class OAuthSettings
    {
        public const string ApplicationId = "YOUR_APP_ID_HERE";
        public const string RedirectUri = "YOUR_REDIRECT_URI_HERE";
        public const string Scopes = "User.Read Calendars.Read";
    }
}
```

> [!IMPORTANT]
> If you're using source control such as git, now would be a good time to exclude the `OAuthSettings.cs` file from source control to avoid inadvertently leaking your app ID.

## Implement sign-in

Open the **App.xaml.cs** file in the **GraphTutorial** project, and add the following `using` statements to the top of the file.

```cs
using GraphTutorial.Models;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
```

Add the following properties to the `App` class.

```cs
// UIParent used by Android version of the app
public static UIParent AuthUIParent = null;

// Microsoft Authentication client for native/mobile apps
public static PublicClientApplication PCA;

// Microsoft Graph client
public static GraphServiceClient GraphClient;
```

Next, create a new `PublicClientApplication` in the constructor of the `App` class.

```cs
public App()
{
    InitializeComponent();

    PCA = new PublicClientApplication(OAuthSettings.ApplicationId);

    MainPage = new MainPage();
}
```

Now update the `SignIn` function to use the `PublicClientApplication` to get an access token. Add the following code above the `await GetUserInfo();` line.

```cs
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
    // Prompt the user to sign-in
    var authResult = await PCA.AcquireTokenAsync(scopes, AuthUIParent);
    Debug.WriteLine($"Access Token: {authResult.AccessToken}");
}
```

This code first attempts to get an access token silently. If a user's information is already in the app's cache (for example, if the user closed the app previously without signing out), this will succeed, and there's no reason to prompt the user. If there is not a user's information in the cache, the `AcquireTokenSilentAsync` function throws an `MsalUiRequiredException`. In this case, the code calls the interactive function to get a token, `AcquireTokenAsync`.

Now update the `SignOut` function to remove the user's information from the cache. Add the following code to the beginning of the `SignOut` function.

```cs
// Get all cached accounts for the app
// (Should only be one)
var accounts = await PCA.GetAccountsAsync();
while (accounts.Any())
{
    // Remove the account info from the cache
    await PCA.RemoveAsync(accounts.First());
    accounts = await PCA.GetAccountsAsync();
}
```

### Update Android project to enable sign-in

When used in a Xamarin Android project, the Microsoft Authentication Library has a few [requirements specific to Android](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/Xamarin-Android-specifics).

First, you need to add the redirect URI from your app registration to the Android manifest. To do this, add a new activity to the **GraphTutorial.Android** project. Right-click the **GraphTutorial.Android** and choose **Add**, then **New Item...**. Choose **Activity**, name the activity `MsalActivity`, and choose **Add**.

Open the **MsalActivity.cs** file and delete the `[Activity(Label = "MsalActivity")]` line, then add the following attributes above the class declaration.

```cs
// This class only exists to create the necessary activity in the Android
// manifest. Doing it this way allows the value of the RedirectUri constant
// to be inserted at build.
[Activity(Name = "microsoft.identity.client.BrowserTabActivity")]
[IntentFilter(new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = Models.OAuthSettings.RedirectUri, DataHost = "auth")]
```

Next, open **MainActivity.cs** and add the following `using` statements to the top of the file.

```cs
using Microsoft.Identity.Client;
using Android.Content;
```

Then, override the `OnActivityResult` function to pass control to the MSAL library. Add the following to the `MainActivity` class.

```cs
protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
{
    base.OnActivityResult(requestCode, resultCode, data);
    AuthenticationContinuationHelper
        .SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
}
```

### Update iOS project to enable sign-in

> [!IMPORTANT]
> Because MSAL requires use of an Entitlements.plist file, you must configure Visual Studio with your Apple developer account to enable provisioning. For more information, see [Device provisioning for Xamarin.iOS](/xamarin/ios/get-started/installation/device-provisioning).

When used in a Xamarin iOS project, the Microsoft Authentication Library has a few [requirements specific to iOS](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/Xamarin-iOS-specifics).

First, you need to enable Keychain access. In Solution Explorer, expand the **GraphTutorial.iOS** project, then open the **Entitlements.plist** file. Locate the **Keychain** entitlement, and select **Enable KeyChain**. In **Keychain Groups**, add an entry with the format `com.YOUR_DOMAIN.GraphTutorial`.

![A screenshot of the Keychain entitlement configuration](./images/enable-keychain-access.png)

Next, you need to register the redirect URI you configured in the app registration step as a URL type that your app handles. Open the **Info.plist** file and make the following changes.

- On the **Application** tab, check that the value of **Bundle identifier** matches the value you set for **Keychain Groups** in **Entitlements.plist**. If it doesn't, update it now.
- On the **Advanced** tab, locate the **URL Types** section. Add a URL type here with the following values:
    - **Identifier**: set to the value of your **Bundle identifier**
    - **URL Schemes**: set to the redirect URI from your app registration that begins with `msal`
    - **Role**: `Editor`
    - **Icon**: Leave empty

![A screenshot of the URL Types section of Info.plist](./images/add-url-type.png)

Finally, update the code in the **GraphTutorial.iOS** project to handle the redirect during authentication. Open the **AppDelegate.cs** file and add the following `using` statement at the top of the file.

```cs
using Microsoft.Identity.Client;
```

Add the following line to `FinishedLaunching` function just before the `return` statement.

```cs
// Specify the Keychain access group
App.PCA.iOSKeychainSecurityGroup = "com.graphdevx.GraphTutorial";
```

Finally, override the `OpenUrl` function to pass the URL to the MSAL library. Add the following to the `AppDelegate` class.

```cs
// Handling redirect URL
// See: https://github.com/azuread/microsoft-authentication-library-for-dotnet/wiki/Xamarin-iOS-specifics
public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
{
    AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
    return true;
}
```

## Storing the tokens

When the Microsoft Authentication Library is used in a Xamarin project, it takes advantage of the native secure storage to cache tokens by default. See [Token cache serialization](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/token-cache-serialization) for more information.

## Test sign-in

At this point if you run the application and tap the **Sign in** button, you are prompted to sign in. On successful sign in, you should see the access token printed into the debugger's output.

![A screenshot of the Output window in Visual Studio](./images/debugger-access-token.png)
