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
using System.IO.IsolatedStorage;

namespace NDG.Helpers.Classes
{
    /// <summary>
    /// Helper for copying files from install package to isolated storage
    /// </summary>
    public class CopyToIsolatedStorageHelper
    {
        public static void CopyToIsolatedStorage(string packageFilename, string isoFilename)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                if (!store.FileExists(isoFilename))
                    using (var stream = System.Windows.Application.GetResourceStream(new Uri(packageFilename, UriKind.Relative)).Stream)
                    using (IsolatedStorageFileStream dest = new IsolatedStorageFileStream(isoFilename, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, store))
                    {
                        stream.Position = 0;
                        stream.CopyTo(dest);
                        dest.Flush();
                    }
        }
    }
}
