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
        public string Date { get; set; }
        public string Note { get; set; }
        public string User { get; set; }
        public double Percentage { get; set; }

        public ParkingPin(double latitude, double longtitude, double accuracy)
        {
            Latitude = latitude;
            Longtitude = longtitude;
            Accuracy = accuracy;
            Date = DateTime.Now.ToString();
        }

        public string GetPrettyFormat()
        {
            return string.Format("lat: {0} /// long: {1} /// acc: {2} /// time: {3}", Latitude, Longtitude, Accuracy, Date);
        }
    }
}
