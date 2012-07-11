using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Media;

namespace NDG.Helpers.Behaviors
{
    public class NavigationBackBehavior : Behavior<UserControl>
    {
        private NavigationMode navigationMode;

        private PhoneApplicationPage currentPage;

        public static readonly DependencyProperty BackNavigationCommandProperty = DependencyProperty.Register("BackNavigationCommand", typeof(ICommand), typeof(NavigationBackBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty BackNavigationCommandParameterProperty = DependencyProperty.Register("BackNavigationCommandParameter", typeof(object), typeof(NavigationBackBehavior), new PropertyMetadata(null));

        public ICommand BackNavigationCommand
        {
            get { return (ICommand)this.GetValue(BackNavigationCommandProperty); }

            set { this.SetValue(BackNavigationCommandProperty, value); }
        }

        public object BackNavigationCommandParameter
        {
            get { return this.GetValue(BackNavigationCommandParameterProperty); }

            set { this.SetValue(BackNavigationCommandParameterProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += OnLoaded;
            this.AssociatedObject.Unloaded += OnUnloaded;
        }

        void OnUnloaded(object sender, RoutedEventArgs e)
        {
           
            this.AssociatedObject.Loaded -= this.OnLoaded;
            if (this.navigationMode == NavigationMode.Back && this.BackNavigationCommand != null)
            {
                this.BackNavigationCommand.Execute(this.BackNavigationCommandParameter);
                this.currentPage.NavigationService.Navigating -= OnNavigating;
                this.AssociatedObject.Unloaded -= this.OnUnloaded;
            }
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            bool isPageFounded = false;
            var parent = this.AssociatedObject as DependencyObject;
            while (!isPageFounded)
            {
                if (parent is PhoneApplicationPage)
                {
                    this.currentPage = parent as PhoneApplicationPage;
                    isPageFounded = true;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }

            this.currentPage.NavigationService.Navigating += OnNavigating;
        }

        void OnNavigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {

            this.navigationMode = e.NavigationMode;
          
            
        }
    }
}
