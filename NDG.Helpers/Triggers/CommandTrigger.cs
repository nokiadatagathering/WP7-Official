// ----------------------------------------------------------------------
// <copyright file="CommandTrigger.cs" company="QArea">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace NDG.Helpers.Triggers
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using Microsoft.Phone.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Trigger for execution command.
    /// </summary>
    public class CommandTrigger : TriggerAction<UserControl>
    {
        /// <summary>
        /// Executable command.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandTrigger), new PropertyMetadata(null));

        /// <summary>
        /// Command parameter.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(CommandTrigger), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
        }

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
        /// Gets or sets command parameter property.
        /// </summary>
        public object CommandParameter
        {
            get
            {
                return this.GetValue(CommandParameterProperty);
            }

            set
            {
                this.SetValue(CommandParameterProperty, value);
            }
        }

        /// <summary>
        /// Executes command with command parameter.
        /// </summary>
        /// <param name="parameter">Invokation parameter.</param>
        protected override void Invoke(object parameter)
        {
            if (this.Command != null)
            {
                this.Command.Execute(this.CommandParameter);
            }
        }
    }
}
