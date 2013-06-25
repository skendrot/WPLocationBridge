using System;
using System.Device.Location;

namespace Windows.Devices.Geolocation
{
    public class StatusChangedEventArgs : EventArgs
    {
        public StatusChangedEventArgs(PositionStatus status)
        {
            Status = status;
        }

        public PositionStatus Status { get; private set; }
    }
}