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
        /// Holds all lots in currently preloaded
        /// </summary>
        private static LotHolder lotholder;

        /// <summary>
        /// Holds the index of lot the map is focusing in on
        /// </summary>
        public static int CurrentLotIndex { set; get; } = 8;

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

            ensureLots().ContinueWith(
                    t => 
                    {
                        lotholder = (LotHolder)Application.Current.Properties["UniversityLots"];
                        Debug.Write("Starting to create map");
                        if (map == null)
                            map = new LotMap(/*
                                       MapSpan.FromCenterAndRadius(
                                           new Position(lotholder.Lots[0].Center_Lat, lotholder.Lots[0].Center_Long), Distance.FromKilometers(0.1))
                                           */)
                        {
                            IsShowingUser = true,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HasScrollEnabled = false,
                            HasZoomEnabled = false,
                            MapType = MapType.Satellite,
                            ParkingPins = new List<Map.Utils.ParkingPin>()
                        };


                        SetPins(CurrentLotIndex).ContinueWith(t2=> {
                            Debug.Write("Done Setting Map and Pins");
                            var stack = new StackLayout { Spacing = 0 };
                            stack.Children.Add(map);
                            Debug.Write("Added Map");
                            stack.Children.Add(new MapMenu());
                            Debug.Write("Added Menu");
                            Debug.Write(Content);
                            Content = stack;
                            Debug.Write("Finished creating map");
                            MoveToLot(CurrentLotIndex);
                        });
                    }
                );
        }

        private static async Task ensureLots()
        {
            Debug.Write("Entering ensureLots()!");
            if (!Application.Current.Properties.ContainsKey("UniversityLots"))
                Application.Current.Properties.Add("UniversityLots", await RestService.service.GetMyUniLots());
            if (Application.Current.Properties["UniversityLots"] == null)
                Application.Current.Properties["UniversityLots"] = await RestService.service.GetMyUniLots();
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
            map.ParkingPins = await PinFactory.GetPinsFor(LotId);
            Debug.Write("Exiting SetPins!");
        }

        /// <summary>
        /// Moves the map to a different lot.
        /// </summary>
        /// <param name="LotIndex">ID of the lot to move to</param>
        public static async void MoveToLot(int LotIndex)
        {
            CurrentLotIndex = LotIndex;

            var position = new Position(lotholder.Lots[CurrentLotIndex].Center_Lat, lotholder.Lots[CurrentLotIndex].Center_Long);
            var radius = Distance.FromKilometers(0.1);

            Debug.Print("Switching Lot to " + lotholder.Lots[CurrentLotIndex].Lot_Name);
            var span = MapSpan.FromCenterAndRadius(position, radius);
            map.MoveToRegion(span);
            SetPins(lotholder.Lots[CurrentLotIndex].Id);
        }
	}
}