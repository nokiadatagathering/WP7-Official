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
using System.Globalization;

namespace NDG.DataAccessModels.DataModels
{
    public class TimeQuestionData : QuestionData
    {
        private DateTime? _answer;
        public DateTime? Answer
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

        public override void SetResult(string answer)
        {
            if (!string.IsNullOrEmpty(answer))
            {
                string[] tokens = answer.Split(':');

                int hour = Convert.ToInt32(tokens[0]);
                int minute = Convert.ToInt32(tokens[1]);
                var str = string.Format("{0}:{1}:00Z", hour, minute);
                Answer = DateTime.Parse(str, CultureInfo.InvariantCulture);
            }
            else
                Answer = null;
        }

        //TODO: Set valid format for time string
        public override string GetResult()
        {
            return Answer.HasValue ? Answer.Value.ToUniversalTime().ToString("hh:mm:ss:00.000Z") : string.Empty;
        }

        public override bool Validate()
        {
            return !IsEnabled || Answer.HasValue;
        }

        public override string InvalidMessage
        {
            get
            {
                return "Please select time!";
            }
        }
    }
}
