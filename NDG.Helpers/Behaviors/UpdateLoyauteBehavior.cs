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

namespace NDG.Helpers.Behaviors
{
    public class UpdateLoyauteBehavior : Behavior<ToggleSwitch>
    {
        public static readonly DependencyProperty IsNeedUpdateLoyauteProperty =
            DependencyProperty.Register(
            "IsNeedUpdateLoyaute",
            typeof(bool),
            typeof(UpdateLoyauteBehavior),
            new PropertyMetadata(OnIsNeedUpdateLoyauteChanged));

        public bool IsNeedUpdateLoyaute
        {
            get { return (bool)this.GetValue(IsNeedUpdateLoyauteProperty); }
            set { this.SetValue(IsNeedUpdateLoyauteProperty, value); }
        }

        private static void OnIsNeedUpdateLoyauteChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (UpdateLoyauteBehavior)sender;
            if (behavior.IsNeedUpdateLoyaute)
            {
                var binding = behavior.AssociatedObject.GetBindingExpression(ToggleSwitch.ContentProperty);
                behavior.AssociatedObject.Content = null;
                behavior.AssociatedObject.SetBinding(ToggleSwitch.ContentProperty, binding.ParentBinding);
            }
        }
    }
}
