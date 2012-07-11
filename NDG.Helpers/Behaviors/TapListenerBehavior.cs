// ----------------------------------------------------------------------
// <copyright file="TapListenerBehavior.cs" company="QArea">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace NDG.Helpers.Behaviors
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    /// <summary>
    /// Invokes command by tap on assosiated object.
    /// </summary>
    public class TapListenerBehavior : Behavior<UIElement>
    {
        /// <summary>
        /// Command property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(TapListenerBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Command parameter property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(TapListenerBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets command property.
        /// </summary>
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

        /// <summary>
        /// Gets or sets command parameter.
        /// </summary>
        public object CommandParameter
        {
            get
            {
                return (object)this.GetValue(CommandParameterProperty);
            }

            set
            {
                this.SetValue(CommandParameterProperty, value);
            }
        }

        /// <summary>
        /// Raises by attaching assosiated object.
        /// </summary>
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

        /// <summary>
        /// Raises by tap on assosiated object.
        /// </summary>
        /// <param name="sender">Assosiated object.</param>
        /// <param name="e">Event parameters.</param>
        private void OnTap(object sender, GestureEventArgs e)
        {
            if (this.Command != null)
            {
                this.Command.Execute(this.CommandParameter);
            }
        }
    }
}
