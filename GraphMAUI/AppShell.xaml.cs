// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using GraphMAUI.Services;
using GraphMAUI.ViewModels;

namespace GraphMAUI;

public partial class AppShell : Shell
{
    private INavigationService _navigationService;

    public AppShell(
        INavigationService navigationService,
        AppShellViewModel viewModel)
    {
        BindingContext = viewModel;
        _navigationService = navigationService;
        InitializeComponent();
    }

    protected override async void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler is not null)
        {
            await _navigationService.InitializeAsync();
        }
    }
}
