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
using System.Collections.ObjectModel;
using NDG.DataAccessModels.DataModels;

namespace NDG.Common
{
    public enum FilterType
    {
        ByDate,
        ByGps,
        ByAddress
    }

    public enum TimePeriods
    {
        At,
        After,
        Before,
        Between
    }

    public class TimePeriodKeyValuePair
    {
        public TimePeriodKeyValuePair() { }

        public TimePeriodKeyValuePair(TimePeriods period, string value)
        {
            this.Key = period;
            this.Value = value;
        }

        public TimePeriods Key { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return this.Value;
        }
    }

    public class DateParameters
    {
        public TimePeriodKeyValuePair SelectedPeriod { get; set; }

        public DateTime SelectedDate { get; set; }

        public DateTime SelectedStartDate { get; set; }

        public DateTime SelectedEndDate { get; set; }
    }

    public class AddressParameters
    {
        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }
    }

    public class FilterParameters
    {
        public FilterType Type { get; set; }

        public DateParameters Date { get; set; }

        public AddressParameters Address { get; set; }
    }
}
