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

namespace NDG.DataAccessModels.DataModels
{
    /// <summary>
    /// Pairs for options in Multiple and Exclsuve Choice questions
    /// </summary>
    public class ChoiceTextValuePair
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return this.Text;
        }
    }
}
