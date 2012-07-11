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

namespace NDG.StorageAccess
{
    public static class ServerMethodAddresses
    {
        public static readonly string CheckForNewSurveys = "ReceiveSurvey";
        public static readonly string AuthentificateUser = "checkAuthorization";
        public static readonly string UploadResponseSet = "PostResults";
        public static readonly string GetLanguageList = "LanguageList";
        public static readonly string DownloadLanguage = "LocalizationServing/text?locale=";
    }
}
