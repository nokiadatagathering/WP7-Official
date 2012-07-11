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
using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace NDG.Helpers.Triggers
{
    public class InvokeOnNewPageTrigger : TriggerAction<UserControl>
    {
        private bool canInvokeCommand = true;

        private PhoneApplicationPage currentPage;

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeOnNewPageTrigger), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(InvokeOnNewPageTrigger), new PropertyMetadata(null));

        public ICommand Command
        {
            get
            {
                return (ICommand)this.GetValue(CommandProperty);
            }

            set
            {
                this.SetValue(CommandProperty, value);
            }
        }

        public object CommandParameter
        {
            get
            {
                return this.GetValue(CommandParameterProperty);
            }

            set 
            {
                this.SetValue(CommandParameterProperty, value);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += this.OnLoaded;
        }

        protected override void Invoke(object parameter)
        {
            if (this.canInvokeCommand && this.Command != null)
            {
                this.Command.Execute(this.CommandParameter);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var parent = this.AssociatedObject as DependencyObject;
            bool isPageFounded = false;
            while (!isPageFounded)
            {
                if (parent is PhoneApplicationPage)
                {
                    this.currentPage = (PhoneApplicationPage)parent;
                    isPageFounded = true;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }

            this.currentPage.NavigationService.Navigating += this.OnNavigating;
        }

        private void OnNavigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                this.canInvokeCommand = true;
            }
            else
            {
                this.canInvokeCommand = false;
            }
        }
    }
}
