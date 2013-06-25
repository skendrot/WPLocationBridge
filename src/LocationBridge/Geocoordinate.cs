using System;
using System.Device.Location;

namespace Windows.Devices.Geolocation
{
    public class Geocoordinate
    {
        private readonly GeoCoordinate _location;

        public Geocoordinate()
            : this(new GeoCoordinate())
        {

        }

        public Geocoordinate(GeoCoordinate location)
        {
            if (location == null) throw new ArgumentNullException("location");

            _location = location;
        }

        // Summary:
        //     The accuracy of the location in meters.
        //
        // Returns:
        //     The accuracy in meters.
        public double Accuracy { get; set; }
        //
        // Summary:
        //     The altitude of the location, in meters.
        //
        // Returns:
        //     The altitude in meters.
        public double Altitude { get { return _location.Altitude; } set { _location.Altitude = value; } }
        //
        // Summary:
        //     The accuracy of the altitude, in meters.
        //
        // Returns:
        //     The accuracy of the altitude.
        public double? AltitudeAccuracy { get; set; }
        //
        // Summary:
        //     The current heading in degrees relative to true north.
        //
        // Returns:
        //     The current heading in degrees relative to true north.
        public double? Heading { get; set; }
        //
        // Summary:
        //     The latitude in degrees.
        //
        // Returns:
        //     The latitude in degrees.
        public double Latitude { get { return _location.Latitude; } set { _location.Latitude = value; } }
        //
        // Summary:
        //     The longitude in degrees.
        //
        // Returns:
        //     The longitude in degrees.
        public double Longitude { get { return _location.Longitude; } set { _location.Longitude = value; } }
        //
        // Summary:
        //     Gets the source used to obtain a Geocoordinate.
        //
        // Returns:
        //     The source used to obtain a Geocoordinate.
        public PositionSource PositionSource { get; set; }
        //
        // Summary:
        //     Gets information about the satellites used to obtain a Geocoordinate.
        //
        // Returns:
        //     Information about the satellites used to obtain a Geocoordinate.
        public GeocoordinateSatelliteData SatelliteData { get; set; }
        //
        // Summary:
        //     The speed in meters per second.
        //
        // Returns:
        //     The speed in meters per second.
        public double? Speed { get; set; }
        //
        // Summary:
        //     The time at which the location was determined.
        //
        // Returns:
        //     The time at which the location was determined.
        public DateTimeOffset Timestamp { get; set; }

        public double GetDistanceTo(Geocoordinate other)
        {
            return _location.GetDistanceTo(new GeoCoordinate(other.Latitude, other.Longitude));
        }

        public GeoCoordinate ToGeoCoordinate()
        {
            return new GeoCoordinate(Latitude, Longitude);
        }
    }
}