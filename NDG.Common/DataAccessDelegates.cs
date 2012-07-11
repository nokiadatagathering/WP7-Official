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
using System.Collections.Generic;
using NDG.Common;
using NDG.Helpers.Classes;

namespace NDG.StorageAccess
{
    public delegate void GetSurveysCallback(IEnumerable<Survey> surveys);
    public delegate void GetLanguageListCallback(IEnumerable<Language> surveys);
    public delegate void AuthentificateUserCallback(AuthentificationCode result);
    public delegate void UploadReposneSetCallback(bool result);
    public delegate void DownloadLanguageCallback(SerializableDictionary<string,string> languageStrings);
    public delegate void ChangeLanguageCallback(bool result);
}
