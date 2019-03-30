using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Backend;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PinInfo : Rg.Plugins.Popup.Pages.PopupPage
    {
        private Map.Utils.ParkingPin Pin;

		public PinInfo (Map.Utils.ParkingPin pin)
		{
            Pin = pin;
			InitializeComponent();

            Header.Text = Pin.OwnerId.ToString(); //TODO Change to user name
            Volume.Text = Pin.Volume.ToString() + "%";
            Upvotes.Text = Pin.Upvotes.ToString();
            Downvotes.Text = Pin.Downvotes.ToString();
		}

        public async void Upvote(object sender, EventArgs e)
        {
            if (await RestService.service.VoteOnPin(Pin.ID, 1))
            {
                //TODO Get pin info and refresh
                Debug.Write("Upvoted Pin " + Pin.ID);
            }
        }

        public async void Downvote(object sender, EventArgs e)
        {
            if (await RestService.service.VoteOnPin(Pin.ID, -1))
            {
                //TODO Get pin info and refresh
                Debug.Write("Downvoted Pin " + Pin.ID);
            }
        }

    }
}