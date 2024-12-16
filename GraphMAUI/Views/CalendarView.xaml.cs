// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using GraphMAUI.ViewModels;

namespace GraphMAUI.Views;

/// <summary>
/// The calendar view.
/// </summary>
public partial class CalendarView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarView"/> class.
    /// </summary>
    /// <param name="viewModel">The view model for the view.</param>
    public CalendarView(CalendarViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
