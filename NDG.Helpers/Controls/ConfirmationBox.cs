using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Controls;

namespace NDG.Helpers.Controls
{
    /// <summary>
    /// Message box with yes/no buttons.
    /// </summary>
    public class ConfirmationBox : UserPrompt
    {
        Button yesButton = new Button();
        Button noButton = new Button();

        public event EventHandler<ConfirmationResulEventArgs> DialogCompleted;

        public ConfirmationBox()
        {
            this.DefaultStyleKey = typeof(ConfirmationBox);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.yesButton = this.GetTemplateChild("yesButton") as Button;
            this.noButton = this.GetTemplateChild("noButton") as Button;
            this.yesButton.Click += OnYesButtonClick;
            this.noButton.Click += OnNoButtonClick;
            bool isPageFounded = false;
            var parent = VisualTreeHelper.GetParent(this);
            while (!isPageFounded)
            {
                if (parent is PhoneApplicationPage)
                {
                    var page = (PhoneApplicationPage)parent;
                    page.BackKeyPress += this.OnBackKeyPress;
                    isPageFounded = true;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }
        }

        private void OnYesButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.DialogCompleted != null)
            {
                this.DialogCompleted(this, new ConfirmationResulEventArgs() { DialogResult = PopUpResult.Ok });
                this.Hide();
            }
        }

        private void OnNoButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.DialogCompleted != null)
            {
                this.DialogCompleted(this, new ConfirmationResulEventArgs() { DialogResult = PopUpResult.NoResponse });
                this.Hide();
            }
        }

        private void OnBackKeyPress(object sender, EventArgs e)
        {
            if (this.DialogCompleted != null)
            {
                this.DialogCompleted(this, new ConfirmationResulEventArgs() { DialogResult = PopUpResult.UserDismissed });
            }
        }
    }

    public class ConfirmationResulEventArgs : EventArgs
    {
        public PopUpResult DialogResult { get; set; }
    }
}
