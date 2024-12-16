// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using GraphMAUI.Services;
using GraphMAUI.ViewModels;
using GraphMAUI.Views;

namespace GraphMAUI;

/// <summary>
/// The app shell class.
/// </summary>
public partial class AppShell : Shell
{
    private readonly INavigationService navigationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppShell"/> class.
    /// </summary>
    /// <param name="navigationService">The navigation service to use.</param>
    /// <param name="viewModel">The view model.</param>
    public AppShell(
        INavigationService navigationService,
        AppShellViewModel viewModel)
    {
        this.navigationService = navigationService;
        BindingContext = viewModel;
        InitializeComponent();

        // Routing.RegisterRoute("Login", typeof(WelcomeView));
        // Routing.RegisterRoute("Calendar", typeof(CalendarView));
        // Routing.RegisterRoute("NewEvent", typeof(NewEventView));
    }

    /// <inheritdoc/>
    protected override async void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler is not null)
        {
            await navigationService.InitializeAsync();
        }
    }
}
