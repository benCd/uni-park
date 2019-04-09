using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Backend;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile.Map
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BestLots : Rg.Plugins.Popup.Pages.PopupPage
    {
        /// <summary>
        /// Holds time for the request
        /// </summary>
        public TimeSpan CurrentTime { get; set; }
        /// <summary>
        /// Holds building for the request
        /// </summary>
        public int BuildingID { get; set; }

        /// <summary>
        /// Holds the map "name" -> "building_id"
        /// </summary>
        private Dictionary<string, int> BuildingMap;

        //private static List<Models.Building> Buildings;

        /// <summary>
        /// Constructor for the BestLots popup
        /// </summary>
        public BestLots()
        {
            BuildingMap = new Dictionary<string, int>();

            InitializeComponent();

            Debug.Write("Initialising BestLots");

            Task task = new Task(
                new Action(
                    async () =>
                    {
                        Models.BuildingHolder buildings = await RestService.service.GetMyUniBuildings();
                        Debug.Write("Got buildings!");
                        if (buildings == null)
                        {
                            Debug.Write("Buildings are null");
                            return;
                        }
                        Debug.Write("Buildings:");
                        if (buildings.Buildings != null)
                            foreach (var b in buildings.Buildings)
                            {
                                if (b != null && b.Building_name != null)
                                    BuildingMap.Add(b.Building_name, b.Building_id);
                            }
                        Debug.Write("Buildings:");
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            BuildingNamePicker.ItemsSource = BuildingMap.Keys.ToList();
                            Debug.Write("Buildings:");
                            BuildingNamePicker.SelectedIndexChanged += Reload;
                            Debug.Write("Buildings:");
                            AtTime.PropertyChanged += Reload;
                        });
                        
                    }
                    )
                    );
            task.Start();
        }

        /// <summary>
        /// EventHandler to hanlde changes in both <see cref="AtTime"/> and <see cref="BuildingNamePicker"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Reload(object sender, EventArgs e)
        {
            Debug.Write("Reloading");
            CurrentTime = AtTime.Time;
            var o = 0;
            if (BuildingNamePicker != null && BuildingNamePicker.SelectedItem != null)
                BuildingMap.TryGetValue((string)BuildingNamePicker.SelectedItem, out o);
            BuildingID = o;

            if (e is PropertyChangedEventArgs || sender is Picker)
                if ((sender is Picker) || (e as PropertyChangedEventArgs).PropertyName == "Time")
                {
                    var occs = await RestService.service.GetCurrentLotVolumes();
                    var map = await RestService.service.GetBestLots(BuildingMap[(string)BuildingNamePicker.SelectedItem], AtTime.Time.ToString());

                    Dictionary<double, int> invMap = new Dictionary<double, int>();

                    int smallest, medium, largest;

                    Debug.Write("Inverting map!");
                    foreach (var key in map.Keys)
                    {
                        invMap.Add(map[key], key);
                    }

                    var l = invMap.Keys.ToList();
                    l.Sort();
                   
                    if(l.Count > 0)
                    {
                        Debug.Write("Getting lot IDs!");
                        smallest = invMap[l[0]];
                        medium = invMap[l[(l.Count-1) / 2]];
                        largest = invMap[l[l.Count - 1]];
                    }
                    else
                    {
                        Debug.Write("There are not enough lots!");
                        return;
                    }

                    var l1 = await RestService.service.GetLotInfo(smallest);
                    var l2 = await RestService.service.GetLotInfo(medium);
                    var l3 = await RestService.service.GetLotInfo(largest);

                    Device.BeginInvokeOnMainThread(() =>
                     {
                         
                        Debug.Write("Reload\n");
                        Lot1.Text = l1.Lot_Name;
                        Lot2.Text = l2.Lot_Name;
                        Lot3.Text = l3.Lot_Name;
                    
                        Lot1.BackgroundColor = Utils.ColorMapper.MapPercentageToRGB(occs[l1.Id]);
                        Lot2.BackgroundColor = Utils.ColorMapper.MapPercentageToRGB(occs[l2.Id]);
                        Lot3.BackgroundColor = Utils.ColorMapper.MapPercentageToRGB(occs[l3.Id]);

                        Lot1.Clicked += async (object s, EventArgs ev) => { await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new LotInfo(l1.Id)); };
                        Lot2.Clicked += async (object s, EventArgs ev) => { await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new LotInfo(l2.Id)); };
                        Lot3.Clicked += async (object s, EventArgs ev) => { await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new LotInfo(l3.Id)); };
                     });

                }

        }
    }
}