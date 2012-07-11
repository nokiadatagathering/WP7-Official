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
    public interface ILanguageRepository
    {
        IEnumerable<Language> GetAllLanguages();
        Language GetLanguage(int languageID);
        Language GetLanguage(string culture);
        bool AddLanguagesCollection(IEnumerable<Language> languages);
        Language UpdateLanguagePath(int languageID, string path);
    }
}
