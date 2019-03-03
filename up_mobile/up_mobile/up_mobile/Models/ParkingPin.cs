using System;

namespace up_mobile.Models
{
    public class ParkingPin //represents one row in gps data
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
        public int Lot_id { get; set; }

        //will remove later
        public ParkingPin(double latitude, double longitude, double accurracy)
        {
            Latitude = latitude;
            Longitude = longitude;
            //Accuracy = accurracy;
            Timestamp = DateTime.Now;
        }
    }
}
