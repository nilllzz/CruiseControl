using System;

namespace CruiseControl.Services
{
    struct GlobePosition
    {
        public double Latitude, Longitude, Altitude;

        public GlobePosition(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = 0;
        }

        public GlobePosition(double latitude, double longitude, double altitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }

        public GlobePosition OffsetMeters(double metersLat, double metersLng)
        {
            const double R = 6378137;
            
            return new GlobePosition(Latitude - (metersLat / R) * (180 / Math.PI), Longitude + (metersLng / R) * (180 / Math.PI) / Math.Cos(Latitude * Math.PI / 180));
        }

        public override string ToString()
        {
            return $"{Latitude},{Longitude},{Altitude}";
        }

        public double Difference(GlobePosition pos)
        {
            return Math.Abs(Longitude - pos.Longitude) +
                 Math.Abs(Latitude - pos.Latitude);
        }
    }
}
