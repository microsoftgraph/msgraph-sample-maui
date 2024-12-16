// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using GraphMAUI.ViewModels;

namespace GraphMAUI.Views;

/// <summary>
/// The new event view.
/// </summary>
public partial class NewEventView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NewEventView"/> class.
    /// </summary>
    /// <param name="viewModel">The view model for the view.</param>
    public NewEventView(NewEventViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
