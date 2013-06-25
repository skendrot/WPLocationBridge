using System;
using System.Device.Location;

namespace Windows.Devices.Geolocation
{
    public class Geoposition
    {
        public Geoposition(GeoPosition<GeoCoordinate> position)
        {
            if (position == null) throw new ArgumentNullException("position");

            Coordinate = new Geocoordinate(position.Location);
        }

        // Summary:
        //     Contains civic address data associated with a geographic location.
        //
        // Returns:
        //     The civic address data associated with a geographic location.
        public CivicAddress CivicAddress { get; private set; }
        //
        // Summary:
        //     The latitude and longitude associated with a geographic location.
        //
        // Returns:
        //     The latitude and longitude associated with a geographic location.
        public Geocoordinate Coordinate { get; private set; }
    }
}