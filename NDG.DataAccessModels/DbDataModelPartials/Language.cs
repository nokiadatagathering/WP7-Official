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
using NDG.DataAccessModels.DataModels;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using NDG.Helpers.Classes;

namespace NDG.DataAccessModels
{
    public partial class Language
    {
        public SerializableDictionary<string, string> Strings { get; set; }

        public void LoadLanguageStrings()
        {
            if (!string.IsNullOrEmpty(Path) && Strings == null)
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                    if (store.FileExists(Path))
                        using (IsolatedStorageFileStream fs = new IsolatedStorageFileStream(Path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read, store))
                            Strings = new TypedXmlSerializer<SerializableDictionary<string, string>>().Deserialize(fs);
        }

        public string TryGetString(string key)
        {
            if (this.Strings != null && this.Strings.ContainsKey(key))
                return this.Strings[key];
            else return string.Empty;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
