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
using System.Collections.Generic;

namespace NDG.DataAccessModels.Repositories.Interfaces
{
    public interface ISettingsRepository
    {
        Settings GetCurrentSettings();
        IEnumerable<PhotoResolution> GetAvailablePhotoResolutions();
        Settings GetDefaultSettings();
        Settings ResetCurrentSettings();
        Settings UpdateCurrentSettings(Settings settings);
        Settings UpdateCurrentServer(int serverID);
        Settings UpdateCurrentLanguage(int languageID);
        Settings UpdateCurrentPhotoResolution(int photoResolutionID);
    }
}
