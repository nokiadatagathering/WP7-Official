using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NDG.Helpers.Controls
{
    public class MaskedBox : Control
    {
        private TextBox startTextBox;
        private TextBox endTextBox;
        private TextBox sampleText;
        private int currentStartTextIndex;

        #region DependencyProperties

        // Full text with of control: startText + innerText + endText.
        public static readonly DependencyProperty MaskedTextProperty = DependencyProperty.Register(
            "MaskedText",
            typeof(string),
            typeof(MaskedBox),
            new PropertyMetadata(OnMaskedTextChanged));

        // Mask for start, that will be placed before enabled text box.
        public static readonly DependencyProperty StartTextProperty = DependencyProperty.Register(
            "StartText",
            typeof(string),
            typeof(MaskedBox),
            new PropertyMetadata(OnStartTextChanged));

        // Mask for end, that will be placed after enabled text box.
        public static readonly DependencyProperty EndTextProperty = DependencyProperty.Register(
            "EndText",
            typeof(string),
            typeof(MaskedBox),
            new PropertyMetadata(OnEndTextChanged));

        // List of start text, that can be changed by tap on it.
        public static readonly DependencyProperty StartTextsProperty = DependencyProperty.Register(
            "StartTexts",
            typeof(List<string>),
            typeof(MaskedBox),
            new PropertyMetadata(OnStartTextsChanged));

        #endregion DependencyProperties

        #region Properties

        public event TextChangedEventHandler MaskedTextChanged;

        public string MaskedText
        {
            get { return (string)this.GetValue(MaskedTextProperty); }
            set { this.SetValue(MaskedTextProperty, value); }
        }

        public string StartText
        {
            get { return (string)this.GetValue(StartTextProperty); }
            set { this.SetValue(StartTextProperty, value); }
        }

        public string EndText
        {
            get { return (string)this.GetValue(EndTextProperty); }
            set { this.SetValue(EndTextProperty, value); }
        }

        public List<string> StartTexts
        {
            get { return (List<string>)this.GetValue(StartTextsProperty); }
            set { this.SetValue(StartTextsProperty, value); }
        }

        #endregion Properties

        public MaskedBox()
        {
            this.DefaultStyleKey = typeof(MaskedBox);
        }

        public new void Focus()
        {
            if (this.sampleText != null)
            {
                this.sampleText.Focus();
                this.sampleText.SelectionStart = sampleText.Text != null ? this.sampleText.Text.Length : 0;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.startTextBox = this.GetTemplateChild("startText") as TextBox;
            this.endTextBox = this.GetTemplateChild("endText") as TextBox;
            this.sampleText = this.GetTemplateChild("sampleText") as TextBox;

            this.startTextBox.Visibility = this.endTextBox.Visibility = System.Windows.Visibility.Collapsed;
            this.sampleText.TextChanged += this.OnSampleTextChanged;
            this.startTextBox.TextChanged += this.OnSampleTextChanged;
            this.CheckForStartText();
            this.CheckForStartTexts();
            this.CheckForEndText();
            this.CheckForMaskedText();
        }

        #region PropertyChanged

        private static void OnMaskedTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var maskedBox = sender as MaskedBox;
            maskedBox.CheckForMaskedText();
        }

        private static void OnStartTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var maskedBox = sender as MaskedBox;
            maskedBox.CheckForStartText();
        }

        private static void OnEndTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var maskedBox = sender as MaskedBox;
            maskedBox.CheckForEndText();
        }

        private static void OnStartTextsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var maskedBox = sender as MaskedBox;
            maskedBox.CheckForStartTexts();
        }

        #endregion PropertyChanged

        #region Methods

        private void CheckForMaskedText()
        {
            if (this.sampleText != null)
            {
                string currentMaskedText = this.startTextBox.Text + this.sampleText.Text + this.endTextBox.Text;
                if (this.MaskedText != currentMaskedText)
                {
                    string sampleTextString = string.Empty;
                    if (!string.IsNullOrEmpty(this.StartText))
                    {
                        sampleTextString = this.MaskedText.Replace(this.StartText, string.Empty);
                    }
                    else if (this.StartTexts != null && this.StartTexts.Count > 0)
                    {
                        foreach (var item in this.StartTexts)
                        {
                            if (this.MaskedText.StartsWith(item))
                            {
                                this.startTextBox.Text = item;
                                this.currentStartTextIndex = this.StartTexts.IndexOf(item);
                                sampleTextString = this.MaskedText.Replace(item, string.Empty);
                                break;
                            }
                        }
                    }
                                        
                    sampleTextString = sampleTextString.Replace(this.endTextBox.Text, string.Empty);
                    this.sampleText.Text = sampleTextString;
                }
            }
        }

        private void CheckForEndText()
        {
            if (this.endTextBox != null)
            {
                if (string.IsNullOrEmpty(this.EndText))
                {
                    this.endTextBox.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.endTextBox.Visibility = Visibility.Visible;
                }
            }
        }

        private void CheckForStartText()
        {
            if (this.startTextBox != null)
            {
                if (string.IsNullOrEmpty(this.StartText))
                {
                    this.startTextBox.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.startTextBox.Visibility = Visibility.Visible;
                }

                this.startTextBox.Tap -= this.OnStartTextTap;
            }
        }

        private void CheckForStartTexts()
        {
            if (this.startTextBox != null)
            {
                if (this.StartTexts != null && this.StartTexts.Count > 0)
                {
                    this.currentStartTextIndex = 0;
                    this.startTextBox.Text = this.StartTexts[this.currentStartTextIndex];
                    this.startTextBox.Visibility = Visibility.Visible;
                    this.startTextBox.Tap += this.OnStartTextTap;
                }
                else if (string.IsNullOrEmpty(this.StartText))
                {
                    this.startTextBox.Visibility = Visibility.Collapsed;
                }
            }
        }

        #endregion Methods

        private void OnStartTextTap(object sender, GestureEventArgs e)
        {
            this.currentStartTextIndex++;
            if (this.currentStartTextIndex == this.StartTexts.Count)
            {
                this.currentStartTextIndex = 0;
            }

            this.startTextBox.Text = this.StartTexts[this.currentStartTextIndex];
            this.sampleText.Focus();
            this.sampleText.SelectionStart = !string.IsNullOrEmpty(sampleText.Text) ? this.sampleText.Text.Length : 0;
        }

        private void OnSampleTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.sampleText.Text) && !string.IsNullOrEmpty(this.sampleText.Text.Trim()))
            {
                this.MaskedText = this.startTextBox.Text + this.sampleText.Text + this.endTextBox.Text;
                if (this.MaskedTextChanged != null)
                {
                    this.MaskedTextChanged(this, e);
                }
            }
        }
    }
}
