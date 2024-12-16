// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using GraphMAUI.Services;
using GraphMAUI.ViewModels;

namespace GraphMAUI;

/// <summary>
/// The app class.
/// </summary>
public partial class App : Application
{
    private readonly INavigationService navigationService;
    private readonly AppShellViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    /// <param name="navigationService">The navigation service to use.</param>
    /// <param name="viewModel">The view model.</param>
    public App(
        INavigationService navigationService,
        AppShellViewModel viewModel)
    {
        this.navigationService = navigationService;
        this.viewModel = viewModel;
        InitializeComponent();
    }

    /// <inheritdoc/>
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell(navigationService, viewModel));
    }
}
