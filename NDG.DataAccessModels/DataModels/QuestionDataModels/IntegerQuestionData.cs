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
    public class IntegerQuestionData : QuestionData
    {
        private int? _answer;
        public int? Answer
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
                Answer = int.Parse(answer);
            else
                Answer = null;
        }

        public override string GetResult()
        {
            return Answer.ToString();
        }

        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }

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

            if(Required)
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
                    invalidMessage.AppendFormat("Please input an integer value!");
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
