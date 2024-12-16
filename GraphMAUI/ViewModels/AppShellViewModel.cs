// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Windows.Input;
using GraphMAUI.Services;

namespace GraphMAUI.ViewModels
{
    /// <summary>
    /// The view model for the app shell.
    /// </summary>
    public class AppShellViewModel : AuthenticatedViewModel
    {
        private string signInOrOutText = "Sign In";
        private string? userName;
        private string? userEmail;
        private ImageSource? userPhoto;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppShellViewModel"/> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service to use.</param>
        /// <param name="graphService">The Graph service to use.</param>
        /// <param name="navigationService">The navigation service to use.</param>
        public AppShellViewModel(
            IAuthenticationService authenticationService,
            IGraphService graphService,
            INavigationService navigationService)
            : base(authenticationService, graphService, navigationService)
        {
            SignInOrOutCommand = new Command(
                execute: async () => await OnSignInOrOut());
        }

        /// <summary>
        /// Gets or sets the text to display on the sign in/out button.
        /// </summary>
        public string SignInOrOutText
        {
            get => signInOrOutText;
            set => SetProperty(ref signInOrOutText, value);
        }

        /// <summary>
        /// Gets or sets the user's display name.
        /// </summary>
        public string? UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string? UserEmail
        {
            get => userEmail;
            set => SetProperty(ref userEmail, value);
        }

        /// <summary>
        /// Gets or sets the user's photo.
        /// </summary>
        public ImageSource? UserPhoto
        {
            get => userPhoto;
            set => SetProperty(ref userPhoto, value);
        }

        /// <summary>
        /// Gets the command to execute when the sign in/out button is activated.
        /// </summary>
        public ICommand SignInOrOutCommand { get; private set; }

        /// <summary>
        /// Loads user details if signed in.
        /// </summary>
        /// <returns>An asynchronous task indicating the status of the operation.</returns>
        protected override Task HandleSignInChange()
        {
            // Update the button text
            SignInOrOutText = IsSignedIn ? "Sign out" : "Sign in";

            if (IsSignedIn)
            {
                return GetUserDetailsAsync();
            }
            else
            {
                UserName = string.Empty;
                UserEmail = string.Empty;
                UserPhoto = null;
                return Task.CompletedTask;
            }
        }

        private async Task OnSignInOrOut()
        {
            if (IsSignedIn)
            {
                await AuthenticationService.SignOutAsync();
                await NavigationService.NavigatePostSignOut();
            }
            else
            {
                await AuthenticationService.SignInAsync();
                await NavigationService.NavigatePostSignIn();
            }
        }

        /// <summary>
        /// Get the user's info and photo from Microsoft Graph.
        /// </summary>
        private async Task GetUserDetailsAsync()
        {
            // Get user
            var user = await GraphService.GetUserInfoAsync();

            // Get user's photo
            var photoStream = await GraphService.GetUserPhotoAsync();

            UserName = user?.DisplayName;
            UserEmail = user?.Mail ?? user?.UserPrincipalName;
            UserPhoto = ImageSource.FromStream(() =>
            {
                if (photoStream != null)
                {
                    var memStream = new MemoryStream();
                    photoStream.CopyTo(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);
                    return memStream;
                }

                return null;
            });
        }
    }
}
