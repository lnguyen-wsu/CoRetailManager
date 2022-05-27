using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TRMDesktopUI.Helpers;
using TRMDesktopUI.Library.Api;

namespace TRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        #region Setting up the properties base on the LoginView.xaml - Wiring up the value the View to backend

        private string _userName;
        private string _password;
        private IAPIHelper _apiHelper;
        private string _errorMessage;
        public LoginViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }
        public string UserName
        {
            get { return _userName; }
            set {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn); // we dont have to have here
            }
        }

        public string Password
        {
            get { return _password; }
            set {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        //10-B Binding Error Handling 
        public bool IsErrorVisible
        {
            get {             
                return ErrorMessage?.Length > 0 ;
            }          
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set {
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);              
            }
        }

        #endregion
        // 1E: Login button
        // 1F: Password Box
        public bool CanLogIn { get { return (UserName?.Length > 0 && Password?.Length > 0); } }

        /// <summary>
        /// Login feature which match the name of Button LogIn 
        /// </summary>
        /// <param name="username">Same name LoginView userName</param>
        /// <param name="password">Same name LoginView password</param>
        public async Task LogIn()
        {
            // Step 9- 9 : Now using api to authenticate 
            try
            {
                // Reset the Error Message 
                ErrorMessage = string.Empty;
                var result = await _apiHelper.Authenticate(UserName, Password);

                // Lesson 11C : Capture more info about the user
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;               
            }
        }

       
    }
}
