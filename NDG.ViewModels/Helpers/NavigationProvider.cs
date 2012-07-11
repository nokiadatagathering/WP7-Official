// ----------------------------------------------------------------------
// <copyright file="NavigationProvider.cs" company="QArea">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace NDG.ViewModels.Helpers
{
    using System;
    using System.Windows;
    using Microsoft.Phone.Controls;
    using NDG.Helpers.Classes;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Threading;

    /// <summary>
    /// Helper class for navigation in application.
    /// </summary>
    public class NavigationProvider
    {
        private static Dictionary<string, object> navigationParameters = new Dictionary<string, object>();

        /// <summary>
        /// Current application root visual.
        /// </summary>
        private static PhoneApplicationFrame rootFame;

        private bool isNavigatingNow = false;

        private string navigationPageSource = null;

        private Uri navigateSource;

#if UNIT_TEST

        public static string CurrentPageSource { get; set; }

#endif

        /// <summary>
        /// Dictionary that contains navigation parameters like objects.
        /// </summary>
        public Dictionary<string, object> NavigationParameters
        {
            get { return navigationParameters; }
            set { navigationParameters = value; }
        }

        public Uri CurrentSource
        {
            get
            {
                this.CheckRootFrame();
                return rootFame.CurrentSource;
            }
        }

        public PhoneApplicationFrame RootFrame
        {
            get 
            {
                if (rootFame == null)
                {
                    CheckRootFrame();
                }

                return rootFame;
            }
        }

        /// <summary>
        /// Can now go back or not.
        /// </summary>
        /// <returns>Boolean value.</returns>
        public bool CanGoBack()
        {
            this.CheckRootFrame();
            return rootFame.CanGoBack;
        }

        public Dictionary<string, string> GetNavigationParameters()
        {
            this.CheckRootFrame();
            Dictionary<string, string> result = new Dictionary<string, string>();
#if !UNIT_TEST
            string currentSource = Regex.Match(rootFame.CurrentSource.OriginalString, @"\?.+").ToString();
#else
            string currentSource = Regex.Match(CurrentPageSource, @"\?.+").ToString();
#endif
            var entries = Regex.Split(currentSource, @"\?|\&", RegexOptions.Singleline).Where(item => !string.IsNullOrEmpty(item));
            if (entries != null && entries.Count() > 0)
            {
                foreach (var entrie in entries)
                {
                    result.Add(entrie.Substring(0, entrie.IndexOf("=")), entrie.Substring(entrie.IndexOf("=") + 1));
                }
            }

            return result;
        }

        /// <summary>
        /// Navigates to specified uri.
        /// </summary>
        /// <param name="navigationSource">Source of navigation.</param>
        public void Navigate(Uri navigationSource)
        {
            this.CheckRootFrame();
            if (!isNavigatingNow)
            {
                var page = rootFame.Content as PhoneApplicationPage;
                bool wasUnfocus = false;
                if (page != null)
                {
                    wasUnfocus = page.Unfocus();
                }

                if (wasUnfocus)
                {
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                    timer.Tick += this.OnTimerTick;
                    timer.Start();
                    this.navigateSource = navigationSource;
                }
                else
                {
                    rootFame.Navigate(navigationSource);
                }
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (this.navigateSource != null)
            {
                rootFame.Navigate(navigateSource);
            }

            var timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= this.OnTimerTick;
        }

        public void NavigateAndRemoveBackEntries(Uri navigationSource)
        {
            this.Navigate(navigationSource);
            this.navigationPageSource = navigationSource.OriginalString;
            navigationPageSource = navigationPageSource.Contains('?') ? navigationPageSource.Substring(0, navigationPageSource.IndexOf('?'))
                    : navigationPageSource;
        }

        /// <summary>
        /// Goes back.
        /// </summary>
        public void GoBack()
        {
            this.CheckRootFrame();
            if (!isNavigatingNow && rootFame.CanGoBack)
            {
                rootFame.GoBack();
            }
        }

        /// <summary>
        /// Checks is rootFrame not null.
        /// </summary>
        private void CheckRootFrame()
        {
            if (rootFame == null)
            {
                rootFame = Application.Current.RootVisual as PhoneApplicationFrame;
                rootFame.Navigating += this.OnFrameNavigating;
                rootFame.Navigated += this.OnFrameNavigated;
            }
        }

        private void OnFrameNavigating(object sender, EventArgs e)
        {
            this.isNavigatingNow = true;
        }

        private void OnFrameNavigated(object sender, EventArgs e)
        {
            this.isNavigatingNow = false;

            if (!string.IsNullOrEmpty(this.navigationPageSource))
            {
                int index = 0;
                var navigatedItem = rootFame.BackStack.SingleOrDefault(item => item.Source.OriginalString.Contains(navigationPageSource));
                if (navigatedItem != null)
                {
                    index = rootFame.BackStack.ToList().IndexOf(navigatedItem);
                }
                else
                {
                    var homePageSource = rootFame.BackStack.SingleOrDefault(item => item.Source.OriginalString.Contains("/Views/Home/HomePage.xaml"));
                    index = rootFame.BackStack.ToList().IndexOf(homePageSource) - 1;
                }

                while (index >= 0)
                {
                    rootFame.RemoveBackEntry();
                    index--;
                }

                this.navigationPageSource = null;
            }
        }
    }
}
