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
using up_mobile.Models;
using Rg.Plugins.Popup.Services;

namespace up_mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapContentPage : ContentPage
	{
        /// <summary>
        /// Variable holding the map for every instance of a <see cref="MapContentPage"/>
        /// </summary>
        public static LotMap map;

        /// <summary>
        /// Holds all lots in currently preloaded
        /// </summary>
        public static LotHolder lotholder;

        /// <summary>
        /// Holds the index of lot the map is focusing in on
        /// </summary>
        public static int CurrentLotID { set; get; } = 0;

        private static MapMenu MapM;

        private StackLayout Stack;

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
            Debug.Write("Entering MapContentPage constructor");
            this.Title = "Parking";
            Debug.Write("Starting to create map");
            if (map == null)
                map = new LotMap()
                {
                    IsShowingUser = true,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HasScrollEnabled = false,
                    HasZoomEnabled = true,
                    MapType = MapType.Satellite,
                    ParkingPins = new List<Map.Utils.ParkingPin>(),
                    MapPolygons = new List<MapPolygon>()
                };

            Stack = new StackLayout { Spacing = 0 };

            MapM = new MapMenu();

            Stack.Children.Add(map);

            var buttonToBringUpMapMenu = new Button()
            {
                Text = "Select a lot!"
            };

            buttonToBringUpMapMenu.Clicked += BringUpLotMenu;

            Stack.Children.Add(buttonToBringUpMapMenu);
            Debug.Write("Added Map");
            Content = Stack;

            EnsureLots().ContinueWith(
                    t => 
                    {
                        Debug.Write("-----> Lots Ensured, Moving Map NOW");
                        MoveToLot(lotholder.Lots[0].Id);
                        MapM.Populate();
                    }
                );
        }

        /// <summary>
        /// Map initialiser
        /// </summary>
        public async static void InitMap()
        {
            await EnsureLots().ContinueWith(t =>
            {
                MoveToLot(lotholder.Lots[0].Id);
                MapM.Populate();
            });
            
        }

        /// <summary>
        /// Ensures that all lots are loaded properly
        /// </summary>
        /// <returns>Task</returns>
        private static async Task EnsureLots()
        {
            Debug.Write("Entering ensureLots()!");
            /*
            
            if (!Application.Current.Properties.ContainsKey("UniversityLots"))
                Application.Current.Properties.Add("UniversityLots", await RestService.service.GetMyUniLots());
            else
            {
                Debug.Write("Setting UniversityLots Property");
                Application.Current.Properties["UniversityLots"] = await RestService.service.GetMyUniLots();
            }
            */    
            

            lotholder = await RestService.service.GetMyUniLots();

            Debug.Write("Finished Lot adding continuing!");
        }

        /// <summary>
        /// Clears and sets new Pins for the pass <see cref="Xamarin.Forms.Maps.Map"/> instance.
        /// </summary>
        /// <param name="map"><see cref="Xamarin.Forms.Maps.Map"/> instance to which the pins are added</param>
        /// <param name="LotId"> The ID for the parking lot, whose information should be loaded</param>
        private static async Task SetPins(int LotId)
        {
            Debug.Write("EnteringSetPins!");
            var pins = await PinFactory.GetPinsFor(LotId);
            Device.BeginInvokeOnMainThread(() =>
            {
                map.Pins.Clear();
                map.ParkingPins = pins;
                foreach (Map.Utils.ParkingPin pin in pins)
                {
                    map.Pins.Add(pin);
                }
                    
            });
            Debug.Write("Exiting SetPins!");
        }

        /// <summary>
        /// Refreshes polygons currently displayed in the map
        /// </summary>
        /// <returns>Task</returns>
        private static async Task RefreshPolys()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Moves the map to a different lot.
        /// </summary>
        /// <param name="IDVal">ID of the lot to move to</param>
        public static async void MoveToLot(int IDVal)
        {
            if (lotholder != null && lotholder.Lots!= null && lotholder.Lots[CurrentLotID] != null)
            {
                var plot = lotholder.Lots[0];

                foreach (ParkingLot lot in lotholder.Lots)
                    if (lot.Id == IDVal)
                        plot = lot;

                var position = new Position(plot.Center_Lat, plot.Center_Long);
                var radius = Distance.FromKilometers(0.1);

                Debug.Print("Switching Lot to " + lotholder.Lots[CurrentLotID].Lot_Name);
                var span = MapSpan.FromCenterAndRadius(position, radius);
                Device.BeginInvokeOnMainThread(() => { map.MoveToRegion(span); });
                CurrentLotID = IDVal;
                await SetPins(CurrentLotID);
            }
        }


        /// <summary>
        /// Opens the <see cref="MapMenu"/> popup
        /// </summary>
        /// <param name="sender">Button sender</param>
        /// <param name="args"> Args I guess</param>
        public async void BringUpLotMenu(object sender, EventArgs args)
        {
            await PopupNavigation.Instance.PushAsync(MapM);
        }

        /// <summary>
        /// Updates the pins in the current parkinglot
        /// </summary>
        /// <returns>NOTHING!</returns>
        public static async Task UpdatePins()
        {
            await SetPins(CurrentLotID);
        }

        /// <summary>
        /// Opens the info page for a selected pin
        /// </summary>
        /// <param name="pin">Pin for which the info page is supposed to be loaded</param>
        public static async void BringUpPinInfo(Map.Utils.ParkingPin pin)
        {
            await PopupNavigation.Instance.PushAsync(new PinInfo(pin));
        }

	}
}