using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Map.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using up_mobile.Backend;

namespace up_mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapContentPage : ContentPage
	{
        /// <summary>
        /// Variable holding the map for every instance of a <see cref="MapContentPage"/>
        /// </summary>
        private static LotMap map;

        /// <summary>
        /// Holds the current lot the map is focusing in on
        /// </summary>
        public static int CurrentLotId { set;  get; }

        public MapContentPage() : this(0)
        {

        }

        /// <summary>
        /// Constructor for MapContentPage pages, which are used to display a map of
        /// a parking lot. 
        /// </summary>
        /// <param name="LotId">ID for the parking lot to load</param>
		public MapContentPage(int LotId = 0) : base()
		{
           
            this.Title = "Lot XYZ";
            //TODO IMPLEMENT MAP REST REQUEST!

            makeMap();

            while (map == null) ;
            
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            stack.Children.Add(new MapMenu());
            Content = stack;
        }

        private static async void makeMap()
        {

            lotholder = await RestService.service.GetMyUniLots();
            if(map == null)
                map = new LotMap(
                           MapSpan.FromCenterAndRadius(
                               new Position(lotholder.Lots[0].Center_Lat, lotholder.Lots[0].Center_Long), Distance.FromKilometers(0.1)))
                {
                    IsShowingUser = true,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HasScrollEnabled = false,
                    HasZoomEnabled = false,
                    MapType = MapType.Satellite
                };

            SetPins(0);
        }

        /// <summary>
        /// Clears and sets new Pins for the pass <see cref="Xamarin.Forms.Maps.Map"/> instance.
        /// </summary>
        /// <param name="map"><see cref="Xamarin.Forms.Maps.Map"/> instance to which the pins are added</param>
        /// <param name="LotId"> The ID for the parking lot, whose information should be loaded</param>
        private static async void SetPins(int LotId)
        {
            map.ParkingPins.Clear();

            var pins = await PinFactory.GetPinsFor(LotId);

            foreach (ParkingPin p in pins)
            {
                map.ParkingPins.Add(p);
            }
        }

        /// <summary>
        /// Moves the map to a different lot.
        /// </summary>
        /// <param name="LotId">ID of the lot to move to</param>
        public static async void MoveToLot(int LotId)
        {
            CurrentLotId = LotId;

            //------------------------------------------
            //TODO request lot information from REST API
            //REQUEST GOES HERE
            //------------------------------------------
            
            var position = new Position(42.671133, -83.2149);
            var radius = Distance.FromKilometers(0.1);

            switch (LotId)
            {
                case 0:
                    position = new Position(42.671133, -83.2149);
                    radius = Distance.FromKilometers(0.1);
                    break;
                case 1:
                    position = new Position(42.671133, -83.25);
                    radius = Distance.FromKilometers(0.5);
                    break;
                case 2:
                    position = new Position(42.671133, -83.214928);
                    radius = Distance.FromKilometers(0.7);
                    break;
            }
            Debug.Print("Button: " + LotId);
            var span = MapSpan.FromCenterAndRadius(position, radius);
            map.MoveToRegion(span);
            SetPins(LotId);
        }
	}
}