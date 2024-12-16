// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Android.App;
using Android.Content;
using Microsoft.Identity.Client;

namespace GraphMAUI
{
    /// <inheritdoc/>
    [Activity(Exported = true)]
    [IntentFilter(
        [Intent.ActionView],
        Categories = [Intent.CategoryBrowsable, Intent.CategoryDefault],
        DataHost = "auth",
        DataScheme = "msalYOUR_CLIENT_ID_HERE")]
    public class MsalActivity : BrowserTabActivity
    {
    }
}
