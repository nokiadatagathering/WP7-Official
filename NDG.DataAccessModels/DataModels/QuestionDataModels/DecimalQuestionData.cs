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

namespace NDG.DataAccessModels.DataModels
{
    public class DecimalQuestionData : QuestionData
    {
        private Decimal? _answer;
        public Decimal? Answer
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
                Answer = Decimal.Parse(answer);
            else
                Answer = null;
        }


        public override string GetResult()
        {
            return Answer.HasValue ? Answer.ToString() : string.Empty;
        }

        public Decimal? MinValue { get; set; }
        public Decimal? MaxValue { get; set; }

        private bool IsMaxValueSet()
        {
            return MaxValue != null;
        }

        private bool IsMinValueSet()
        {
            return MinValue != null;
        }

        public bool Required { get; set; }

        public override bool Validate()
        {
            var validationResult = true;

            if (Required)
            {
                if (!Answer.HasValue)
                    return false;

                if (IsMaxValueSet())
                {
                    validationResult &= Answer <= MaxValue;
                }
                if (IsMinValueSet())
                {
                    validationResult &= Answer >= MinValue;
                }
                return validationResult;
            }

            //if (!IsEnabled)
            //    return true;

            //if (!Answer.HasValue)
            //    return false;

            if (Answer.HasValue)
            {
                if (IsMaxValueSet())
                {
                    validationResult &= Answer <= MaxValue;
                }
                if (IsMinValueSet())
                {
                    validationResult &= Answer >= MinValue;
                }
            }

            return validationResult;
        }

        public override string InvalidMessage
        {
            get
            {
                var invalidMessage = new StringBuilder();
                if (!Answer.HasValue && Required)
                {
                    invalidMessage.AppendFormat("Please input a decimal value!");
                    return invalidMessage.ToString();
                }
                if (IsMinValueSet())
                    invalidMessage.AppendFormat("Minimum value: {0} ", MinValue);
                if (IsMaxValueSet())
                    invalidMessage.AppendFormat("Maximum value: {0} ", MaxValue);
                return invalidMessage.ToString();
            }
        }
    }
}
