using System;

namespace Windows.Devices.Geolocation
{
    public sealed class PositionChangedEventArgs : EventArgs
    {
        internal PositionChangedEventArgs(Geoposition position)
        {
            Position = position;
        }
        // Summary:
        //     The location data associated with the PositionChanged event.
        //
        // Returns:
        //     A Geoposition object containing geographic location data.
        public Geoposition Position { get; set; }
    }
}