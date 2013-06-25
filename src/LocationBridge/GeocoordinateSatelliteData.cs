namespace Windows.Devices.Geolocation
{
    public class GeocoordinateSatelliteData
    {
        // Summary:
        //     Gets the Horizontal Dilution of Precision of a Geocoordinate.
        //
        // Returns:
        //     The Horizontal Dilution of Precision.
        public double? HorizontalDilutionOfPrecision { get; set; }
        //
        // Summary:
        //     Gets the Position Dilution of Precision of a Geocoordinate.
        //
        // Returns:
        //     The Position Dilution of Precision.
        public double? PositionDilutionOfPrecision { get; set; }
        //
        // Summary:
        //     Gets the Vertical Dilution of Precision of a Geocoordinate.
        //
        // Returns:
        //     The Vertical Dilution of Precision.
        public double? VerticalDilutionOfPrecision { get; set; }
    }
}