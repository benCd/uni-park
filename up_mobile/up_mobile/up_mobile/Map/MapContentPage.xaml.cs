using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;

namespace up_mobile.Map
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapContentPage : ContentPage
	{
        /// <summary>
        /// Variable holding the map for every instance of a <see cref="MapContentPage"/>
        /// </summary>
        private static Xamarin.Forms.Maps.Map map;

        /// <summary>
        /// Constructor for MapContentPage pages, which are used to display a map of
        /// a parking lot. 
        /// </summary>
        /// <param name="LotId">ID for the parking lot to load</param>
		public MapContentPage(int LotId)
		{
            this.Title = "Lot XYZ";
            //TODO IMPLEMENT MAP REST REQUEST!

            if(map == null)
                map = new Xamarin.Forms.Maps.Map(
                           MapSpan.FromCenterAndRadius(
                               new Position(42.671133, -83.214928), Distance.FromKilometers(0.1)))
                {
                    IsShowingUser = true,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HasScrollEnabled = false,
                    HasZoomEnabled = false,
                    MapType = MapType.Satellite
                };

            SetPins(LotId);
            
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            stack.Children.Add(new MapMenu());
            Content = stack;
        }

        /// <summary>
        /// Clears and sets new Pins for the pass <see cref="Xamarin.Forms.Maps.Map"/> instance.
        /// </summary>
        /// <param name="map"><see cref="Xamarin.Forms.Maps.Map"/> instance to which the pins are added</param>
        /// <param name="LotId"> The ID for the parking lot, whose information should be loaded</param>
        private static async void SetPins(int LotId)
        {
            map.Pins.Clear();

            var pins = await PinFactory.GetPinsFor(LotId);

            foreach (Pin p in pins)
            {
                map.Pins.Add(p);
            }
        }

        /// <summary>
        /// Moves the map to a different lot.
        /// </summary>
        /// <param name="LotId">ID of the lot to move to</param>
        public static async void MoveToLot(int LotId)
        {
            //------------------------------------------
            //TODO request lot information from REST API
            //REQUEST GOES HERE
            //------------------------------------------
            
            var position = new Position(42.671133, -83.2149);
            var radius = Distance.FromKilometers(0.1);
            var span = MapSpan.FromCenterAndRadius(position, radius);
            map.MoveToRegion(span);
        }
	}
}