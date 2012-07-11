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
using System.Text;
using System.Globalization;

namespace NDG.DataAccessModels.DataModels
{
    public class DateQuestionData : QuestionData
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
            if (!answer.Equals(string.Empty))
                Answer = DateTime.Parse(answer, new CultureInfo("en-US"), DateTimeStyles.None);
            else
                Answer = null;
        }

        public override string GetResult()
        {

            return Answer.HasValue ? Answer.Value.ToUniversalTime().ToString("yyyy-MM-dd") : string.Empty;
        }

        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }

        private bool IsMaxDateSet()
        {
            return MaxDate != DateTime.MinValue;
        }

        private bool IsMinDateSet()
        {
            return MinDate != DateTime.MinValue;
        }

        public override bool Validate()
        {
            var validationResult = true;
            if (!IsEnabled)
                return validationResult;

            if (Answer.HasValue)
            {
                if (IsMaxDateSet())
                {
                    validationResult &= Answer.Value <= MaxDate;
                }
                if (IsMinDateSet())
                {
                    validationResult &= Answer.Value >= MinDate;
                }
                return validationResult;
            }
            return false;
        }

        public override string InvalidMessage
        {
            get
            {
                var invalidMessage = new StringBuilder();
                if (IsMaxDateSet())
                    invalidMessage.AppendFormat("Maximum date: {0} ", MaxDate.ToShortDateString());
                if (IsMaxDateSet())
                    invalidMessage.AppendFormat("Minimum date: {0} ", MinDate.ToShortDateString());
                return invalidMessage.ToString();
            }
        }
    }
}
