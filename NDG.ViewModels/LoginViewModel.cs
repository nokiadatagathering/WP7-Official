// ----------------------------------------------------------------------
// <copyright file="LoginViewModel.cs" company="QArea">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace NDG.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using NDG.BussinesLogic.Providers;
    using NDG.Common;
    using NDG.DataAccessModels.Repositories;
    using System.IO.IsolatedStorage;
    using NDG.Helpers.Classes;
    using DeepForest.Phone.Assets.Tools;

    /// <summary>
    /// View model for login view.
    /// </summary>
    public class LoginViewModel : ViewModel
    {
        #region Constants

        private const string HOME_PAGE_SOURCE = "/Views/Home/HomePage.xaml";
        private const string SEVER_PATH_PARAMETER = "serverPath";

        #endregion Constants

        #region Fields

        private static string _firstLaunchKey = "firstLaunchKey";

        /// <summary>
        /// Path to server.
        /// </summary>
        private string serverPath = string.Empty;

        /// <summary>
        /// Login property.
        /// </summary>
        private string login = string.Empty;

        private bool wasInitialization = false;

        /// <summary>
        /// Password proeprty.
        /// </summary>
        private string password = string.Empty;

        /// <summary>
        /// Is there is authorization data or not.
        /// </summary>
        private bool isAuthorized = false;

        /// <summary>
        /// A key to store whether the user accepted or
        /// declined the EULA.
        /// </summary>
        private const string EULA_KEY = "eula";

        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the LoginViewModel class.
        /// </summary>
        public LoginViewModel()
        {

            this.InitializeViewModelCommand = new RelayCommand(this.InitializeViewModeExecute);
            this.LoginCommand = new RelayCommand(this.LoginExecute, this.LoginCanExecute);
            if (Membership.CurrentUser == null)
            {
                this.Login = string.Empty;
                this.Password = string.Empty;
            }
            else
            {
                this.Login = Membership.CurrentUser.Name;
                this.Password = Membership.CurrentUser.Password;
                this.ServerPath = Membership.CurrentUser.Server.Address;
            }

        }

        #region Properties

        /// <summary>
        /// Gets login command.
        /// </summary>
        public RelayCommand LoginCommand { get; private set; }

        /// <summary>
        /// Gets or sets server path.
        /// </summary>
        public string ServerPath
        {
            get
            {
                return this.serverPath;
            }

            set
            {
                this.serverPath = value;
                this.RaisePropertyChanged("ServerPath");
                this.LoginCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is there authorization data or n0t.
        /// </summary>
        public bool IsAuthorized
        {
            get
            {
                return this.isAuthorized;
            }

            set
            {
                this.isAuthorized = value;
                this.RaisePropertyChanged("IsAuthorized");
            }
        }

        /// <summary>
        /// Gets or sets login property.
        /// </summary>
        public string Login
        {
            get
            {
                return this.login;
            }

            set
            {
                this.login = value;
                this.RaisePropertyChanged("Login");
                this.LoginCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets password property.
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                this.password = value;
                this.RaisePropertyChanged("Password");
                this.LoginCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion Properties

        /// <summary>
        /// Starts intialization of view model.
        /// </summary>
        private void InitializeViewModeExecute()
        {
            if (!this.wasInitialization)
            {
                this.wasInitialization = true;
                using (var settingsRepository = new SettingsRepository())
                {
                    if (!IsolatedStorageSettings.ApplicationSettings.Contains(_firstLaunchKey))
                    {
                        IsolatedStorageSettings.ApplicationSettings[_firstLaunchKey] = wasInitialization;
                        var result = MessageBox.Show("Would you like this application to use your phone's GPS function?", "GPS", MessageBoxButton.OKCancel);
                        var currentSettings = settingsRepository.GetCurrentSettings();
                        currentSettings.IsGpsEnabled = result == MessageBoxResult.OK;
                        settingsRepository.UpdateCurrentSettings(currentSettings);
                    }

                    bool userAcceptedEula = false;
                    var store = IsolatedStorageSettings.ApplicationSettings;
                    if (store.Contains(EULA_KEY))
                        userAcceptedEula = (bool)store[EULA_KEY];

                    if (!userAcceptedEula)
                    {
                        NotificationTool.Show(
                            "Privacy Policy",
                            "NDG",
                            new NotificationAction("Accept", () => { AcceptedPrivacy(); }),
                            new NotificationAction("Decline", () => { DeclinedPrivacy(); }));
                    }

                    GpsTracker.Instance.GpsAllowed = settingsRepository.GetCurrentSettings().IsGpsEnabled;
                    GpsTracker.Instance.StartTracking();
                }                
            }

            if (Membership.CurrentUser == null)
            {
                this.Login = string.Empty;
                this.Password = string.Empty;
                using (var settingsRepository = new SettingsRepository())
                    this.ServerPath = settingsRepository.GetCurrentSettings().Server.Address;
            }

            var pageParameters = NavigationProvider.GetNavigationParameters();
            if (pageParameters.ContainsKey(SEVER_PATH_PARAMETER))
            {
                this.ServerPath = pageParameters[SEVER_PATH_PARAMETER];
            }
        }

        /// <summary>
        /// Stores accept option.
        /// </summary>
        private void AcceptedPrivacy()
        {
            var store = IsolatedStorageSettings.ApplicationSettings;
            if (store.Contains(EULA_KEY))
            {
                store[EULA_KEY] = true;
            }
            else
            {
                store.Add(EULA_KEY, true);
            }
        }

        /// <summary>
        /// Stores decline option.
        /// </summary>
        private void DeclinedPrivacy()
        {
            throw new Wp7EulaPopup.Exception.QuitException();
        }

        /// <summary>
        /// Executes login.
        /// </summary>
        private void LoginExecute()
        {
            this.BusyCount++;
            this.LoginCommand.RaiseCanExecuteChanged();

            #region prepare url
            string serverUrl = ServerPath;
            if (!ServerPath.StartsWith("http://") && !ServerPath.StartsWith("https://"))
            {
                serverUrl = string.Concat("http://", ServerPath);
            }
            if (!serverUrl.EndsWith("/"))
            {
                serverUrl = string.Concat(serverUrl, "/");
            }

            #endregion

            try
            {
                Membership.CheckUserData(Login, Password, serverUrl, LoginCallback);
            }
            catch
            {
                this.BusyCount--;
#if !UNIT_TEST
                MessageBox.Show((Application.Current.Resources["LanguageStrings"] as LanguageStrings).WRONG_SERVER_ADDRESS);
#else
                    RaiseTestCompleted("cannot locate server");
#endif
            }
            
        }

        private void LoginCallback(AuthentificationCode result)
        {
            this.BusyCount--;
            switch (result)
            {
                case AuthentificationCode.LoginSuccessed:
#if !UNIT_TEST
                    using (var settingsRepository = new SettingsRepository())
                        settingsRepository.UpdateCurrentServer(Membership.CurrentUser.ServerID);

                    using (var settingsRespository = new SettingsRepository())
                    {
                        if (settingsRespository.GetCurrentSettings().CheckForNewSurveys && InternetChecker.IsInernetActive)
                        {
                            Locator.MySurveysStatic.RefreshExecute();
                        }
                    }

                    NavigationProvider.Navigate(new System.Uri(HOME_PAGE_SOURCE, System.UriKind.Relative));
#else
                    RaiseTestCompleted("LoginSuccess");
#endif
                    break;
                case AuthentificationCode.InvalidCredentials:
#if !UNIT_TEST
                    MessageBox.Show((Application.Current.Resources["LanguageStrings"] as LanguageStrings).WRONG_PASSWORD);
#else
                    RaiseTestCompleted("Wrong username/password");
#endif
                    break;
                case AuthentificationCode.ServerNotFound:
                    MessageBox.Show((Application.Current.Resources["LanguageStrings"] as LanguageStrings).CANNOT_LOCATE_SERVER);
                    break;
                case AuthentificationCode.NoInternetConnection:
                    MessageBox.Show((Application.Current.Resources["LanguageStrings"] as LanguageStrings).ERROR_LOGIN_DB);
                    break;
            }  
        }

        /// <summary>
        /// Can execute command login or not.
        /// </summary>
        /// <returns>Bollean value.</returns>
        private bool LoginCanExecute()
        {
            return !string.IsNullOrEmpty(this.Login) && !string.IsNullOrEmpty(this.Password) && !string.IsNullOrEmpty(this.serverPath) && !this.IsBusy;
        }
    }
}
