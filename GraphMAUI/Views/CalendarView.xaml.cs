// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using GraphMAUI.ViewModels;

namespace GraphMAUI.Views;

public partial class CalendarView : ContentPage
{
	public CalendarView(CalendarViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}
