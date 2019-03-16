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
    /// <summary>
    /// ParkedButton page - Where the user goes to tell the app they have parked
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParkedButton : ContentPage
    {
        /// <summary>
        /// Loads ParkedButton page <see cref="ParkedButton.xaml"/>
        /// </summary>
        public ParkedButton()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the I Parked button on <see cref="ParkedButton.xaml"/> is pressed
        /// navigates to the ParkedConfirm page <see cref="ParkedConfirm.xaml"/>
        /// and it locks the button from being pressed for 10 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void ParkedButtonPressed(object sender, EventArgs args)
        {
            // Disables the button
            ((Button)sender).IsEnabled = false;

            Background.TaskScheduler.ScheduleFunctionForExecution("EnableButton", 99, DateTime.Now.AddSeconds(10), 
                () => {
                    Debug.Write("Enabling Button START!");
                    Debug.Write(((Button)sender).Id);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        // Enables the button
                        ((Button)sender).IsEnabled = true;
                    });
                    Debug.Write("Enabling Button END!");
                return 0; }
                );

            try { 

            var pos = await GeoProvider.GetCurrentPositionAsync();

            if (pos == null)
            {//TODO implement spamming protection! see #40
                DisplayAlert("We could not get your location!", "We messed up, sorry about that! Please try again later!", "Alright, if I have to...");
                ((Button)sender).IsEnabled = true;
                Background.TaskScheduler.RemoveScheduledFunction("EnableButton");
            }
            else
            {
                Models.ParkingLot lot = await RestService.service.FindLot(pos.Latitude, pos.Longitude, pos.Accuracy);

                if (lot == null)
                {//TODO implement spamming protection! see #40
                    DisplayAlert("Oops, we hope you're not in the lake!", "You are not in a recognized parking lot, if this is a mistake, please try again!", "Gotcha!");
                    ((Button)sender).IsEnabled = true;
                    Background.TaskScheduler.RemoveScheduledFunction("EnableButton");
                }
                else
                {
                    //TODO Set map to current parking lot
                    await Navigation.PushAsync(new ParkedConfirm((Button)sender, pos, lot));
                }
            }
            }catch(Exception e)
            {
                Debug.Write(e.StackTrace);
            }
        }
    }
}