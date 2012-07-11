// ----------------------------------------------------------------------
// <copyright file="EnterPressedBehavior.cs" company="QArea">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace NDG.Helpers.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using NDG.Helpers.Controls;

    /// <summary>
    /// Behavior for execute command on enter press or change focused elemnt or unfocus element.
    /// </summary>
    public class EnterPressedBehavior : Behavior<FrameworkElement>
    {
        #region DependencyProperies

        /// <summary>
        /// Command for execution by enter press.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EnterPressedBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Command parameter.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(EnterPressedBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Next focused element.
        /// </summary>
        public static readonly DependencyProperty NextElementProperty = DependencyProperty.Register("NextElement", typeof(Control), typeof(EnterPressedBehavior), new PropertyMetadata(null));

        #endregion DependencyProperies

        #region Properties

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
        /// Gets or sets next element.
        /// </summary>
        public Control NextElement
        {
            get
            {
                return (Control)this.GetValue(NextElementProperty);
            }

            set
            {
                this.SetValue(NextElementProperty, value);
            }
        }

        #endregion Properties

        /// <summary>
        /// Raises when assosiated object is attached.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.KeyDown += this.OnKeyDown;
        }

        /// <summary>
        /// Raises when assosiated object is detaching.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.KeyDown -= this.OnKeyDown;
        }

        /// <summary>
        /// Raises by key down on assosiated object.
        /// </summary>
        /// <param name="sender">Assosiated object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (this.Command != null)
                {
                    this.Command.Execute(this.CommandParameter);
                }
                else if (this.NextElement != null)
                {
                    if (NextElement is MaskedBox)
                    {
                        ((MaskedBox)NextElement).Focus();
                    }
                    else
                    {
                        this.NextElement.Focus();
                    }
                }
                else if (AssociatedObject is Control)
                {
                    ((Control)AssociatedObject).IsEnabled = false;
                    ((Control)AssociatedObject).IsEnabled = true;
                }
            }
        }
    }
}
