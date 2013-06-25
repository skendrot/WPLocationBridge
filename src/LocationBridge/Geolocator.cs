using System;
using System.Device.Location;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Windows.Devices.Geolocation
{
    /// <summary>
    /// Provides access to the current geographic location.
    /// </summary>
    public class Geolocator
    {
        private static bool _disabled;

        private PositionStatus _status = PositionStatus.NoData;
        private GeoCoordinateWatcher _watcher;

        private TypedEventHandler<Geolocator, PositionChangedEventArgs> _positionChangedDelegate;
        private readonly object _padLock = new object();

        /// <summary>
        /// Initializes a new Geolocator object.
        /// </summary>
        public Geolocator()
        {
            _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default) { MovementThreshold = 10 };
        }

        /// <summary>
        /// Raised when the location is updated.
        /// </summary>
        public event TypedEventHandler<Geolocator, PositionChangedEventArgs> PositionChanged
        {
            add
            {
                lock (_padLock)
                {
                    if (_positionChangedDelegate == null)
                    {
                        // if this is the first subscription to the PositionChanged event
                        // then subscribe to the GeoCoordinateWatcher events and start
                        // tracking location.
                        _watcher.PositionChanged += OnPositionChanged;
                        _watcher.StatusChanged += OnStatusChanged;
                        _watcher.Start();
                    }
                    _positionChangedDelegate += value;
                }
            }
            remove
            {
                lock (_padLock)
                {
                    _positionChangedDelegate -= value;
                    if (_positionChangedDelegate == null)
                    {
                        // Last person to unsubscribe from this event
                        // Stop the GeoCoordinateWatcher and unsubscribe from the events.
                        _watcher.Stop();
                        _watcher.PositionChanged -= OnPositionChanged;
                        _watcher.StatusChanged -= OnStatusChanged;
                    }
                }
            }
        }

        /// <summary>
        /// Raised when the ability of the Geolocator to provide updated location changes.
        /// </summary>
        public event TypedEventHandler<Geolocator, StatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// The status that indicates the ability of the Geolocator to provide location
        /// updates.
        /// </summary>
        /// <returns>The status of the Geolocator.</returns>
        public PositionStatus LocationStatus
        {
            get
            {
                if (_disabled) return PositionStatus.Disabled;
                return _status;
            }
        }

        /// <summary>
        /// Initializes an asynchronous operation to retrieve the location of the user's
        /// computer.
        /// </summary>
        /// <returns>
        /// Provides methods for starting the asynchronous request for location data
        /// and handling its completion.
        /// </returns>
        public Task<Geoposition> GetGeopositionAsync()
        {
            var completion = new TaskCompletionSource<Geoposition>();

            var locator = new Geolocator()
                {
                    DesiredAccuracyInMeters = DesiredAccuracyInMeters,
                    MovementThreshold = MovementThreshold,
                    ReportInterval = ReportInterval
                };

            TypedEventHandler<Geolocator, PositionChangedEventArgs> positionChangedHandler = null;
            TypedEventHandler<Geolocator, StatusChangedEventArgs> statusChangedHandler = null;

            positionChangedHandler = (s, e) =>
                {
                    locator.PositionChanged -= positionChangedHandler;
                    locator.StatusChanged -= statusChangedHandler;
                    completion.SetResult(e.Position);
                };

            statusChangedHandler = (sender, args) =>
                {
                    if (args.Status == PositionStatus.Disabled)
                    {
                        // unsubscribe as we will not get any data
                        locator.PositionChanged -= positionChangedHandler;
                        locator.StatusChanged -= statusChangedHandler;
                        completion.SetResult(null);
                    }
                };

            locator.PositionChanged += positionChangedHandler;
            locator.StatusChanged += statusChangedHandler;

            return completion.Task;
        }

        /// <summary>
        /// Gets or sets the desired accuracy in meters for data returned from the location
        /// service.
        /// </summary>
        /// <returns>
        /// The desired accuracy in meters for data returned from the location service.
        /// </returns>
        public double DesiredAccuracyInMeters
        {
            get { return _watcher.MovementThreshold; }
            set { _watcher.MovementThreshold = value; }
        }

        /// <summary>
        /// Gets the distance of movement, in meters, relative to the coordinate from//     
        /// the last PositionChanged event, that is required for the Geolocator to raise//     
        /// a PositionChanged event.
        /// </summary>
        /// <returns>
        /// The distance of required movement, in meters, for the location provider to
        /// raise a PositionChanged event.
        /// </returns>
        public double MovementThreshold
        {
            get { return _watcher.MovementThreshold; }
            set
            {
                if (_positionChangedDelegate != null)
                    throw new NotSupportedException("Cannot change the MovementThreshold while getting location.");

                _watcher.MovementThreshold = value;
            }
        }

        /// <summary>
        /// The accuracy level at which the Geolocator provides location updates.
        /// </summary>
        /// <returns>
        /// The accuracy level at which the Geolocator provides location updates.
        /// </returns>
        public PositionAccuracy DesiredAccuracy
        {
            get { return (PositionAccuracy)_watcher.DesiredAccuracy; }
            set
            {
                if (_positionChangedDelegate != null)
                    throw new NotSupportedException("Cannot change the DesiredAccuracy while getting location.");

                double movementThreshold = MovementThreshold;
                // We cannot change the accuracy in a GeoCoodinateWatcher so we need to create a new one
                _watcher = new GeoCoordinateWatcher((GeoPositionAccuracy)value) 
                    { MovementThreshold = movementThreshold };
            }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public int ReportInterval { get; set; }

        private void OnStatusChanged(object sender, GeoPositionStatusChangedEventArgs args)
        {
            var handler = StatusChanged;
            if (handler != null)
            {
                _status = args.Status.ToPositionStatus();
                _disabled = _status == PositionStatus.Disabled;
                var changedEventArgs = new StatusChangedEventArgs(_status);
                handler(this, changedEventArgs);
            }
        }

        private void OnPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> args)
        {
            var handler = _positionChangedDelegate;
            if (handler != null)
            {
                var geoposition = new Geoposition(args.Position);
                handler(this, new PositionChangedEventArgs(geoposition));
            }
        }
    }
}
