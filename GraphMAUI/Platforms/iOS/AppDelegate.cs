// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;

namespace GraphMAUI;

/// <inheritdoc/>
[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    /// <inheritdoc/>
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
