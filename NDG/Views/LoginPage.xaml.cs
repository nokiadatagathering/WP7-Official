// ----------------------------------------------------------------------
// <copyright file="LoginPage.xaml.cs" company="QArea">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace NDG.Views
{
    using Microsoft.Phone.Controls;
    using System.Linq;
    using System;

    /// <summary>
    /// Class for login page.
    /// </summary>
    public partial class LoginPage : PhoneApplicationPage
    {
        /// <summary>
        /// Initializes a new instance of the LoginPage class.
        /// </summary>
        public LoginPage()
        {
            InitializeComponent();
            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (this.NavigationService != null)
            {
                var backCount = this.NavigationService.BackStack.Count();
                while (backCount > 0)
                {
                    this.NavigationService.RemoveBackEntry();
                    backCount--;
                }
            }
        }

        /// <summary>
        /// Raises by navigation from login page.
        /// </summary>
        /// <param name="e">Navigation event arguments.</param>
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (!e.Uri.OriginalString.Contains("AboutPage.xaml") && !e.Uri.OriginalString.Contains("SettingsPage.xaml"))
            {
                this.NavigationService.RemoveBackEntry();
            }
        }


    }
}