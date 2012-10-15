using System;
using System.Windows;
using System.Windows.Interactivity;
using NDG.DataAccessModels.DataModels;
using NDG.DataAccessModels;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;

namespace NDG.Convertors
{
    public class ValidateAnswerBehavior : Behavior<Control>
    {
        public static readonly DependencyProperty InvalidMessageProperty = DependencyProperty.Register("InvalidMessage", typeof(string), typeof(Control), new PropertyMetadata(null));

        public static readonly DependencyProperty InvalidContentVisibilityProperty = DependencyProperty.Register("InvalidContentVisibility", typeof(Visibility), typeof(Control), new PropertyMetadata(null));

        public string InvalidMessage
        {
            get { return (string)this.AssociatedObject.GetValue(InvalidMessageProperty); }

            set { this.AssociatedObject.SetValue(InvalidMessageProperty, value); }
        }

        public Visibility InvalidContentVisibility
        {
            get { return (Visibility)this.AssociatedObject.GetValue(InvalidContentVisibilityProperty); }
            set { this.AssociatedObject.SetValue(InvalidContentVisibilityProperty, value); }
        }

        private Question currentQuestion;

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += this.OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var question = this.AssociatedObject.DataContext as Question;
            if (question != null)
            {
                this.currentQuestion = question;
                if (this.currentQuestion.Data != null)
                {
                    bool isAnswerValid = this.currentQuestion.Data.Validate();
                    if (!isAnswerValid)
                    {
                        this.InvalidMessage = this.currentQuestion.Data.InvalidMessage;
                        this.InvalidContentVisibility = Visibility.Visible;
                    }
                    else
                    {
                        this.InvalidContentVisibility = Visibility.Collapsed;
                        this.InvalidMessage = null;
                    }
                    this.currentQuestion.Data.PropertyChanged += this.OnPropertyChanged;
                }
            }
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= this.OnLoaded;
            base.OnDetaching();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Answer")
            {
                this.currentQuestion.UpdateDependentQuestionsData();
                bool isAnswerValid = this.currentQuestion.Data.Validate();
                if (!isAnswerValid)
                {
                    this.InvalidMessage = this.currentQuestion.Data.InvalidMessage;
                    this.InvalidContentVisibility = Visibility.Visible;
                }
                else
                {
                    this.InvalidContentVisibility = Visibility.Collapsed;
                    this.InvalidMessage = null;
                }
            }
        }

        private void OnLostFocus(object sender, EventArgs e)
        {

            bool isAnswerValid = this.currentQuestion.Data.Validate();
            if (!isAnswerValid)
            {
                this.InvalidMessage = this.currentQuestion.Data.InvalidMessage;
                this.InvalidContentVisibility = Visibility.Visible;
            }
        }

        private void OnGotFocus(object sender, EventArgs e)
        {
            if (this.InvalidContentVisibility == Visibility.Visible)
            {
                this.InvalidContentVisibility = Visibility.Collapsed;
                this.InvalidMessage = null;
            }
        }
    }
}
