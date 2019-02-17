using System;
using System.Collections.Generic;
using System.Text;

namespace up_mobile.Models
{
    public class ParkingPin
    {
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public double Accuracy { get; set; }
        public DateTime Time { get; set; }

        public ParkingPin(double latitude, double longtitude, double accurracy)
        {
            Latitude = latitude;
            Longtitude = longtitude;
            Accuracy = accurracy;
            Time = DateTime.Now;
        }

        public string getAsString()
        {
            return String.Format("lat: {0} /// long: {1} /// acc: {2} /// time: {3}", Latitude, Longtitude, Accuracy, Time);
        }
    }
}
