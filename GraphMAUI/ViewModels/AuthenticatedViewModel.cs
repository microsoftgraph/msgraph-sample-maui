// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GraphMAUI.Services;

namespace GraphMAUI.ViewModels
{
    /// <summary>
    /// Base for view models that need authentication.
    /// </summary>
    public abstract class AuthenticatedViewModel : ObservableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticatedViewModel"/> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service to use.</param>
        /// <param name="graphService">The Graph service to use.</param>
        /// <param name="navigationService">The navigation service to use.</param>
        public AuthenticatedViewModel(
            IAuthenticationService authenticationService,
            IGraphService graphService,
            INavigationService navigationService)
        {
            AuthenticationService = authenticationService;
            AuthenticationService.PropertyChanged += OnPropertyChanged;
            GraphService = graphService;
            NavigationService = navigationService;
        }

        /// <summary>
        /// Gets a value indicating whether a user is signed in.
        /// </summary>
        public bool IsSignedIn
        {
            get => AuthenticationService?.IsSignedIn ?? false;
        }

        /// <summary>
        /// Gets the authentication service.
        /// </summary>
        protected IAuthenticationService AuthenticationService { get; private set; }

        /// <summary>
        /// Gets the Graph service.
        /// </summary>
        protected IGraphService GraphService { get; private set; }

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        protected INavigationService NavigationService { get; private set; }

        /// <summary>
        /// Override this to take action when IsSignedIn changes.
        /// </summary>
        /// <returns>An asynchronous task indicating the status of the operation.</returns>
        protected virtual Task HandleSignInChange()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Event handler for property change event on authentication service's IsSignedIn property.
        /// </summary>
        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(AuthenticationService.IsSignedIn))
            {
                // Bubble up the change to any views bound to this view model
                OnPropertyChanged(nameof(IsSignedIn));
                HandleSignInChange();
            }
        }
    }
}
