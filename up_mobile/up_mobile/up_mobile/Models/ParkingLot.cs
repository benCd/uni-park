﻿using System;
using System.Collections.Generic;
using System.Text;

namespace up_mobile.Models
{
    public class ParkingLot
    {
        public int Id { get; set; }
        public int University_Id { get; set; }
        public int Capacity { get; set; }
        public double Center_Long { get; set; }
        public double Center_Lat { get; set; }
        public double Max_Long { get; set; }
        public double Max_Lat { get; set; }
        public double Min_Long { get; set; }
        public double Min_Lat { get; set; }

        public string GetPrettyFormat()
        {
            return string.Format("ID: {0} /// max lat: {1} /// min lat: {2} /// max long: {3} /// min long: {4}",
                Id, Max_Lat, Min_Lat, Max_Long, Min_Long);
        }
    }
}
