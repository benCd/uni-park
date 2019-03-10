using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Map.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Diagnostics;
using up_mobile.Backend;
using up_mobile.Models;

namespace up_mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapMenu : ScrollView
    {
        /// <summary>
        /// 
        /// </summary>
        StackLayout Stack;

        public MapMenu()
        {
            Orientation = ScrollOrientation.Horizontal;
            HeightRequest = DeviceDisplay.MainDisplayInfo.Height / 18;
            BackgroundColor = Color.Accent;

            Stack = new StackLayout();
            Stack.Orientation = StackOrientation.Horizontal;
            Populate().ContinueWith(
                t => {
                    Content = Stack;
                    Debug.Write("Done constructing MapMenu"); }
                );
            
        }

        private async Task Populate()
        {
            Debug.Write("Entering Populate()");

            var lotholder = await RestService.service.GetMyUniLots();

            Debug.Write(lotholder.Lots);

            foreach (ParkingLot lot in lotholder.Lots)
            {
                Debug.Write(lot);
                Stack.Children.Add(new RememberButton(lot.Lot_Name, lot.Id));
            }

            Debug.Write("Exiting Populate()");
        }

        /// <summary>
        /// Button that keeps the necesssary information to switch maps in <see cref="MapContentPage"/> using <see cref="MapContentPage.MoveToLot(int)"/>
        /// </summary>
        private class RememberButton : Button
        {
            /// <summary>
            /// Gets the ID set to this button.
            /// </summary>
            public int IDval { get; }

            public RememberButton(string label, int idval) : base()
            {
                IDval = idval;
                Text = label;
                Clicked += async (sender, args) => MapContentPage.MoveToLot(IDval);

                BackgroundColor = Color.Accent;

            }
            
        }
    }
}