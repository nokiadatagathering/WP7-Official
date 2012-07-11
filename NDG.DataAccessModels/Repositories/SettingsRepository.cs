using System;
using System.Linq;
using System.Data.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using NDG.DataAccessModels.Repositories.Interfaces;

namespace NDG.DataAccessModels.Repositories
{
    public class SettingsRepository : Repository, ISettingsRepository
    {
        public SettingsRepository()
        {
            var loadOptions = new DataLoadOptions();

            loadOptions.LoadWith<Settings>(s => s.Server);
            loadOptions.LoadWith<Settings>(s => s.Language);
            loadOptions.LoadWith<Settings>(s => s.PhotoResolution);

            _context.LoadOptions = loadOptions;
        }

        public Settings GetCurrentSettings()
        {
            return _context.Settings.FirstOrDefault(s => !s.IsDefault.HasValue || !s.IsDefault.Value);
        }

        public Settings GetDefaultSettings()
        {
            return _context.Settings.FirstOrDefault(s => s.IsDefault.HasValue && s.IsDefault.Value);
        }

        public Settings UpdateCurrentSettings(Settings settings)
        {
            UpdateCurrentSettingsInfo(settings);
            return GetCurrentSettings();
        }


        public Settings UpdateCurrentServer(int serverID)
        {
            var current = GetCurrentSettings();
            if (current.ServerID != serverID)
            {
                current.Server = _context.Server.First(s => s.ID == serverID);
                _context.SubmitChanges();
            }
            return current;
        }

        public Settings UpdateCurrentLanguage(int languageID)
        {
            var current = GetCurrentSettings();
            if (current.LanguageID != languageID)
            {
                current.Language = _context.Language.First(s => s.ID == languageID);
                _context.SubmitChanges();
            }
            return current;
        }

        public Settings UpdateCurrentPhotoResolution(int photoResolutionID)
        {
            var current = GetCurrentSettings();
            if (current.PhotoResolutionID != photoResolutionID)
            {
                current.PhotoResolution = _context.PhotoResolution.First(p => p.ID == photoResolutionID);
                _context.SubmitChanges();
            }
            return current;
        }


        public System.Collections.Generic.IEnumerable<PhotoResolution> GetAvailablePhotoResolutions()
        {
            return _context.PhotoResolution;
        }


        public Settings ResetCurrentSettings()
        {
            var settings = GetDefaultSettings();
            UpdateCurrentSettingsInfo(settings);
            return GetCurrentSettings();
        }

        private void UpdateCurrentSettingsInfo(Settings settings)
        {
            UpdateCurrentLanguage(settings.LanguageID);
            UpdateCurrentPhotoResolution(settings.PhotoResolutionID);
            UpdateCurrentServer(settings.ServerID);
            var currentSettings = GetCurrentSettings();
            currentSettings.CheckForNewSurveys = settings.CheckForNewSurveys;
            currentSettings.IsGpsEnabled = settings.IsGpsEnabled;
            _context.SubmitChanges();
        }

        public ChangeSet GetChangeSet()
        {
            return _context.GetChangeSet();
        }
    }
}
