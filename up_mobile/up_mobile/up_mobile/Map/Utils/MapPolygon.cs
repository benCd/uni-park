using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace up_mobile.Map.Utils
{
    /// <summary>
    /// Allows for easy access to polygons to be drawn in the map
    /// </summary>
    public class MapPolygon
    {
        public int ID { set; get; }

        /// <summary>
        /// List of GPS positions
        /// </summary>
        public List<Position> Points { set; get; }
        
        /// <summary>
        /// Percentage of occupancy
        /// </summary>
        public double Percentage { set; get; }
    }
}
