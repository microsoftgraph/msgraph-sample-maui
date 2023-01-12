// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Android.App;
using Android.Content;
using Microsoft.Identity.Client;

namespace GraphMAUI
{
    [Activity(Exported = true)]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
        DataHost = "auth",
        DataScheme = "msal2a5044bb-da58-4e0d-8b85-e4d21298a3b2")]
    public class MsalActivity : BrowserTabActivity
    {
    }
}
