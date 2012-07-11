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
using System.ComponentModel;
using System.Xml.Serialization;

namespace NDG.DataAccessModels.DataModels
{
    public abstract class QuestionData : INotifyPropertyChanged, IValidation
    {
        public QuestionData()
        {
            IsEnabled = true;
        }

        private string _label;
        [XmlAttribute]
        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                NotifyPropertyChanged("Label");
            }
        }

        private bool _isEnabled;
        [XmlAttribute]
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                if (!value)
                    SetResult(string.Empty);
                NotifyPropertyChanged("IsEnabled");
            }
        }

        public abstract string GetResult();
        public abstract void SetResult(string answer);

     
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public abstract bool Validate();

        public abstract string InvalidMessage { get;}
    }
}
