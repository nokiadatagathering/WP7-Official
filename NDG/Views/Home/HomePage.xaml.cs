// ----------------------------------------------------------------------
// <copyright file="HomePage.xaml.cs" company="QArea">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace NDG.Views.Home
{
    using Microsoft.Phone.Controls;
    using System.Windows;
    using System.Linq;

    /// <summary>
    /// Class for home page.
    /// </summary>
    public partial class HomePage : PhoneApplicationPage
    {
        /// <summary>
        /// Initializes a new instance of the HomePage class.
        /// </summary>
        public HomePage()
        {
            InitializeComponent();
            this.Loaded += this.OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null && this.NavigationService.BackStack.Count() > 0)
            {
                while (this.NavigationService.BackStack.Count() > 0)
                {
                    this.NavigationService.RemoveBackEntry();
                }
            }
        }
    }
}