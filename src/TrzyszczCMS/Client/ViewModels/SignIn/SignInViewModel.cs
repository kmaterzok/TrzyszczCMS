using Core.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Client.Services.Interfaces;
using TrzyszczCMS.Client.ViewModels.Shared;

namespace TrzyszczCMS.Client.ViewModels.SignIn
{
    /// <summary>
    /// The viewmodel used for <see cref="Views.SignIn.SignInView"/> instances.
    /// </summary>
    public class SignInViewModel : ViewModelBase
    {
        #region Fields
        /// <summary>
        /// The service used for user authentication.
        /// </summary>
        private readonly IAuthService _authService;
        #endregion

        #region Properties
        private string _username;
        /// <summary>
        /// Name of signing in user
        /// </summary>
        public string Username
        {
            get => _username;
            set { Set(ref _username, value, nameof(Username)); this.ErrorMessage = string.Empty; }
        }

        private string _password;
        /// <summary>
        /// Password of signing in user
        /// </summary>
        public string Password
        {
            get => _password;
            set { Set(ref _password, value, nameof(Password)); this.ErrorMessage = string.Empty; }
        }

        private bool _rememberMe;
        /// <summary>
        /// Indicated if the user wants to be signed in for a long time.
        /// </summary>
        public  bool RememberMe
        {
            get => _rememberMe;
            set { Set(ref _rememberMe, value, nameof(RememberMe)); this.ErrorMessage = string.Empty; }
        }

        private string _errorMessage;
        /// <summary>
        /// Message informing about the error that happened.
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => Set(ref _errorMessage, value, nameof(ErrorMessage));
        }
        #endregion

        #region Ctor
        public SignInViewModel(IAuthService authService)
        {
            this._authService = authService;
            this.ClearData();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set empty data for signing in.
        /// </summary>
        public void ClearData()
        {
            this.Username = string.Empty;
            this.Password = string.Empty;
            this.ErrorMessage = string.Empty;
            this.RememberMe = false;
        }
        /// <summary>
        /// Sign in user to the administering panel.
        /// </summary>
        /// <param name="afterSuccess">What to to after successful sign in.</param>
        /// <returns>Task returning if the signing in was finished successfully and used can administer the page</returns>
        public async Task<bool> SignInUser(Action afterSuccess)
        {
            if(!PasswordValidationHelper.CheckCredentials(this.Username, this.Password, out string error))
            {
                this.ErrorMessage = error;
                return false;
            }

            var success = await this._authService.AuthenticateWithCredentialsAsync(this.Username, this.Password, this.RememberMe);

            if (success) { afterSuccess?.Invoke(); }
            else         { this.ErrorMessage = "You have typed wrong username or password."; }

            return success;
        }
        #endregion

    }
}
