using System;
using System.Collections.Generic;
using System.Text;

namespace up_mobile.Models
{
    public class Polygon
    {
        public PolygonPoint[] Points { get; set; }

        public class PolygonPoint
        {
            public double X { get; set; }
            public double Y { get; set; }
        }
    }
}
