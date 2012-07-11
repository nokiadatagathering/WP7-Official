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
using NDG.DataAccessModels.Repositories;
using System.Xml.Linq;
using NDG.XFormsParser;
using Microsoft.Phone.Info;
using NDG.Helpers.Classes;

namespace NDG.ModelsParser
{
    public class ResponseSetParser
    {
        public string ParseResponseSetToRequestXml(int responseSetID)
        {
            using (var responseSetRepository = new ResponseSetRepository())
            {
                var responseSet = responseSetRepository.GetResponseSetForUserByID(responseSetID);

                XDocument resultDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                XElement root = new XElement("data", new XAttribute("id", responseSet.Survey.SystemID), new XAttribute(XNamespace.Xmlns + "orx", Namespaces.JavaRosaMetaDataNamespace));

                #region meta tag creation
                XElement meta = new XElement(Namespaces.JavaRosaMetaDataNamespace + "meta");
                root.Add(meta);
                meta.Add(new XElement(Namespaces.JavaRosaMetaDataNamespace + "instanceID")
                {
                    Value = responseSet.SystemID,

                });
                meta.Add(new XElement(Namespaces.JavaRosaMetaDataNamespace + "deviceID")
                {
                    Value = Convert.ToBase64String((byte[])DeviceExtendedProperties.GetValue("DeviceUniqueId"))
                });
                meta.Add(new XElement(Namespaces.JavaRosaMetaDataNamespace + "timeStart")
                {
                    Value = responseSet.DateSaved.Value.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture)
                });
                meta.Add(new XElement(Namespaces.JavaRosaMetaDataNamespace + "timeEnd")
                {
                    Value = responseSet.DateModified.Value.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture)
                });
                if (GpsTracker.Instance.UserLocation != null && new SettingsRepository().GetCurrentSettings().IsGpsEnabled)
                    meta.Add(new XElement(Namespaces.JavaRosaMetaDataNamespace + "geostamp")
                    {
                        Value = string.Format("{0} {1}", GpsTracker.Instance.UserLocation.Latitude, GpsTracker.Instance.UserLocation.Longitude),
                    });
                #endregion

                foreach (var category in responseSet.Survey.Category)
                {
                    XElement categoryXElement = new XElement(category.SystemID);
                    foreach (var question in category.Question)
                    {
                        XElement questionXElement = new XElement(question.SystemID);

                        var answer = responseSetRepository.GetQuestionAnswerByQuestionAndResponseSet(question.ID, responseSetID);

                        if (answer != null)
                            questionXElement.Value = answer.AnswerText;
                        else
                            questionXElement.Value = question.Data.GetResult();

                        categoryXElement.Add(questionXElement);
                    }
                    root.Add(categoryXElement);
                }

                return root.ToString();
            }
        }
    }
}
