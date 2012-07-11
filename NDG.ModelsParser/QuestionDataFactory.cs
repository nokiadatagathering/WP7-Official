using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using NDG.DataAccessModels;
using NDG.XFormsParser;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NDG.DataAccessModels.DataModels;
using NDG.Helpers.Classes;
using System.Globalization;
using System.Text;

namespace NDG.ModelsParser
{
    public class QuestionDataFactory
    {

        public static string GetRelevantString(XElement questionIterator, Category parent, XDocument xmlDocument)
        {
            var questionId = questionIterator.Name.LocalName;

            var constraint = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                     .Element(Namespaces.DefaultNamespace + "model").Elements(Namespaces.DefaultNamespace + "bind").ToList()
                     .Where(e => e.Attribute("nodeset").Value.Equals(string.Format("/data/{0}/{1}", parent.SystemID, questionId))).Single().Attribute("relevant");
            return constraint != null ? constraint.Value : null;

        }

        public static string CreateDescriptiveQuestion(XElement questionIterator, Category parent, XDocument xmlDocument)
        {
            var questionId = questionIterator.Name.LocalName;

            var constraint = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                    .Element(Namespaces.DefaultNamespace + "model").Elements(Namespaces.DefaultNamespace + "bind").ToList()
                    .Where(e => e.Attribute("nodeset").Value.Equals(string.Format("/data/{0}/{1}", parent.SystemID, questionId))).Single().Attribute("constraint");

            var length = 255;

            if (constraint != null)
            {
                var contraints = GetConstraints(constraint.Value, @"string-length\( \. \) <=(?'length'\d+)");
                var lengthString = contraints["length"];

                if (!string.IsNullOrEmpty(lengthString))
                    length = int.Parse(lengthString);
            }

            var defaultAnswer = questionIterator.Value;

            var label = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                    .Element(Namespaces.DefaultNamespace + "model").Element(Namespaces.DefaultNamespace + "itext").Element(Namespaces.DefaultNamespace + "translation")
                    .Elements().ToList().Where(e => e.Attribute("id").Value.Equals(string.Format("/data/{0}/{1}:label", parent.SystemID, questionId))).Single().Element(Namespaces.DefaultNamespace + "value").Value;
            var descriptiveQuestionData = new DescriptiveQuestionData
            {
                Label = label,
                Answer = defaultAnswer,
                MaxLength = length,
            };
            return new TypedXmlSerializer<DescriptiveQuestionData>().SerializeToXmlString(descriptiveQuestionData);
        }

        public static string CreateIntegerQuestion(XElement questionIterator, Category parent, XDocument xmlDocument)
        {
            var questionId = questionIterator.Name.LocalName;

            var constraint = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                    .Element(Namespaces.DefaultNamespace + "model").Elements(Namespaces.DefaultNamespace + "bind").ToList()
                    .Where(e => e.Attribute("nodeset").Value.Equals(string.Format("/data/{0}/{1}", parent.SystemID, questionId))).Single().Attribute("constraint");

            int? min = null, max = null;

            if (constraint != null)
            {
                var contraints = GetConstraints(constraint.Value, @"\((\. >= (?'min'\d+(\.\d+)?))?( and )?(\. <= (?'max'\d+(\.\d+)?))?\)");

                var minString = contraints["min"];
                var maxString = contraints["max"];

                if (!string.IsNullOrEmpty(minString))
                    min = int.Parse(minString);

                if (!string.IsNullOrEmpty(maxString))
                    max = int.Parse(maxString);
            }

            var intialDefaultAnswer = 0;
            int? defaultAnswer = null;
            if (Int32.TryParse(questionIterator.Value, out intialDefaultAnswer))
            {
                defaultAnswer = intialDefaultAnswer;
            }

            var label = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                    .Element(Namespaces.DefaultNamespace + "model").Element(Namespaces.DefaultNamespace + "itext").Element(Namespaces.DefaultNamespace + "translation")
                    .Elements().ToList().Where(e => e.Attribute("id").Value.Equals(string.Format("/data/{0}/{1}:label", parent.SystemID, questionId))).Single().Element(Namespaces.DefaultNamespace + "value").Value;

            var integerQuestionData = new IntegerQuestionData
            {
                Label = label,
                Answer = defaultAnswer,
                MinValue = min,
                MaxValue = max,
            };

            return new TypedXmlSerializer<IntegerQuestionData>().SerializeToXmlString(integerQuestionData);
        }

