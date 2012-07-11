// ----------------------------------------------------------------------
// <copyright file="TextBoxTextChangedBehavior.cs" company="QArea">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace NDG.Helpers.Behaviors
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Interactivity;
    using NDG.Helpers.Controls;

    /// <summary>
    /// Behavior for updating bindable expression in text box when user input text.
    /// </summary>
    public class TextBoxTextChangedBehavior : Behavior<Control>
    {
        /// <summary>
        /// Raises when assosiated object is attached.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += this.OnLoaded;
        }

        /// <summary>
        /// Raises when assosiated object is detaching.
        /// </summary>
        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= this.OnLoaded;
            base.OnDetaching();
        }

        /// <summary>
        /// Raises when assosiated object is loaded.
        /// </summary>
        /// <param name="sender">Assosiated object.</param>
        /// <param name="e">Event parameters.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.AssociatedObject is TextBox)
            {
                ((TextBox)this.AssociatedObject).TextChanged += this.OnTextChanged;
            }
            else if (this.AssociatedObject is PasswordBox)
            {
                ((PasswordBox)this.AssociatedObject).PasswordChanged += this.OnTextChanged;
            }
            else if (AssociatedObject is MaskedBox)
            {
                ((MaskedBox)AssociatedObject).MaskedTextChanged += this.OnTextChanged;
            }
        }

        /// <summary>
        /// Raises by text chenging in assosiated object.
        /// </summary>
        /// <param name="sender">Assosiated object.</param>
        /// <param name="e">Event parameters.</param>
        private void OnTextChanged(object sender, EventArgs e)
        {
            BindingExpression bindingExpression = default(BindingExpression);
            if (this.AssociatedObject is TextBox)
            {
                bindingExpression = this.AssociatedObject.GetBindingExpression(TextBox.TextProperty);
            }
            else if (this.AssociatedObject is PasswordBox)
            {
                bindingExpression = this.AssociatedObject.GetBindingExpression(PasswordBox.PasswordProperty);
            }
            else if (AssociatedObject is MaskedBox)
            {
                bindingExpression = AssociatedObject.GetBindingExpression(MaskedBox.MaskedTextProperty);
            }

            if (bindingExpression != null)
            {
                bindingExpression.UpdateSource();
            }
        }
    }
}
