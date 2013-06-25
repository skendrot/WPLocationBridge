using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Text;

namespace Windows.Devices.Geolocation
{
    static class LocationExtensions
    {
        public static PositionStatus ToPositionStatus(this GeoPositionStatus status)
        {
            switch (status)
            {
                case GeoPositionStatus.Disabled:
                    return PositionStatus.Disabled;
                case GeoPositionStatus.Ready:
                    return PositionStatus.Ready;
                case GeoPositionStatus.Initializing:
                    return PositionStatus.Initializing;
                case GeoPositionStatus.NoData:
                    return PositionStatus.NoData;
                default:
                    throw new ArgumentOutOfRangeException("status");
            }
        }

        public static double GetDistanceToInFeet(this Geocoordinate source, GeoCoordinate coordinate)
        {
            var startCoord = source.ToGeoCoordinate();
            return startCoord.GetDistanceTo(coordinate)*3.2808399;
        }
    }
}
