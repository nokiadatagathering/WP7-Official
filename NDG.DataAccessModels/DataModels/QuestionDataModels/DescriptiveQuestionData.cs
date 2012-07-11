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

namespace NDG.DataAccessModels.DataModels
{
    public class DescriptiveQuestionData : QuestionData
    {
        private String _answer;
        public String Answer
        {
            get
            {
                return _answer;
            }
            set
            {
                _answer = value;
                NotifyPropertyChanged("Answer");
            }
        }

        public override string GetResult()
        {
            return Answer;
        }

        public override void SetResult(string answer)
        {
            Answer = answer;
        }

        public int MaxLength { get; set; }

        public override bool Validate()
        {
            return !IsEnabled || (!string.IsNullOrEmpty(Answer) && Answer.Length <= MaxLength);

        }

        public override string InvalidMessage
        {
            get
            {
                if(string.IsNullOrEmpty(Answer))
                    return string.Format("Please fill an answer!");
                return string.Format("Maximum length is {0}",MaxLength);
            }
        }
    }
}
