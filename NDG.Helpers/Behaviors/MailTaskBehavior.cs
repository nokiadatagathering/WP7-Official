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
using System.Windows.Interactivity;
using Microsoft.Phone.Tasks;

namespace NDG.Helpers.Behaviors
{
    public class MailTaskBehavior : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Tap += this.OnTap;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Tap -= this.OnTap;
            base.OnDetaching();
        }

        private void OnTap(object sender, EventArgs e)
        {
            var textBlock = (TextBlock)sender;
            EmailComposeTask emailTask = new EmailComposeTask();
            emailTask.To = textBlock.Text;
            emailTask.Show();
        }
    }
}
