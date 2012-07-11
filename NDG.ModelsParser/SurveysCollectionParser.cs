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
using NDG.DataAccessModels;
using System.IO;
using System.Xml.Linq;

namespace NDG.XFormsParser
{
    public class SurveysCollectionParser
    {
        public IEnumerable<string> GetSurveysDownloadUrlsFromStream(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var res = ParseXmlStringToSurveyDownloadUrls(reader.ReadToEnd());
                return res;
            }
        }

        private IEnumerable<string> ParseXmlStringToSurveyDownloadUrls(string xmlString)
        {
            XNamespace defaultNamespace = "http://openrosa.org/xforms/xformsList";

            XDocument documentXML = XDocument.Parse(xmlString);

            var root = documentXML.Element(defaultNamespace + "xforms");
            var surveys = root.Elements("xform");

            var result = new List<string>();
            foreach (XElement xSurvey in surveys)
            {
                result.Add(xSurvey.Element("downloadUrl").Value);
            }
            return result;
        }
    }
}
