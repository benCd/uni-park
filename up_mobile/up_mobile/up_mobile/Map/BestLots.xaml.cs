using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //private static List<Models.Building> Buildings;

        /// <summary>
        /// Constructor for the BestLots popup
        /// </summary>
        public BestLots()
        {
            InitializeComponent();
            //BuildingNamePicker.ItemsSource = new List<String>(); TODO use rest service here
            BuildingNamePicker.Items.Add("hi");
            BuildingNamePicker.Items.Add("ho");
        }

        /// <summary>
        /// EventHandler to hanlde changes in both <see cref="AtTime"/> and <see cref="BuildingNamePicker"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Reload(object sender, EventArgs e)
        {
            if(e is PropertyChangedEventArgs || sender is Picker)
                if((sender is Picker) || (e as PropertyChangedEventArgs).PropertyName == "Time"  )
                {
                    Debug.Write("Reload\n");
                    Lot1.Text = "Lot1";
                    Lot2.Text = "Lot2";
                    Lot3.Text = "Lot3";

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Lot1.BackgroundColor = Utils.ColorMapper.MapPercentageToRGB(0.5);
                        Lot2.BackgroundColor = Utils.ColorMapper.MapPercentageToRGB(0.9);
                        Lot3.BackgroundColor = Utils.ColorMapper.MapPercentageToRGB(0.2);
                    });

                    Lot1.Clicked += async (object s, EventArgs ev) => { await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new LotInfo()); };
                    Lot2.Clicked += async (object s, EventArgs ev) => { await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new LotInfo()); };
                    Lot3.Clicked += async (object s, EventArgs ev) => { await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new LotInfo()); };
                }
            //CurrentTime = AtTime.Time;
            //BuildingID = BuildingNamePicker.SelectedIndex;
        }
    }
}