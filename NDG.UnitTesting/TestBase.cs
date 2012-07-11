using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NDG.BussinesLogic.Providers;
using NDG.DataAccessModels;
using NDG.UnitTesting.NDG.ViewModels;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using NDG.ViewModels.Helpers;
using NDG.DataAccessModels.Repositories;
using NDG.Common;

namespace NDG.UnitTesting
{
    [TestClass]
    public class TestBase : SilverlightTest
    {
        private PhoneApplicationFrame rootFrame;

        [TestInitialize]
        public  void TestInitialize()
        {
            if (Membership.CurrentUser == null)
            {
                UserRepository repository = new UserRepository();
                User user = new User();
                user = repository.TryGetCurrentUser();
                if (user == null)
                {
                    user = new User();
                    user.Name = LoginViewModelTest.LOGIN;
                    user.Password = LoginViewModelTest.PASSWORD;
                    Server server = new Server();
                    server.Address = LoginViewModelTest.SERVER_PATH;
                    user.ID = 0;
                    user.Server = server;
                    user.IsCurrent = true;
                    repository.AddNewUser(user);
                }

                Membership.CheckUserData(user.Name, user.Password, user.Server.Address, this.OnUserAuthenticated);
            }
        }

        private void OnUserAuthenticated(AuthentificationCode code)
        { 
        }

        /// <summary>
        /// Default time of waiting execution of asynchronous test
        /// </summary>
        private const int TIME_OUT = 10000;

        /// <summary>
        /// If test execute more time than timeout, test will be stopped by this method
        /// </summary>
        /// <param name="timeout">Time of waiting execution of asynchronous test</param>
        [Asynchronous]
        protected void EndOnWaitingResponse(int timeout = TIME_OUT)
        {
            EnqueueDelay(timeout);
            EnqueueCallback(() =>
            {
                Assert.Fail("Method was executed too long");
            });
            EnqueueTestComplete();
        }

        /// <summary>
        /// Call the Test complete method
        /// </summary>
        /// <param name="isCalled">Is test complete method allredy called</param>
        [Asynchronous]
        protected void CallTestComplete(ref bool isCalled)
        {
            if (!isCalled)
            {
                isCalled = true;
                TestComplete();
            }
        }        

        protected void ClearPageParameters()
        {
#if UNIT_TEST
            NavigationProvider.CurrentPageSource = this.rootFrame.CurrentSource.OriginalString;
#endif
        }

        protected void AddPageParameters(string parameter)
        {
            if (this.rootFrame == null)
            {
                rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            }
#if UNIT_TEST
            NavigationProvider.CurrentPageSource = this.rootFrame.CurrentSource.OriginalString + "?" + parameter;
#endif
        }
    }
}