        public static string CreateDecimalQuestion(XElement questionIterator, Category parent, XDocument xmlDocument)
        {
            var questionId = questionIterator.Name.LocalName;

            var constraint = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                    .Element(Namespaces.DefaultNamespace + "model").Elements(Namespaces.DefaultNamespace + "bind").ToList()
                    .Where(e => e.Attribute("nodeset").Value.Equals(string.Format("/data/{0}/{1}", parent.SystemID, questionId))).Single().Attribute("constraint");

            decimal? min = null, max = null;

            if (constraint != null)
            {
                var contraints = GetConstraints(constraint.Value, @"\((\. >= (?'min'\d+(\.\d+)?))?( and )?(\. <= (?'max'\d+(\.\d+)?))?\)");

                var minString = contraints["min"];
                var maxString = contraints["max"];

                if (!string.IsNullOrEmpty(minString))
                    min = decimal.Parse(minString);

                if (!string.IsNullOrEmpty(maxString))
                    max = decimal.Parse(maxString);
            }

            var intialDefaultAnswer = 0.0M;
            Decimal? defaultAnswer = null;
            if (Decimal.TryParse(questionIterator.Value, out intialDefaultAnswer))
            {
                defaultAnswer = intialDefaultAnswer;
            }

            var label = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                    .Element(Namespaces.DefaultNamespace + "model").Element(Namespaces.DefaultNamespace + "itext").Element(Namespaces.DefaultNamespace + "translation")
                    .Elements().ToList().Where(e => e.Attribute("id").Value.Equals(string.Format("/data/{0}/{1}:label", parent.SystemID, questionId))).Single().Element(Namespaces.DefaultNamespace + "value").Value;

            var decimalQuestionData = new DecimalQuestionData
            {
                Label = label,
                Answer = defaultAnswer,
                MinValue = min,
                MaxValue = max,
            };

            return new TypedXmlSerializer<DecimalQuestionData>().SerializeToXmlString(decimalQuestionData);
        }

        public static string CreateMultipleChoiceQuestion(XElement questionIterator, Category parent, XDocument xmlDocument)
        {
            var questionId = questionIterator.Name.LocalName;

            var label = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                    .Element(Namespaces.DefaultNamespace + "model").Element(Namespaces.DefaultNamespace + "itext").Element(Namespaces.DefaultNamespace + "translation")
                    .Elements().ToList().Where(e => e.Attribute("id").Value.Equals(string.Format("/data/{0}/{1}:label", parent.SystemID, questionId))).Single().Element(Namespaces.DefaultNamespace + "value").Value;



            var choiceElements = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                    .Element(Namespaces.DefaultNamespace + "model").Element(Namespaces.DefaultNamespace + "itext").Element(Namespaces.DefaultNamespace + "translation")
                    .Elements().ToList().Where(e => e.Attribute("id").Value.StartsWith(string.Format("/data/{0}/{1}:option", parent.SystemID, questionId)));

            var defaultAnswers = questionIterator.Value.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);

            var multipleQuestion = new MultipleChoiceQuestionData
            {
                Label = label,
                Answers = new System.Collections.ObjectModel.ObservableCollection<ChoiceTextValuePair>(),
            };

