using System;
using System.Net;
using System.Linq;
using System.Data.Linq;
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
    public class LanguageRepository : Repository, ILanguageRepository
    {

        public System.Collections.Generic.IEnumerable<Language> GetAllLanguages()
        {
            return _context.Language;
        }

        public Language GetLanguage(int languageID)
        {
            return _context.Language.FirstOrDefault(l => l.ID == languageID);
        }

        public Language GetLanguage(string culture)
        {
            return _context.Language.FirstOrDefault(l => l.Culture.Equals(culture));
        }


        public bool AddLanguagesCollection(System.Collections.Generic.IEnumerable<Language> languages)
        {
            try
            {
                _context.Language.InsertAllOnSubmit(languages);
                _context.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public Language UpdateLanguagePath(int languageID, string path)
        {
            try
            {
                var language = GetLanguage(languageID);
                language.Path = path;
                _context.SubmitChanges();
                return language;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
    }
}
