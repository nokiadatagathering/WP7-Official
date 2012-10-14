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

namespace NDG.DataAccessModels.DataModels
{
    public class GeopointQuestionData : QuestionData
    {
        private String _answer;
        public String Answer
        {
            get
            {
                if(new SettingsRepository().GetCurrentSettings().IsGpsEnabled)
                    return _answer;
                return "";
            }
            set
            {
                _answer = "";
                if (new SettingsRepository().GetCurrentSettings().IsGpsEnabled)
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

        public override bool Validate()
        {
            return !IsEnabled || (!string.IsNullOrEmpty(Answer));
        }

        public override string InvalidMessage
        {
            get
            {
                return string.Format("Please record a location!");
            }
        }
    }
}
