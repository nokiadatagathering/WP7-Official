using System;
using System.Net;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace NDG.DataAccessModels.DataModels
{
    public class ExclusiveChocieQuestionData : QuestionData
    {
        private List<ChoiceTextValuePair> _options;
        public List<ChoiceTextValuePair> Options
        {
            get
            {
                return _options;
            }
            set
            {
                _options = value;
                NotifyPropertyChanged("Options");
            }
        }

        private ChoiceTextValuePair _answer;
        public ChoiceTextValuePair Answer
        {
            get { return _answer; }
            set
            {
                _answer = value;
                NotifyPropertyChanged("Answer");
            }
        }

        public override void SetResult(string answer)
        {
            if (!string.IsNullOrEmpty(answer))
            {
                var values = Options.Select(o => o.Value);

                if (values.Contains(answer))
                {
                    Answer = Options.First(o => o.Value.Equals(answer));

                }
            }
            else
            {
                Answer = null;
            }
        }

        public override string GetResult()
        {
            if (Answer != null)
                return Answer.Value;
            return string.Empty;
        }
        public ExclusiveChocieQuestionData()
        {
            Options = new List<ChoiceTextValuePair>();
        }

        public bool Required { get; set; }

        public override bool Validate()
        {
            if(Required)
                return !IsEnabled || Answer != null && Required;
            //return !IsEnabled || Answer != null;
            return true;
        }

        public override string InvalidMessage
        {
            get
            {
                return "Please select an option!";
            }
        }
    }
}
