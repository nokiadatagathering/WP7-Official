using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace NDG.Helpers.Behaviors
{
    public class ValidateTextLenghtBehavior : Behavior<TextBox>
    {
        public static readonly DependencyProperty MaxTextLenghtProperty = DependencyProperty.Register("MaxTextLenght", typeof(int), typeof(ValidateTextLenghtBehavior), new PropertyMetadata(null));
        public static readonly DependencyProperty MaxNumberProperty = DependencyProperty.Register("MaxNumber", typeof(int), typeof(ValidateTextLenghtBehavior), new PropertyMetadata(null));
        public static readonly DependencyProperty MinNumberProperty = DependencyProperty.Register("MinNumber", typeof(int), typeof(ValidateTextLenghtBehavior), new PropertyMetadata(null));
        public static readonly DependencyProperty ValidationTextBlockProperty = DependencyProperty.Register("ValidationTextBlock", typeof(TextBlock), typeof(ValidateTextLenghtBehavior), new PropertyMetadata(null));

        public int MaxTextLenght
        {
            get { return (int)this.GetValue(MaxTextLenghtProperty); }
            set { this.SetValue(MaxTextLenghtProperty, value); }
        }

        public int MaxNumber
        {
            get { return (int)this.GetValue(MaxNumberProperty); }
            set { this.SetValue(MaxNumberProperty, value); }
        }

        public int MinNumber
        {
            get { return (int)this.GetValue(MinNumberProperty); }
            set { this.SetValue(MinNumberProperty, value); }
        }

        public TextBlock ValidationTextBlock
        {
            get { return (TextBlock)this.GetValue(ValidationTextBlockProperty); }
            set { this.SetValue(ValidationTextBlockProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += this.OnLoaded;        
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= this.OnLoaded;
            base.OnDetaching();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.TextChanged += this.OnTextChanged;
            this.AssociatedObject.Unloaded += this.OnUnloaded;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (!string.IsNullOrWhiteSpace(textBox.Text) && textBox.Text.Length > this.MaxTextLenght)
            {
                textBox.Text = textBox.Text.Substring(0, this.MaxTextLenght);
                textBox.SelectionStart = textBox.Text.Length;
            }

            if (this.ValidationTextBlock != null)
            {
                double doubleNumber;
                bool isNumber = double.TryParse(textBox.Text, out doubleNumber);
                if (isNumber && doubleNumber >= this.MinNumber && doubleNumber <= this.MaxNumber)
                {
                    this.ValidationTextBlock.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.ValidationTextBlock.Visibility = Visibility.Visible;
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.TextChanged -= this.OnTextChanged;
            this.AssociatedObject.Unloaded -= this.OnUnloaded;
        }
    }
}
