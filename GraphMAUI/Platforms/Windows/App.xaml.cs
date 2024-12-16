// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;

namespace GraphMAUI.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// This is the first line of authored code executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    /// <inheritdoc/>
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    /// <inheritdoc/>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
    }
}
