using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using NDG.DataAccessModels;
using NDG.Common;
using NDG.StorageAccess;
using NDG.DataAccessModels.Repositories.Interfaces;
using NDG.DataAccessModels.Repositories;
using NDG.StorageAccess.DataServices;
using NDG.Helpers.Classes;
using System.IO.IsolatedStorage;
using NDG.DataAccessModels.DataModels;
using System.Threading;
using System.Globalization;

namespace NDG.BussinesLogic.Providers
{
    public static class LanguageProvider
    {
        private static GetLanguageListCallback _getLanguagesListCallback;
        private static ChangeLanguageCallback _changeLanguageCallback;

        private static ILanguageRepository _languageRepository;
        private static Language _languageToSet;

        private static Language _currentLanguage;
        public static Language CurrentLanguage
        {
            get
            {
                if (_currentLanguage == null)
                {
                    using (var settingsRepository = new SettingsRepository())
                        _currentLanguage = settingsRepository.GetCurrentSettings().Language;
                    ChangeCurrentCulture(_currentLanguage);
                    _currentLanguage.LoadLanguageStrings();
                }
                return _currentLanguage;
            }
            set
            {
                value.LoadLanguageStrings();
                ChangeCurrentCulture(value);

                _currentLanguage = value;
            }
        }

        private static void ChangeCurrentCulture(Language value)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(value.Culture);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(value.Culture);
            }
            catch (Exception)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            }
        }

        static LanguageProvider()
        {
            _languageRepository = new LanguageRepository();
        }

        public static void GetLanguagesList(string serverUrl, GetLanguageListCallback _callback)
        {
            _getLanguagesListCallback = _callback;
            if (InternetChecker.IsInernetActive)
            {
                new LanguageDataService().GetAllLanguages(serverUrl, GetLanguagesListCallback);
            }
            InvokeCallbackWithLanguageListParameter(_languageRepository.GetAllLanguages());
        }

        private static void GetLanguagesListCallback(IEnumerable<Language> languages)
        {
            if (languages != null)
            {
                var newLanguages = new List<Language>();
                var existingLanguageCultures = _languageRepository.GetAllLanguages().Select(l => l.Culture);
                foreach (var language in languages)
                {
                    if (!existingLanguageCultures.Contains(language.Culture))
                        newLanguages.Add(language);
                }
                _languageRepository.AddLanguagesCollection(newLanguages);
            }
            InvokeCallbackWithLanguageListParameter(_languageRepository.GetAllLanguages());
        }

        private static void InvokeCallbackWithLanguageListParameter(IEnumerable<Language> param)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                _getLanguagesListCallback.Invoke(param);
            });
        }

        public static void SetCurrentLanguage(string serverUrl, Language language, ChangeLanguageCallback _callback)
        {
            _changeLanguageCallback = _callback;
            _languageToSet = language;
            if (string.IsNullOrEmpty(language.Path))
                new LanguageDataService().DownloadLanguage(serverUrl, _languageToSet, DownloadLanguageCallback);
            else
            {
                CurrentLanguage = _languageToSet;
                _changeLanguageCallback.Invoke(true);
            }
        }

        public static void DownloadLanguageCallback(SerializableDictionary<string, string> languageStrings)
        {
            var languagePath = string.Concat(_languageToSet.Name, "_", _languageToSet.Culture, ".xml");
            CreateLanguageStringsFile(languageStrings, languagePath);
            _languageToSet = _languageRepository.UpdateLanguagePath(_languageToSet.ID, languagePath);
            if (_languageToSet != null)
            {
                CurrentLanguage = _languageToSet;
                _changeLanguageCallback.Invoke(true);
                return;
            }
            _changeLanguageCallback.Invoke(false);
        }

        private static void CreateLanguageStringsFile(SerializableDictionary<string, string> languageStrings, string languagePath)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            using (IsolatedStorageFileStream fs = store.CreateFile(languagePath))
                new TypedXmlSerializer<SerializableDictionary<string, string>>().Serialize(fs, languageStrings);
        }


    }
}
