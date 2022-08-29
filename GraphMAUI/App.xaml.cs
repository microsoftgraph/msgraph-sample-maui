// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using GraphMAUI.Services;
using GraphMAUI.ViewModels;

namespace GraphMAUI;

public partial class App : Application
{
	public App(
		INavigationService navigationService,
        AppShellViewModel viewModel)
	{
		InitializeComponent();

		MainPage = new AppShell(navigationService, viewModel);
	}
}
