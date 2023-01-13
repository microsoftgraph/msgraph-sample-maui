// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using GraphMAUI.ViewModels;

namespace GraphMAUI.Views;

public partial class NewEventView : ContentPage
{
    public NewEventView(NewEventViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
