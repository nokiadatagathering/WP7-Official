// ----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="QArea">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace NDG
{
    using System;
    using System.Windows;
    using System.Windows.Navigation;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;
    using NDG.Helpers.Classes;
    using System.Collections;
    using System.Windows.Media;
    using System.IO.IsolatedStorage;

    /// <summary>
    /// Class of application.
    /// </summary>
    public partial class App : Application
    {

        /// <summary>
        /// is application started now.
        /// </summary>
        private bool isApplicationStarted = true;

        /// <summary>
        /// Avoid double-initialization
        /// </summary>
        private bool phoneApplicationInitialized = false;

        /// <summary>
        /// Initializes a new instance of the App class.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            this.UnhandledException += this.Application_UnhandledException;

            // Standard Silverlight initialization
            this.InitializeComponent();

            // Phone-specific initialization
            this.InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {

#if DEBUG
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

#endif

                // Show the areas of the app that are being redrawn in each frame.
                // Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                // Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }

        /// <summary>
        /// Gets root frame of the Phone Application.
        /// </summary>        
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Code to execute when the application is launching (eg, from Start)
        /// This code will not execute when the application is reactivated
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event parameters</param>
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        
         

        }

        /// <summary>
        /// Code to execute when the application is activated (brought to foreground)
        /// This code will not execute when the application is first launched
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event parameters</param>
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        /// <summary>
        /// Code to execute when the application is deactivated (sent to background)
        /// This code will not execute when the application is closing
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event parameters</param>
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        /// <summary>
        /// Code to execute when the application is closing (eg, user hit Back)
        /// This code will not execute when the application is deactivated
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event parameters</param>
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        /// <summary>
        /// Code to execute if a navigation fails
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event parameters</param>
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        /// <summary>
        /// Code to execute on Unhandled Exceptions
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event parameters</param>
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        /// <summary>
        /// Do not add any additional code to this method
        /// </summary>
        private void InitializePhoneApplication()
        {
            if (this.phoneApplicationInitialized)
            {
                return;
            }
            InitialCopy();
            OverridePhoneStyles();

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            this.RootFrame = new TransitionFrame();
            this.RootFrame.Navigated += this.CompleteInitializePhoneApplication;
            // Handle navigation failures
            this.RootFrame.NavigationFailed += this.RootFrame_NavigationFailed;
            this.RootFrame.Background = this.Resources["PhoneChromeBrush"] as SolidColorBrush;
            // Ensure we don't initialize again
            this.phoneApplicationInitialized = true;
        }

        private static void InitialCopy()
        {
            CopyToIsolatedStorageHelper.CopyToIsolatedStorage("Initial/NdgDB.sdf", "NdgDB.sdf");
            CopyToIsolatedStorageHelper.CopyToIsolatedStorage("Initial/English_en-GB.xml", "English_en-GB.xml");
        }

        /// <summary>
        /// Method override system brushes by brushes contained in Styles/OverridedPhoneStyles.xaml resourse dictionary
        /// </summary>
        private static void OverridePhoneStyles()
        {
            string source = String.Format("/Styles/OverridePhoneStyles.xaml");
            var themeStyles = new ResourceDictionary { Source = new Uri(source, UriKind.Relative) };

            ResourceDictionary appResources = App.Current.Resources;
            foreach (DictionaryEntry newStyle in themeStyles)
            {
                if (newStyle.Value is SolidColorBrush)
                {
                    SolidColorBrush newBrush = (SolidColorBrush)newStyle.Value;
                    SolidColorBrush existingBrush = (SolidColorBrush)appResources[newStyle.Key];
                    existingBrush.Color = newBrush.Color;
                }
            }
        }

        /// <summary>
        /// Do not add any additional code to this method
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event parameters</param>
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            if (this.isApplicationStarted)
            {
                var locator = this.Resources["Locator"] as NDG.ViewModels.Locator;
                if (locator.Login.IsAuthorized)
                {
                    this.RootFrame.Navigate(new Uri("/Views/Home/HomePage.xaml", UriKind.Relative));
                }

                this.isApplicationStarted = false;
            }

            // Set the root visual to allow the application to render
            if (this.RootVisual != this.RootFrame)
            {
                this.RootVisual = this.RootFrame;
            }

            // Remove this handler since it is no longer needed
            this.RootFrame.Navigated -= this.CompleteInitializePhoneApplication;
        }

        #endregion
    }
}