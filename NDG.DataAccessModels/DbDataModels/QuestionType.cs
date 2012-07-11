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

namespace NDG.DataAccessModels
{
    //NOTE: Add type here when new question types will appear
    /// <summary>
    /// Enumeration for all question types in the system
    /// </summary>
    public enum QuestionType
    {
        IntegerQuestion = 1,
        DescriptiveQuestion = 2,
        DateQuestion = 3,
        DecimalQuestion = 4,
        ExclusiveChoiceQuestion = 5,
        ImageQuestion = 6,
        MultipleChocieQuestion = 7,
        TimeQuestion = 8,
    }
}
