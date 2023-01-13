// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GraphMAUI.Services;

namespace GraphMAUI.ViewModels
{
    /// <summary>
    /// Base for view models that need authentication
    /// </summary>
    public abstract class AuthenticatedViewModel : ObservableObject
    {
        protected IAuthenticationService _authenticationService;

        public bool IsSignedIn
        {
            get => _authenticationService?.IsSignedIn ?? false;
        }

        public AuthenticatedViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _authenticationService.PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// Event handler for property change event on authentication service's IsSignedIn property
        /// </summary>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(_authenticationService.IsSignedIn))
            {
                // Bubble up the change to any views bound to this view model
                OnPropertyChanged(nameof(IsSignedIn));
                HandleSignInChange();
            }
        }

        /// <summary>
        /// Override this to take action when IsSignedIn changes
        /// </summary>
        protected virtual void HandleSignInChange()
        {
        }
    }
}
