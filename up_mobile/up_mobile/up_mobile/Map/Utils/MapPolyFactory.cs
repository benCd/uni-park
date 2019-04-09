using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Backend;
using Xamarin.Forms.Maps;

namespace up_mobile.Map.Utils
{
    /// <summary>
    /// Utility class to create <see cref="MapPolygon"/> objects*
    /// </summary>
    public class MapPolyFactory
    {
        public static async Task<List<MapPolygon>> GetPolygons()
        {
            Debug.Write("Entered GetPolygons");
            var polys = await RestService.service.GetLotPolygons();
            Debug.Write("Got Polygons");
            var o = new List<MapPolygon>();

            if (polys == null)
                Debug.Write("Alert"); //TODO display alert
            Debug.Write("Beginning to set polygons");
            foreach (var p in polys)
            {
                var l = new List<Position>();
                foreach (var point in p.Value.Points)
                    l.Add(new Position(point.Y, point.X));
                o.Add(new MapPolygon()
                {
                    ID = p.Key,
                    Points = l,
                    Percentage = await RestService.service.GetVolumeByLotId(p.Key)
                });
            }
            Debug.Write("Finished setting polygons");
            return o;
        }
    }
}
