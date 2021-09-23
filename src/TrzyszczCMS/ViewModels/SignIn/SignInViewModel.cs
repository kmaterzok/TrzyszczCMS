using ApplicationCore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.ViewModels.Bases;

namespace TrzyszczCMS.ViewModels.SignIn
{
    /// <summary>
    /// The viewmodel used for <see cref="Views.SignIn.SignInView"/> instances.
    /// </summary>
    public class SignInViewModel : ViewModelBase
    {
        #region Properties
        private string _username;
        /// <summary>
        /// Name of signing in user
        /// </summary>
        public string Username
        {
            get => _username;
            set => Set(ref _username, value, nameof(Username));
        }

        private string _password;
        /// <summary>
        /// Password of signing in user
        /// </summary>
        public string Password
        {
            get => _password;
            set => Set(ref _password, value, nameof(Password));
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
        public SignInViewModel()
        {
            this.Username     = string.Empty;
            this.Password     = string.Empty;
            this.ErrorMessage = string.Empty;
        }
        #endregion

        #region Methods
        public async Task SignInUser(Action afterSuccess)
        {
            // TODO: Consider using _AuthenticationStateProvider_ for sign in.
        }
        #endregion

    }
}
