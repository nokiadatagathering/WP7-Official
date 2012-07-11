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
using NDG.DataAccessModels;
using System.IO;
using System.Xml.Linq;
using NDG.ModelsParser;
using System.Collections.Generic;

namespace NDG.XFormsParser
{
    public class SurveyContentParser
    {
        private Dictionary<Question, string> _relevantList = new Dictionary<Question, string>();

        public Survey GetSurveyFromXFormsStream(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                var documentXML = XDocument.Parse(streamReader.ReadToEnd());
                var resultSurvey = new Survey();

                ParseBasicSurveyInformationFromXDocument(documentXML, resultSurvey);
                ParseSurveyCategoriesFromXmlString(documentXML, resultSurvey);

                return resultSurvey;
            }

        }

        private void ParseBasicSurveyInformationFromXDocument(XDocument documentXML, Survey survey)
        {
            var surveyName = documentXML.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                .Element(Namespaces.XHtmlNamespace + "title").Value;

            var surveySystemID = documentXML.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                .Element(Namespaces.DefaultNamespace + "model").Element(Namespaces.DefaultNamespace + "instance").Element(Namespaces.DefaultNamespace + "data").Attribute("id").Value;

            survey.DateReceived = DateTime.Now;
            survey.SystemID = surveySystemID;
            survey.Name = surveyName;
        }

        private void ParseSurveyCategoriesFromXmlString(XDocument xmlDocument, Survey survey)
        {
            var categories = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                .Element(Namespaces.DefaultNamespace + "model").Element(Namespaces.DefaultNamespace + "instance")
                .Element(Namespaces.DefaultNamespace + "data").Elements();


            foreach (var categoryIterator in categories)
            {
                var category = new Category
                {
                    SystemID = categoryIterator.Name.LocalName,
                };

                category.Name = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                    .Element(Namespaces.DefaultNamespace + "model").Element(Namespaces.DefaultNamespace + "itext").Element(Namespaces.DefaultNamespace + "translation")
                    .Elements().ToList().Where(e => e.Attribute("id").Value.Equals(string.Format("/data/{0}:label", category.SystemID))).Single().Element(Namespaces.DefaultNamespace + "value").Value;

                var questions = categoryIterator.Elements();

                foreach (var questionIterator in questions)
                {
                    ParseQuestion(questionIterator, category, xmlDocument, survey);
                }

                survey.Category.Add(category);
            }
            ParseDependentQuestions(survey);
        }

        private void ParseDependentQuestions(Survey survey)
        {
            foreach (var relevant in _relevantList)
            {
                var dependentQuestion = new DependentQuestions();
                dependentQuestion.Question_DependentQuestions.Add(relevant.Key);
                var constraintSplitResult = relevant.Value.Split('=');
                var systemID = constraintSplitResult[0].Split('/').Last();
                var requiredAnswer = constraintSplitResult[1].Replace("'",string.Empty);
                dependentQuestion.RequiredAnswer = requiredAnswer;
                var parentQuestion = survey.Category.SelectMany(c => c.Question).First(q => q.SystemID.Equals(systemID));
                dependentQuestion.Question = parentQuestion;
            }
        }

       

        private void ParseQuestion(XElement questionIterator, Category category, XDocument xmlDocument, Survey survey)
        {
            Question question = new Question
            {
                SystemID = questionIterator.Name.LocalName,
            };

            var quesionType = xmlDocument.Element(Namespaces.XHtmlNamespace + "html").Element(Namespaces.XHtmlNamespace + "head")
                    .Element(Namespaces.DefaultNamespace + "model").Elements(Namespaces.DefaultNamespace + "bind").ToList()
                    .Where(e => e.Attribute("nodeset").Value.Equals(string.Format("/data/{0}/{1}", category.SystemID, question.SystemID))).Single().Attribute("type").Value;

            switch (quesionType)
            {
                case "string":
                    question.Type = QuestionType.DescriptiveQuestion;
                    question.XML = QuestionDataFactory.CreateDescriptiveQuestion(questionIterator, category, xmlDocument);
                    break;

                case "int":
                    question.Type = QuestionType.IntegerQuestion;
                    question.XML = QuestionDataFactory.CreateIntegerQuestion(questionIterator, category, xmlDocument);
                    break;

                case "decimal":
                    question.Type = QuestionType.DecimalQuestion;
                    question.XML = QuestionDataFactory.CreateDecimalQuestion(questionIterator, category, xmlDocument);
                    break;

                case "select":
                    question.Type = QuestionType.MultipleChocieQuestion;
                    question.XML = QuestionDataFactory.CreateMultipleChoiceQuestion(questionIterator, category, xmlDocument);
                    break;

                case "select1":
                    question.Type = QuestionType.ExclusiveChoiceQuestion;
                    question.XML = QuestionDataFactory.CreateExclusiveChoiceQuestion(questionIterator, category, xmlDocument);
                    break;

                case "time":
                    question.Type = QuestionType.TimeQuestion;
                    question.XML = QuestionDataFactory.CreateTimeQuestion(questionIterator, category, xmlDocument);
                    break;

                case "date":
                    question.Type = QuestionType.DateQuestion;
                    question.XML = QuestionDataFactory.CreateDateQuestion(questionIterator, category, xmlDocument);
                    break;

                case "binary":
                    question.Type = QuestionType.ImageQuestion;
                    question.XML = QuestionDataFactory.CreateImageQuestion(questionIterator, category, xmlDocument);
                    break;
            }

            if (!string.IsNullOrEmpty(question.XML))
            {
                category.Question.Add(question);
                var relevantConstraint = QuestionDataFactory.GetRelevantString(questionIterator, category, xmlDocument);
                if (relevantConstraint != null)
                    _relevantList.Add(question, relevantConstraint);
            }
        }
    }
}
