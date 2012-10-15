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
using System.Device.Location;
using GalaSoft.MvvmLight.Command;
using NDG.Helpers.Classes;
using Microsoft.Phone.Controls;

namespace NDG.Helpers.Controls
{
    public class Coordinate
    {
        public GeoCoordinate Location { get; set; }

        public RelayCommand<Coordinate> RemoveCommand { get; set; }
    }

    public class GeopointPicker : Control
    {
        #region DependencyProperties

        public static readonly DependencyProperty RecordGeoButtonStyleProperty = DependencyProperty.Register("RecordGeoButtonStyle", typeof(string), typeof(GeopointPicker), new PropertyMetadata(null));

        public static readonly DependencyProperty RemoveGeoButtonStyleProperty = DependencyProperty.Register("RemoveGeoButtonStyle", typeof(string), typeof(GeopointPicker), new PropertyMetadata(null));

        public static readonly DependencyProperty RecordedLocationProperty = DependencyProperty.Register("RecordedLocation", typeof(string), typeof(GeopointPicker), new PropertyMetadata(null));

        #endregion DependencyProperties

        private Button recordGeoButton;
        private TextBlock geoTextBlock;
        private PerformanceProgressBar performanceProgressBar;
        private bool wasAplpyTemplate = false;
        private bool isRecordedLocationChanged = false;

        public String RecordedLocation
        {
            get { return (String)this.GetValue(RecordedLocationProperty); }
            set { this.SetValue(RecordedLocationProperty, value); }
        }

        public string RecordGeoButtonStyle
        {
            get { return (string)this.GetValue(RecordGeoButtonStyleProperty); }
            set { this.SetValue(RecordGeoButtonStyleProperty, value); }
        }

        public string RemoveGeoButtonStyle
        {
            get { return (string)this.GetValue(RemoveGeoButtonStyleProperty); }
            set { this.SetValue(RemoveGeoButtonStyleProperty, value); }
        }

        public GeopointPicker()
        {
            this.DefaultStyleKey = typeof(GeopointPicker);
        }

        public event EventHandler RecordedLocationChanged;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.recordGeoButton = this.GetTemplateChild("recordGeoButton") as Button;
            this.performanceProgressBar = this.GetTemplateChild("performanceProgressBar") as PerformanceProgressBar;
            this.geoTextBlock = this.GetTemplateChild("geoTextBlock") as TextBlock;
            this.recordGeoButton.Click += this.OnRecordButtonClick;
            this.wasAplpyTemplate = true;
            if (this.isRecordedLocationChanged)
            {
                //AddRecordedLocation(this);
            }
        }

        //private static void OnRecordedLocationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    GeopointPicker picker = (GeopointPicker)sender;
        //    if (picker.wasAplpyTemplate)
        //    {
        //        picker.AddRecordedLocation(picker);
        //    }
        //    else
        //    {
        //        picker.isRecordedLocationChanged = true;
        //    }

        //    if (picker.RecordedLocationChanged != null)
        //    {
        //        picker.RecordedLocationChanged.Invoke(picker, new EventArgs());
        //    }
        //}

        private void OnRecordButtonClick(object sender, RoutedEventArgs e)
        {
            this.recordGeoButton.Visibility = System.Windows.Visibility.Collapsed;
            this.recordGeoButton.Click -= this.OnRecordButtonClick;

            this.performanceProgressBar.IsIndeterminate = true;

            if (GpsTracker.Instance.UserLocation != null)
            {
                this.RecordedLocation = new GeoCoordinate(GpsTracker.Instance.UserLocation.Latitude, GpsTracker.Instance.UserLocation.Longitude).ToString();
                this.geoTextBlock.Visibility = System.Windows.Visibility.Visible;
                this.geoTextBlock.Text = this.RecordedLocation;
                this.performanceProgressBar.IsIndeterminate = false;
            }
        }

        private void OnRemoveButtonClick(object sender, RoutedEventArgs e)
        {
        }

        //private void AddRecordedLocation(GeopointPicker picker)
        //{
        //    if (picker.RecordedLocation != null)
        //    {
        //        picker.recordGeoButton.Click -= picker.OnAddButtonClick;
        //        picker.recordGeoButton.Visibility = System.Windows.Visibility.Collapsed;
        //        picker.LocationCollection = new List<GeoCoordinate>();
        //        picker.items = new Collection<Coordinate>();
        //        picker.LocationCollection.Add(picker.RecordedLocation);
        //        picker.co(new Coordinate() { Location = picker.RecordedLocation, RemoveGeoCommand = new RelayCommand<Coordinate>(picker.OnRemoveButtonClick) });
        //        picker.itemsControl.ItemsSource = null;
        //        picker.itemsControl.ItemsSource = picker.items;
        //    }
        //}
    }
}
