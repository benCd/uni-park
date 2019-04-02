using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace up_mobile.Map.Utils
{
    /// <summary>
    /// The map object holding displayable data
    /// </summary>
    public class LotMap : Xamarin.Forms.Maps.Map
    {
        /// <summary>
        /// Parking pins displayed in the map
        /// </summary>
        public List<ParkingPin> ParkingPins { get; set; } = new List<ParkingPin>();

        /// <summary>
        /// Parking lot polygons drawn and coloured in the map
        /// </summary>
        public List<MapPolygon> MapPolygons { get; set; } = new List<MapPolygon>();
    }
}
