// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Windows.Input;
using GraphMAUI.Services;

namespace GraphMAUI.ViewModels
{
    public class AppShellViewModel : AuthenticatedViewModel
    {
        private IGraphService _graphService;
        private INavigationService _navigationService;

        private string _signInOrOutText = "Sign In";
        /// <summary>
        /// The text to display on the sign in/out button
        /// </summary>
        public string SignInOrOutText
        {
            get => _signInOrOutText;
            set => SetProperty(ref _signInOrOutText, value);
        }

        private string _userName;
        /// <summary>
        /// The user's display name
        /// </summary>
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _userEmail;
        /// <summary>
        /// The user's email address
        /// </summary>
        public string UserEmail
        {
            get => _userEmail;
            set => SetProperty(ref _userEmail, value);
        }

        private ImageSource _userPhoto;
        /// <summary>
        /// The user's photo
        /// </summary>
        public ImageSource UserPhoto
        {
            get => _userPhoto;
            set => SetProperty(ref _userPhoto, value);
        }

        /// <summary>
        /// The command to execute when the sign in/out button is activated
        /// </summary>
        public ICommand SignInOrOutCommand { get; private set; }

        public AppShellViewModel(
            IAuthenticationService authenticationService,
            IGraphService graphService,
            INavigationService navigationService)
            : base(authenticationService)
        {
            _graphService = graphService;
            _navigationService = navigationService;

            SignInOrOutCommand = new Command(
                execute: async () => await OnSignInOrOut());
        }

        private async Task OnSignInOrOut()
        {
            if (IsSignedIn)
            {
                await _authenticationService.SignOutAsync();
                await _navigationService.NavigatePostSignOut();
            }
            else
            {
                var success = await _authenticationService.SignInAsync();
                await _navigationService.NavigatePostSignIn();
            }
        }

        protected override void HandleSignInChange()
        {
            // Update the button text
            SignInOrOutText = IsSignedIn ? "Sign out" : "Sign in";

            if (IsSignedIn)
            {
                GetUserDetailsAsync();
            }
            else
            {
                UserName = string.Empty;
                UserEmail = string.Empty;
                UserPhoto = null;
            }
        }

        /// <summary>
        /// Get the user's info and photo from Microsoft Graph
        /// </summary>
        private async void GetUserDetailsAsync()
        {
            // Get user
            var user = await _graphService.GetUserInfoAsync();

            // Get user's photo
            var photoStream = await _graphService.GetUserPhotoAsync();

            UserName = user.DisplayName;
            UserEmail = user.Mail ?? user.UserPrincipalName;
            UserPhoto = ImageSource.FromStream(() => photoStream);
        }
    }
}
