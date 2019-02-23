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
        /// Constructor for MapContentPage pages, which are used to display a map of
        /// a parking lot. 
        /// </summary>
        /// <param name="LotId">ID for the parking lot to load</param>
		public MapContentPage(int LotId)
		{
            //TODO IMPLEMENT MAP REST REQUEST!
            var map = new Xamarin.Forms.Maps.Map(
                       MapSpan.FromCenterAndRadius(
                           new Position(37, -122), Distance.FromKilometers(0.3)))
            {
                IsShowingUser = true,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HasScrollEnabled = false,
                HasZoomEnabled = false,
                MapType = MapType.Satellite
            };
            SetPins(map, LotId);
            
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;
        }

        private async void SetPins(Xamarin.Forms.Maps.Map map, int LotId)
        {
            var pins = await PinFactory.GetPinsFor(LotId);

            foreach (Pin p in pins)
            {
                map.Pins.Add(p);
            }
        }
	}
}