            foreach (var item in choiceElements)
            {
                var itemIdTest = GetConstraints(item.Attribute("id").Value, string.Format(@"/data/{0}/{1}:(?'id'\w+)", parent.SystemID, questionId));
                var itemId = itemIdTest["id"];

                var itemText = item.Element(Namespaces.DefaultNamespace + "value").Value;

                var itemValue = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "body")
                   .Elements(Namespaces.DefaultNamespace + "group").ToList().Where(e => e.Element(Namespaces.DefaultNamespace + "label").Attribute("ref").Value.Equals(string.Format("jr:itext('/data/{0}:label')", parent.SystemID))).Single()
                   .Elements(Namespaces.DefaultNamespace + "select").ToList().Where(e => e.Attribute("ref").Value.Equals(string.Format("/data/{0}/{1}", parent.SystemID, questionId))).Single()
                   .Elements(Namespaces.DefaultNamespace + "item").ToList().Where(e => e.Element(Namespaces.DefaultNamespace + "label").Attribute("ref").Value.Equals(string.Format("jr:itext('/data/{0}/{1}:{2}')", parent.SystemID, questionId, itemId))).Single()
                   .Element(Namespaces.DefaultNamespace + "value").Value;

                var choiceItem = new ChoiceTextValuePair
                {
                    Text = itemText,
                    Value = itemValue,
                };

                multipleQuestion.Options.Add(choiceItem);
                if (defaultAnswers.Contains(choiceItem.Value))
                    multipleQuestion.Answers.Add(choiceItem);
            }

