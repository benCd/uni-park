using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace up_mobile.Map.Utils
{
    public class LotMap : Xamarin.Forms.Maps.Map
    {
        public List<ParkingPin> ParkingPins { get; set; } = new List<ParkingPin>();
    }
}
