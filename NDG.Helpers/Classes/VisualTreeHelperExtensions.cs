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
using Microsoft.Phone.Controls;

namespace NDG.Helpers.Classes
{
    public static class VisualTreeHelperExtensions
    {
        public static bool Unfocus(this PhoneApplicationPage page)
        {
            return UnfocusTextBox(page);
        }

        private static bool UnfocusTextBox(DependencyObject element)
        {
            bool result = false;
            var childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                if (child is TextBox)
                {
                    ((TextBox)child).IsEnabled = false;
                    ((TextBox)child).IsEnabled = true;
                    result = true;
                }
                else if (child is PasswordBox)
                {
                    ((PasswordBox)child).IsEnabled = false;
                    ((PasswordBox)child).IsEnabled = true;
                    result = true;
                }
                else
                {
                    bool wasUnfocus = UnfocusTextBox(child);
                    result = !result && wasUnfocus ? wasUnfocus : result;
                }
            }

            return result;
        }

        public static void DisableCurrentPage(this PhoneApplicationFrame rootFrame)
        {
            var page = (PhoneApplicationPage)rootFrame.Content;
            SetIsEnabledAllControls(page, false);
        }

        public static void EnableCurrenPage(this PhoneApplicationFrame rootFrame)
        {
            var page = (PhoneApplicationPage)rootFrame.Content;
            SetIsEnabledAllControls(page, true);
        }

        private static void SetIsEnabledAllControls(DependencyObject element, bool isEnabled)
        {
            var childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                if (child is Control && !(child is PhoneApplicationPage) && !(child is Pivot) && !(child is Panorama)
                    && !(child is Grid))
                {
                    ((Control)child).IsEnabled = isEnabled;
                }
                else
                {
                    SetIsEnabledAllControls(child, isEnabled);
                }
            }
        }
    }
}