            return new TypedXmlSerializer<MultipleChoiceQuestionData>().SerializeToXmlString(multipleQuestion);
        }

        public static string CreateExclusiveChoiceQuestion(XElement questionIterator, Category parent, XDocument xmlDocument)
        {
            var questionId = questionIterator.Name.LocalName;

            var label = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                    .Element(Namespaces.DefaultNamespace + "model").Element(Namespaces.DefaultNamespace + "itext").Element(Namespaces.DefaultNamespace + "translation")
                    .Elements().ToList().Where(e => e.Attribute("id").Value.Equals(string.Format("/data/{0}/{1}:label", parent.SystemID, questionId))).Single().Element(Namespaces.DefaultNamespace + "value").Value;


            var choiceElements = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                    .Element(Namespaces.DefaultNamespace + "model").Element(Namespaces.DefaultNamespace + "itext").Element(Namespaces.DefaultNamespace + "translation")
                    .Elements().ToList().Where(e => e.Attribute("id").Value.StartsWith(string.Format("/data/{0}/{1}:option", parent.SystemID, questionId)));

            var defaultAnswer = questionIterator.Value.Trim();


            var exclusiveQuestion = new ExclusiveChocieQuestionData
            {
                Label = label,

            };

            foreach (var item in choiceElements)
            {
                var itemIdTest = GetConstraints(item.Attribute("id").Value, string.Format(@"/data/{0}/{1}:(?'id'\w+)", parent.SystemID, questionId));
                var itemId = itemIdTest["id"];

                var itemText = item.Element(Namespaces.DefaultNamespace + "value").Value;
                var itemValue = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "body")
                    .Elements(Namespaces.DefaultNamespace + "group").ToList().Where(e => e.Element(Namespaces.DefaultNamespace + "label").Attribute("ref").Value.Equals(string.Format("jr:itext('/data/{0}:label')", parent.SystemID))).Single()
                    .Elements(Namespaces.DefaultNamespace + "select1").ToList().Where(e => e.Attribute("ref").Value.Equals(string.Format("/data/{0}/{1}", parent.SystemID, questionId))).Single()
                    .Elements(Namespaces.DefaultNamespace + "item").ToList().Where(e => e.Element(Namespaces.DefaultNamespace + "label").Attribute("ref").Value.Equals(string.Format("jr:itext('/data/{0}/{1}:{2}')", parent.SystemID, questionId, itemId))).Single()
                    .Element(Namespaces.DefaultNamespace + "value").Value;

                var choiceItem = new ChoiceTextValuePair
                {
                    Text = itemText,
                    Value = itemValue,

                };

                exclusiveQuestion.Options.Add(choiceItem);
                if (choiceItem.Value.Equals(defaultAnswer))
                    exclusiveQuestion.Answer = choiceItem;
            }

            return new TypedXmlSerializer<ExclusiveChocieQuestionData>().SerializeToXmlString(exclusiveQuestion);
        }

        public static string CreateTimeQuestion(XElement questionIterator, Category parent, XDocument xmlDocument)
        {
            var questionId = questionIterator.Name.LocalName;

            DateTime? defaultAnswer = null;
            if (!string.IsNullOrEmpty(questionIterator.Value))
                defaultAnswer = DateTime.ParseExact(questionIterator.Value, "hh:mm:ss", CultureInfo.InvariantCulture);

            var label = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                    .Element(Namespaces.DefaultNamespace + "model").Element(Namespaces.DefaultNamespace + "itext").Element(Namespaces.DefaultNamespace + "translation")
                    .Elements().ToList().Where(e => e.Attribute("id").Value.Equals(string.Format("/data/{0}/{1}:label", parent.SystemID, questionId))).Single().Element(Namespaces.DefaultNamespace + "value").Value;

            var timeQuestion = new TimeQuestionData
            {
                Answer = defaultAnswer,
                Label = label,

            };

            return new TypedXmlSerializer<TimeQuestionData>().SerializeToXmlString(timeQuestion);
        }

        public static string CreateDateQuestion(XElement questionIterator, Category parent, XDocument xmlDocument)
        {
            var questionId = questionIterator.Name.LocalName;

            var constraint = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                    .Element(Namespaces.DefaultNamespace + "model").Elements(Namespaces.DefaultNamespace + "bind").ToList()
                    .Where(e => e.Attribute("nodeset").Value.Equals(string.Format("/data/{0}/{1}", parent.SystemID, questionId))).Single().Attribute("constraint");

            DateTime min = DateTime.MinValue, max = DateTime.MinValue;

            if (constraint != null)
            {
                var contraints = GetConstraints(constraint.Value, @"\((\. >= (?'min'\d{4}-\d{2}-\d{2}))?( and )?(\. <= (?'max'\d{4}-\d{2}-\d{2}))?\)");

                var minString = contraints["min"];
                var maxString = contraints["max"];

                if (!string.IsNullOrEmpty(minString))
                    min = DateTime.ParseExact(minString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (!string.IsNullOrEmpty(maxString))
                    max = DateTime.ParseExact(maxString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            DateTime? defaultAnswer = null;
            if (!string.IsNullOrEmpty(questionIterator.Value))
                defaultAnswer = DateTime.ParseExact(questionIterator.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var label = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                    .Element(Namespaces.DefaultNamespace + "model").Element(Namespaces.DefaultNamespace + "itext").Element(Namespaces.DefaultNamespace + "translation")
                    .Elements().ToList().Where(e => e.Attribute("id").Value.Equals(string.Format("/data/{0}/{1}:label", parent.SystemID, questionId))).Single().Element(Namespaces.DefaultNamespace + "value").Value;

            var dateQuestion = new DateQuestionData
            {
                Label = label,
                Answer = defaultAnswer,
                MaxDate = max,
                MinDate = min,
            };


            return new TypedXmlSerializer<DateQuestionData>().SerializeToXmlString(dateQuestion);
        }

        public static string CreateImageQuestion(XElement questionIterator, Category parent, XDocument xmlDocument)
        {
            var questionId = questionIterator.Name.LocalName;

            var label = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                     .Element(Namespaces.DefaultNamespace + "model").Element(Namespaces.DefaultNamespace + "itext").Element(Namespaces.DefaultNamespace + "translation")
                     .Elements().ToList().Where(e => e.Attribute("id").Value.Equals(string.Format("/data/{0}/{1}:label", parent.SystemID, questionId))).Single().Element(Namespaces.DefaultNamespace + "value").Value;

            var question = new ImageQuestionData
            {
                Label = label,
                AnswerBase64 = questionIterator.Value,
            };

            return new TypedXmlSerializer<ImageQuestionData>().SerializeToXmlString(question);
        }

        private static Dictionary<string, string> GetConstraints(string constraint, string expression)
        {
            var result = new Dictionary<string, string>();
            var regex = new Regex(expression);
            var match = regex.Match(constraint);

            foreach (var groupName in regex.GetGroupNames())
                result.Add(groupName, match.Groups[groupName].Value);

            return result;
        }
    }
}
