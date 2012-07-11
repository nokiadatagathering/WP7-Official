using System.Collections.ObjectModel;
using System.Linq;
using NDG.ViewModels.Helpers;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using NDG.DataAccessModels;
using NDG.BussinesLogic.Providers;
using NDG.DataAccessModels.Repositories;
using System.Collections.Generic;
using NDG.Helpers.Classes;
using System.Text;
using System;
using System.Text.RegularExpressions;
using NDG.Common;
using NDG.DataAccessModels.DataModels;
using NDG.Helpers.Controls;

namespace NDG.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        #region Constants

        private  string WRONG_SERVER =(Application.Current.Resources["LanguageStrings"] as LanguageStrings).WRONG_SERVER_ADDRESS;
        private  string LOGIN_SOURCE = "/Views/LoginPage.xaml?serverPath={0}";
        private  string NETWORK_UNVAIBLE =(Application.Current.Resources["LanguageStrings"] as LanguageStrings).CANNOT_LOCATE_SERVER;
        private string DEFAULT_SETTINGS_MESSAGE = (Application.Current.Resources["LanguageStrings"] as LanguageStrings).REVERT_SETTINGS_CONFIRMATION;
        private  string UNABLE_DOWNLOAD_LANGUAGE = (Application.Current.Resources["LanguageStrings"] as LanguageStrings).DOWNLOAD_LOCALE_FAILED;

        #endregion Constants

        #region Fields

        private bool isNeedUpdate = false;
        private Settings currentSettings = new Settings();
        private bool isGpsEnabled;
        private bool isCheckForSurveysOnStart;
        private List<string> enabledProptocols = new List<string>();
        private ServerRepository serverRepository;
        private SettingsRepository settingsRepository;
        private string serverPath = string.Empty;
        private Language currentLanguage = null;
        private PhotoResolution selectedResolution = null;
        private ObservableCollection<PhotoResolution> resolutions = new ObservableCollection<PhotoResolution>();
        private ObservableCollection<Language> languages = new ObservableCollection<Language>();

        #endregion Fields

        public SettingsViewModel()
        {
            this.enabledProptocols.Add("http://");
            this.enabledProptocols.Add("https://");
            this.EnabledProtocols = this.enabledProptocols;

            this.InitializeViewModelCommand = new RelayCommand(this.InitializeViewModelExecute);
            this.SaveSettingsCommand = new RelayCommand(this.SaveSettingsExecute, this.SaveSettingsCanExecute);
            this.RevertCommand = new RelayCommand(this.RevertExecute);
            this.NavigationBackCommand = new RelayCommand(this.NavigationBackExecute);
        }

        #region Commands

        public RelayCommand NavigationBackCommand { get; private set; }

        public RelayCommand RevertCommand { get; private set; }

        public RelayCommand SaveSettingsCommand { get; private set; }

        #endregion Commands

        #region Properties

        public bool IsNeedUpdate
        {
            get { return this.isNeedUpdate; }
            set { this.isNeedUpdate = value; this.RaisePropertyChanged("IsNeedUpdate"); }
        }

        public bool IsCheckForNewSurveysOnStart
        {
            get { return this.isCheckForSurveysOnStart; }
            set { this.isCheckForSurveysOnStart = value; this.RaisePropertyChanged("IsCheckForNewSurveysOnStart"); this.SaveSettingsCommand.RaiseCanExecuteChanged(); }
        }

        public bool IsGpsEnabled
        {
            get
            {
                return this.isGpsEnabled;
            }

            set
            {
                this.isGpsEnabled = value;
                RaisePropertyChanged("IsGpsEnabled");
                this.SaveSettingsCommand.RaiseCanExecuteChanged();
            }
        }

        public List<string> EnabledProtocols
        {
            get { return this.enabledProptocols; }
            set { this.enabledProptocols = value; this.RaisePropertyChanged("EnabledProtocols"); }
        }

        public string ServerPath
        {
            get { return this.serverPath; }
            set
            {
                this.serverPath = value;
                this.RaisePropertyChanged("ServerPath");
                this.SaveSettingsCommand.RaiseCanExecuteChanged();
                this.RevertCommand.RaiseCanExecuteChanged();
            }
        }

        public Language CurrentLanguage
        {
            get { return this.currentLanguage; }
            set { this.currentLanguage = value; this.RaisePropertyChanged("CurrentLanguage"); }
        }

        public ObservableCollection<Language> Languages
        {
            get { return this.languages; }
            set { this.languages = value; this.RaisePropertyChanged("Languages"); }
        }

        public ObservableCollection<PhotoResolution> Resolutions
        {
            get { return this.resolutions; }
            private set { this.resolutions = value; this.RaisePropertyChanged("Resolutions"); }
        }

        public PhotoResolution SelectedResolution
        {
            get { return this.selectedResolution; }
            set { this.selectedResolution = value; this.RaisePropertyChanged("SelectedResolution"); this.SaveSettingsCommand.RaiseCanExecuteChanged(); }
        }

        #endregion Properties

        #region Executes

        private void RevertExecute()
        {
            ConfirmationBox box = new ConfirmationBox();
            box.Message = DEFAULT_SETTINGS_MESSAGE;
            box.DialogCompleted += this.OnDialogCompleted;
            box.Show();            
        }

        public void InitializeViewModelExecute()
        {
            this.serverRepository = new ServerRepository();            
            this.settingsRepository = new SettingsRepository();

            currentSettings = settingsRepository.GetCurrentSettings();
            this.Refresh();
        }

        private void LanguageSetCallback(bool result)
        {
            --BusyCount;
            if (result)
            {
                this.ServerPath = this.ServerPath.Trim();
                bool isPathChanged = IsServerPathChanged();
                bool isResolutionChanged = IsResolutionChanged();
                Settings settings = new Settings()
                {
                    IsGpsEnabled = this.IsGpsEnabled,
                    CheckForNewSurveys = this.IsCheckForNewSurveysOnStart,
                    LanguageID = this.CurrentLanguage.ID,
                    PhotoResolutionID = this.SelectedResolution.ID,
                    ServerID = currentSettings.Server.ID
                };

                if (IsServerPathChanged())
                {
                    Server newServer = serverRepository.TryGetServerByAddress(this.ServerPath);
                    if (newServer == null)
                    {
                        newServer = serverRepository.CreateServerByAddress(this.ServerPath);
                    }

                    settings.ServerID = newServer.ID;
                }

                this.ChangeSettings(settings, isPathChanged, isResolutionChanged);
                (Application.Current.Resources["LanguageStrings"] as LanguageStrings).RaiseAllPropertyChanged();
            }
            else
            {
                MessageBox.Show(UNABLE_DOWNLOAD_LANGUAGE);
            }

            this.SaveSettingsCommand.RaiseCanExecuteChanged();
        }

        private void SaveSettingsExecute()
        {
            ++BusyCount;
            LanguageProvider.SetCurrentLanguage(settingsRepository.GetCurrentSettings().Server.Address,CurrentLanguage,LanguageSetCallback);
            this.SaveSettingsCommand.RaiseCanExecuteChanged();
        }

        private void NavigationBackExecute()
        {
            this.serverRepository.Dispose();
            this.settingsRepository.Dispose();
        }

        #endregion Executes

        #region CanExecute

        private bool SaveSettingsCanExecute()
        {
            return IsServerPathChanged() || IsGpsChanged() || IsCheckinForSurveysChanged()
                || IsResolutionChanged() || IsLanguageChanged() || !IsBusy;
        }

        #endregion CanExecute

        #region Methods

        private void OnDialogCompleted(object sender, ConfirmationResulEventArgs e)
        {
            if (e.DialogResult == Coding4Fun.Phone.Controls.PopUpResult.Ok)
            {                
                this.currentSettings = settingsRepository.ResetCurrentSettings();
                bool isServicePathChanged = this.currentSettings.Server.Address != this.ServerPath;
                SyncContext.Post((parameter) =>
                    {
                        if (isServicePathChanged)
                        {
                            Membership.ResetCurrentUser();
                            string navigationSource = string.Format(LOGIN_SOURCE, this.currentSettings.Server.Address);
                            NavigationProvider.Navigate(new System.Uri(navigationSource, System.UriKind.Relative));
                            Locator.Cleanup();
                        }
                        else
                        {
                            this.Refresh();
                        }

                    }, null);
            }
        }

        private void ChangeSettings(Settings settings, bool isPathChanged, bool isResolutionChanged)
        {
            this.settingsRepository.UpdateCurrentSettings(settings);

            if (isResolutionChanged)
            {
                ImageQuestionData.CurrentResolution = this.SelectedResolution;
            }

            GpsTracker.Instance.GpsAllowed = settings.IsGpsEnabled;

            this.Refresh();

            if (isPathChanged)
            {
                Membership.ResetCurrentUser();
                string navigationSource = string.Format(LOGIN_SOURCE, this.serverPath);
                NavigationProvider.Navigate(new System.Uri(navigationSource, System.UriKind.Relative));
                Locator.Cleanup();
            }
        }

        private void SaveSettingsCallback(bool result)
        {
            (Application.Current.Resources["LanguageStrings"] as LanguageStrings).RaiseAllPropertyChanged();
            --BusyCount;
        }

        private void Refresh()
        {
            this.settingsRepository = new SettingsRepository();
            this.currentSettings = settingsRepository.GetCurrentSettings();
            this.Resolutions = new ObservableCollection<PhotoResolution>(settingsRepository.GetAvailablePhotoResolutions());

            this.IsCheckForNewSurveysOnStart = this.currentSettings.CheckForNewSurveys;
            this.IsGpsEnabled = this.currentSettings.IsGpsEnabled;
            this.SelectedResolution = this.Resolutions.SingleOrDefault(item => item.ID == this.currentSettings.PhotoResolutionID);
            this.UpdateLanguages();
            this.ServerPath = this.currentSettings.Server != null ? this.currentSettings.Server.Address : string.Empty;
            this.IsNeedUpdate = true;
            this.IsNeedUpdate = false;
        }

        private void UpdateLanguages()
        {
            using (var languageRepository = new LanguageRepository())
                Languages = new ObservableCollection<Language>(languageRepository.GetAllLanguages());

            InitializeSelectedLanguage();

            if (InternetChecker.IsInernetActive)
            {
                this.BusyCount++;
                LanguageProvider.GetLanguagesList(currentSettings.Server.Address, GetLanguagesListCallback);
            }
        }

        private void GetLanguagesListCallback(IEnumerable<Language> languages)
        {
            Languages = new ObservableCollection<Language>(languages);
            InitializeSelectedLanguage();
#if UNIT_TEST
            RaiseTestCompleted("REFRESHED");
#endif
            this.BusyCount--;
        }

        private void InitializeSelectedLanguage()
        {
            CurrentLanguage = Languages.First(l => l.ID == currentSettings.LanguageID);
        }        

        #endregion Methods

        #region IsChanged

        private bool IsServerPathChanged()
        {
            return !string.IsNullOrWhiteSpace(this.ServerPath) && !string.IsNullOrEmpty(this.ServerPath.Trim())
                && this.currentSettings.Server != null
                && this.ServerPath != this.currentSettings.Server.Address;
        }

        private bool IsGpsChanged()
        {
            return this.isGpsEnabled != currentSettings.IsGpsEnabled;
        }

        private bool IsCheckinForSurveysChanged()
        {
            return this.isCheckForSurveysOnStart != currentSettings.CheckForNewSurveys;
        }

        private bool IsLanguageChanged()
        {
            return this.CurrentLanguage != null &&
                this.CurrentLanguage.ID != currentSettings.LanguageID;
        }

        private bool IsResolutionChanged()
        {
            return this.SelectedResolution != null &&
                this.SelectedResolution.ID != currentSettings.PhotoResolutionID;
        }

        #endregion IsChanged
    }
}
