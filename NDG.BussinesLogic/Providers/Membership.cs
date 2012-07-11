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
using NDG.DataAccessModels;
using NDG.StorageAccess;
using NDG.DataAccessModels.Repositories;
using NDG.Common;

namespace NDG.BussinesLogic.Providers
{
    public static class Membership
    {
        static Membership()
        {
            using (var userRepository = new UserRepository())
                CurrentUser = userRepository.TryGetCurrentUser();
        }

        public static User CurrentUser { get; set; }

        private static AuthentificateUserCallback _authCallback;
        private static User _authUser;

        public static void AuthentificateUser(User user, AuthentificateUserCallback callback)
        {

            _authCallback = callback;
            _authUser = user;
            new AuthentitficationDataService().AuthentificateUser(user.Name, user.Password, user.Server.Address, AuthentificateUserCallback);
        }

        public static void CheckUserData(string userName, string password, string serverUrl, AuthentificateUserCallback callback)
        {
            _authCallback = callback;
            Server server = null;
            _authUser = new User();
            using (var serverRepository = new ServerRepository())
            {
                server = serverRepository.TryGetServerByAddress(serverUrl);

            }
            if (server != null)
            {

                User user = null;
                using (var userRepository = new UserRepository())
                {
                    user = userRepository.TryGetUserByNamePasswordAndServerID(userName, password, server.ID);
                }
                if (user != null)
                {
                    _authUser = user;
                    AuthentificateUserCallback(AuthentificationCode.LoginSuccessed);
                    return;
                }

                _authUser.ServerID = server.ID;
            }
            else
            {
                server = new Server
                {
                    Address = serverUrl,
                };
                _authUser.Server = server;
            }

            _authUser.Name = userName;
            _authUser.Password = password;
            if (InternetChecker.IsInernetActive)
                new AuthentitficationDataService().AuthentificateUser(_authUser.Name, _authUser.Password, serverUrl, CheckUserDataCallback);
            else
                InvokeCallbackWithAuthCodeParameter(AuthentificationCode.NoInternetConnection);
        }

        private static void CheckUserDataCallback(AuthentificationCode result)
        {
            if (result == AuthentificationCode.LoginSuccessed)
            {
                using (var userRepository = new UserRepository())
                    _authUser = userRepository.AddNewUser(_authUser);
                SetCurrentUser(_authUser);
            }

            InvokeCallbackWithAuthCodeParameter(result);
        }

        private static void AuthentificateUserCallback(AuthentificationCode result)
        {
            if (result == AuthentificationCode.LoginSuccessed)
                SetCurrentUser(_authUser);
            InvokeCallbackWithAuthCodeParameter(result);
        }

        private static void InvokeCallbackWithAuthCodeParameter(AuthentificationCode param)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                _authCallback.Invoke(param);
            });
        }

        private static void SetCurrentUser(User user)
        {
            ResetCurrentUser();
            CurrentUser = user;
            using (var userRepository = new UserRepository())
                userRepository.SetUserToCurrentByUserID(user.ID);
        }

        public static void ResetCurrentUser()
        {
            CurrentUser = null;
            using (var userRepository = new UserRepository())
                userRepository.ResetCurrentUser();
        }
    }
}
