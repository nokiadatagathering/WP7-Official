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
using NDG.Helpers.Classes;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NDG.LanguageParser
{
    public class JavaMessagesParser
    {
        public static SerializableDictionary<string, string> GetLanguageStrings(Stream javaMessagesStream)
        {
            var result = new SerializableDictionary<string, string>();

            using (var reader = new StreamReader(javaMessagesStream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    var deviderIndex = line.IndexOf('=');
                    if (deviderIndex != -1 && line[0] != '#')
                    {
                        var text = line.Substring(deviderIndex + 1);

                        var regex = new Regex(@"\\u[A-Za-z0-9]{4}");

                        text = regex.Replace(text, m => ((Char)UInt16.Parse(m.Value.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier)).ToString());

                        var key = line.Substring(0, deviderIndex);
                        if (!result.ContainsKey(key))
                            result.Add(key, text);
                    }
                }
            }
            return result;
        }
    }
}
