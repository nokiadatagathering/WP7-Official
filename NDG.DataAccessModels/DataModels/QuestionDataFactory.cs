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

namespace NDG.DataAccessModels.DataModels
{
    public class QuestionDataFactory
    {
        public static QuestionData GetQuestionDataForQuestion(Question question)
        {
            switch (question.Type)
            {
                case QuestionType.DateQuestion:
                    return new TypedXmlSerializer<DateQuestionData>().DeserializeFromXmlString(question.XML);

                case QuestionType.DecimalQuestion:
                    return new TypedXmlSerializer<DecimalQuestionData>().DeserializeFromXmlString(question.XML);

                case QuestionType.DescriptiveQuestion:
                    return new TypedXmlSerializer<DescriptiveQuestionData>().DeserializeFromXmlString(question.XML);

                case QuestionType.ExclusiveChoiceQuestion:
                    var exlusiveData = new TypedXmlSerializer<ExclusiveChocieQuestionData>().DeserializeFromXmlString(question.XML);
                    exlusiveData.SetResult(exlusiveData.GetResult());
                    return exlusiveData;

                case QuestionType.ImageQuestion:
                    return new TypedXmlSerializer<ImageQuestionData>().DeserializeFromXmlString(question.XML);

                case QuestionType.IntegerQuestion:
                    return new TypedXmlSerializer<IntegerQuestionData>().DeserializeFromXmlString(question.XML);

                case QuestionType.MultipleChocieQuestion:
                    var multipleData = new TypedXmlSerializer<MultipleChoiceQuestionData>().DeserializeFromXmlString(question.XML);
                    multipleData.SetResult(multipleData.GetResult());
                    return multipleData;

                case QuestionType.TimeQuestion:
                    return new TypedXmlSerializer<TimeQuestionData>().DeserializeFromXmlString(question.XML);

                default:
                    throw new ArgumentException("XML data for question is invalid!");


            }
        }
    }
}
