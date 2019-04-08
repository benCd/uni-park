using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Diagnostics;
using up_mobile.Backend;
using up_mobile.Models;

namespace up_mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapMenu : Rg.Plugins.Popup.Pages.PopupPage
    {

        public MapMenu()
        {
            InitializeComponent();
            //HeightRequest = DeviceDisplay.MainDisplayInfo.Height / 18;
        }

        public async Task Populate()
        {
            Debug.Write("Entering Populate()");

            LotHolder lotholder = null;

            //Debug.Write("------------> CHECKPOINT 0");

            if (Application.Current.Properties.ContainsKey("UniversityLots"))
                lotholder = (LotHolder)Application.Current.Properties["UniversityLots"];

            //Debug.Write("------------> CHECKPOINT 1");

            if (lotholder == null)
            {
                Debug.Write("Lotholder == null!");
                lotholder = await RestService.service.GetMyUniLots();
            }

            foreach (ParkingLot lot in lotholder.Lots)
                Debug.Write("Lotholder swallowed: " + lot.Lot_Name + " with STD " + lot.Id);


                //Debug.Write("------------> CHECKPOINT 2");
                foreach (ParkingLot lot in lotholder.Lots)
                {
                Debug.Write("Adding Button for: " + lot.Lot_Name);
                Device.BeginInvokeOnMainThread(() =>
                {
                    Stack.Children.Add(new RememberButton(lot.Lot_Name, lot.Id));
                });
                }

            //Debug.Write("------------> CHECKPOINT 3");

            foreach (RememberButton button in Stack.Children)
                Debug.Write(button.Text + " :::: " + button.IDVal);
            
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
            public int IDVal { get; }

            public RememberButton(string label, int id) : base()
            {
                IDVal = id;
                Text = label;
                Clicked += async (sender, args) => MapContentPage.MoveToLot(IDVal);
                
                BackgroundColor = Color.Red;
            }
        }
    }
}