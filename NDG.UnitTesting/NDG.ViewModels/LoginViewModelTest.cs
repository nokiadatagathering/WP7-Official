using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NDG.ViewModels;
using NDG.Helpers.Controls;

namespace NDG.UnitTesting.NDG.ViewModels
{
    [TestClass]
    public class LoginViewModelTest : TestBase
    {
        #region Constants

        internal const string SUCCESS_TEST = "LoginSuccess";
        internal const string WRONG_USERNAME_PASSWORD = "Wrong username/password";
        internal const string CANNOT_LOCATE_SERVER = "cannot locate server";
        internal const string FALLING_LOGIN = "some_login";
        internal const string FALLING_PASSWORD = "some_password";
        internal const string FALLING_SERVER_PATH = "some server path";
        internal const string LOGIN = "admin";
        internal const string PASSWORD = "ndg";
        internal const string SERVER_PATH = "http://192.168.13.47:9000/";

        #endregion Constants

        [TestMethod]
        public void CreationLoginTest()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            Assert.IsFalse(loginViewModel.IsBusy);
            Assert.IsNotNull(loginViewModel.LoginCommand);
            Assert.IsNotNull(loginViewModel.InitializeViewModelCommand);
        }

        [TestMethod]
        [Asynchronous]
        public void LoginTest()
        {
            this.TryLogin(LOGIN, PASSWORD, SERVER_PATH, SUCCESS_TEST);
            this.TryLogin(FALLING_LOGIN, PASSWORD, SERVER_PATH, WRONG_USERNAME_PASSWORD);
            this.TryLogin(LOGIN, FALLING_PASSWORD, SERVER_PATH, WRONG_USERNAME_PASSWORD);
            this.TryLogin(LOGIN, PASSWORD, FALLING_SERVER_PATH, CANNOT_LOCATE_SERVER);            
        }

        [Asynchronous]
        internal void TryLogin(string login, string password, string serverPath, string resultMessage)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.Login = login;
            bool isTestCompletedCalled = false;
            loginViewModel.Password = password;
            loginViewModel.ServerPath = serverPath;
            loginViewModel.TestCompleted += (sender, args) =>
            {
                if (args.Message == resultMessage)
                {
                    CallTestComplete(ref isTestCompletedCalled);
                }
            };

            Assert.IsTrue(loginViewModel.LoginCommand.CanExecute(null));
            loginViewModel.LoginCommand.Execute(null);
            if (serverPath != FALLING_SERVER_PATH)
            {
                EndOnWaitingResponse();
            }
        }
    }
}
