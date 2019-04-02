﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Backend;
using up_mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LotInfo : Rg.Plugins.Popup.Pages.PopupPage
    {
        /// <summary>
        /// Holds LotId
        /// </summary>
        private int LotId;
        /// <summary>
        /// Holds the Lot
        /// </summary>
        private ParkingLot Lot;

        public LotInfo() { InitializeComponent(); }

        /// <summary>
        /// Represents a popup window to display info for one lot
        /// </summary>
        public LotInfo(int lotid)
        {
            //Remembering the ID for the current lot
            LotId = lotid;
            Lot = null;

            //Intialising the XAML
            InitializeComponent();

            //Async loading components
            LoadComponents();
        }

        /// <summary>
        /// Loads components for the LotInfo page
        /// </summary>
        /// <returns>nn</returns>
        private async Task LoadComponents()
        {
            //Lot = await RestService.service.
            var parkingLot = RestService.service.GetLotInfo(LotId);

        }

        /// <summary>
        /// Triggered when the navigation button is pressed in <see cref="LotInfo"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnNavigateButtonClicked(object sender, EventArgs e)
        {
            var request = string.Format("http://maps.google.com/?daddr=" + Lot.Center_Lat + "," + Lot.Center_Long + "");
            Device.OpenUri(new Uri(request));
        }
    }
}