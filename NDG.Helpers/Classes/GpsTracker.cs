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
using System.ComponentModel;

namespace NDG.Helpers.Classes
{
    public class GpsTracker : INotifyPropertyChanged
    {
        private GpsTracker()
        {

        }

        private bool _gpsAllowed;
        public bool GpsAllowed
        {
            get { return _gpsAllowed; }
            set
            {
                _gpsAllowed = value;
                if (value)
                    StartTracking();
                else
                    StopTracking();

                NotifyPropertyChanged("GpsAllowed");
            }
        }
        private bool _gpsActive;
        public bool GpsActive
        {
            get { return _gpsActive; }
            set
            {
                _gpsActive = value;


                NotifyPropertyChanged("GpsActive");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private static readonly object Lock = new object();
        private static GpsTracker _instance;
        public static GpsTracker Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        _instance = new GpsTracker();

                    }
                }
                return _instance;
            }
        }

        private GeoCoordinateWatcher _watcher;

        private GeoCoordinate _userLocation;
        public GeoCoordinate UserLocation
        {
            get { return _userLocation; }
            set
            {
                _userLocation = value;
                NotifyPropertyChanged("UserLocation");
            }
        }



        public void StartTracking()
        {
            if (GpsAllowed)
            {
                if (_watcher == null)
                {

                    _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High) { MovementThreshold = 20 };
                    _watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(WatcherPositionChanged);
                    _watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(_watcher_StatusChanged);



                }
                _watcher.Start();
            }
           
        }

        public void StopTracking()
        {
            if (_watcher != null)
                _watcher.Stop();
        }

        void _watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            if (e.Status == GeoPositionStatus.Ready)
            {
                GpsActive = true;
            }
        }

        void WatcherPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            UserLocation = e.Position.Location;
        }


    }
}
