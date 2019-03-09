using System;
using up_mobile.Map.Utils;

namespace up_mobile.Models
{
    public class ParkingPin //represents one row in gps data
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public double Latitude { get; set; }    
        public double Longitude { get; set; }
        public double Volume { get; set; }
        public DateTime Timestamp { get; set; }
        public int Lot_id { get; set; }

        //will remove later
        public ParkingPin(double latitude, double longitude, DateTime timestamp)
        {
            Latitude = latitude;
            Longitude = longitude;
            Timestamp = timestamp;
        }

        public string GetPrettyFormat()
        {
            return string.Format("lat: {0} /// long: {1} /// time: {2}", Latitude, Longitude, Timestamp);
        }
    }
}
