using System;
using System.Linq;
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
using NDG.DataAccessModels;
using System.IO;

namespace NDG.LanguageParser
{
    public class LanguageListParser
    {
        public static List<Language> GetLanguagesList(Stream languagesListStream)
        {
            var result = new List<Language>();
            using (var reader = new StreamReader(languagesListStream))
            {
                while (!reader.EndOfStream)
                {
                    var languageString = new string(reader.ReadLine().Reverse().ToArray());
                    var languageCulture = new string(languageString.TakeWhile(c => c != ' ').Reverse().ToArray());
                    var languageName = new string(languageString.Substring(languageCulture.Length + 1).Reverse().ToArray());
                    result.Add(new Language { Name = languageName, Culture = languageCulture, Path = string.Empty });
                }
            }
            return result;
        }
    }
}
