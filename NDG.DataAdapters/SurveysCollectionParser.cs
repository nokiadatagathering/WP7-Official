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

namespace NDG.DataAdapters
{
    public class SurveysCollectionParser
    {
        public IEnumerable<Survey> GetSurveysCollectionFromStream(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var res= ParseXmlStringToSurveyCollection(reader.ReadToEnd());
                return res;
            }
        }

        private IEnumerable<Survey> ParseXmlStringToSurveyCollection(string xmlString)
        {
            XNamespace defaultNamespace = "http://openrosa.org/xforms/xformsList";

            XDocument documentXML = XDocument.Parse(xmlString);

            var root = documentXML.Element(defaultNamespace + "xforms");
            var surveys = root.Elements("xform");

            var res = new List<Survey>();
            foreach (XElement xSurvey in surveys)
            {
               res.Add( new Survey
                {
                    Name = xSurvey.Element("name").Value,
                });
              
                //SurveyBasicInfo survey = new SurveyBasicInfo() { Name = xSurvey.Element("name").Value, SurveyId = xSurvey.Element("formID").Value };
            }
            return res;
        }
    }
}
