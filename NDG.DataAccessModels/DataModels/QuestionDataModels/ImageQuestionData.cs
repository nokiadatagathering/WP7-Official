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
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using NDG.Helpers.Classes;
using NDG.DataAccessModels.Repositories;

namespace NDG.DataAccessModels.DataModels
{
    public class ImageQuestionData : QuestionData
    {
        public static PhotoResolution CurrentResolution { get; set; }

        public ImageQuestionData()
        {
            if (CurrentResolution == null)
            {
                using (var settingsRepository = new SettingsRepository())
                {
                    CurrentResolution = settingsRepository.GetCurrentSettings().PhotoResolution;
                }
            }
        }

        private string _answerBase64;
        public string AnswerBase64
        {
            get { return _answerBase64; }
            set
            {
               
                    _answerBase64 = value;
                    if (!string.IsNullOrEmpty(value))
                    {
                        Answer = new ImageStringBase64Converter().GetImageFromStringBase64(value);
                    }
                    else
                        Answer = null;
            }
        }
        public override void SetResult(string answer)
        {
            AnswerBase64 = answer;
        }

        private BitmapImage _answer;
        [XmlIgnore]
        public BitmapImage Answer
        {
            get
            {
                return _answer;
            }
            set
            {
                _answer = value;
                _answerBase64 = new ImageStringBase64Converter().GetStringBase64FromImage(value, CurrentResolution.Width, CurrentResolution.Height);
                NotifyPropertyChanged("Answer");
            }
        }

        public override string GetResult()
        {
            if (Answer != null)
            {
                return new ImageStringBase64Converter().GetStringBase64FromImage(Answer, CurrentResolution.Width, CurrentResolution.Height);
            }
            return string.Empty;
        }

        public override bool Validate()
        {
            return !IsEnabled || Answer != null;

        }

        public override string InvalidMessage
        {
            get
            {
                return "Please select an image!";
            }
        }
    }
  
}
