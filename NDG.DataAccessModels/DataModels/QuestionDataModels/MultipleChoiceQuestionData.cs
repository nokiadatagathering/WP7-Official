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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NDG.DataAccessModels.DataModels
{
    public class MultipleChoiceQuestionData : QuestionData
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

        private ObservableCollection<ChoiceTextValuePair> _answers;
        public ObservableCollection<ChoiceTextValuePair> Answers
        {
            get { return _answers; }
            set
            {
                if (_answers == value) return;
                _answers = new ObservableCollection<ChoiceTextValuePair>(value);
                NotifyPropertyChanged("Answers");
            }
        }

        public override string GetResult()
        {
            return String.Join(" ", Answers.Select(a => a.Value).ToArray());
        }

        public override void SetResult(string answer)
        {
            Answers = new ObservableCollection<ChoiceTextValuePair>();
            var answerValues = answer.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var values = Options.Select(o=>o.Value);
            foreach (var value in answerValues)
                if (values.Contains(value))
                    Answers.Add(Options.First(o => o.Value.Equals(value)));
        }

        public MultipleChoiceQuestionData()
        {
            Options = new List<ChoiceTextValuePair>();
            Answers = new ObservableCollection<ChoiceTextValuePair>();
        }

        public bool Required { get; set; }

        public override bool Validate()
        {
            if(Required)
                return !IsEnabled || Answers.Count != 0 && Required;
            //return !IsEnabled || Answers.Count != 0;
            return true;
        }

        public override string InvalidMessage
        {
            get
            {
                return "Make your choice please!";
            }
        }
    }
}
