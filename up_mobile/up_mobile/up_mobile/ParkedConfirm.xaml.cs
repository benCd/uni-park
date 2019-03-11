using Plugin.Geolocator.Abstractions;
using System;
using System.Diagnostics;
using up_mobile.Backend;
using up_mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile
{
    /// <summary>
    /// ParkedConfirm page - Where the user confirms that they parked after 
    /// pressing I Parked button on <see cref="ParkedButton.xaml"/> page
    /// and the app uses GPS to detect which lot they are in
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ParkedConfirm : ContentPage
	{
        private Button PButton;
        private Position Position;
        private ParkingLot Lot;

        /// <summary>
        /// Loads the ParkedConfirm page <see cref="ParkedConfirm.xaml"/>
        /// </summary>
        /// <param name="button"></param> Button from ParkedButton page
		public ParkedConfirm (Button button, Position pos, ParkingLot lot)
		{
            InitializeComponent ();
            Position = pos;
            Label.Text = "You parked in " + lot.Lot_Name;
            Lot = lot;
            PButton = button;
		}

        /// <summary>
        /// When Confirm button on <see cref="ParkedConfirm.xaml"/> is pressed
        /// it sends the information to the server and locks the I Parked button
        /// on <see cref="ParkedButton"/> page for 10 minutes, then navigates back
        /// one page to the User Tabbed Page <see cref="User.xaml"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void ConfirmPressed(object sender, EventArgs args)
        {
            try { 

            /// <remarks>
            /// Removes Task scheduled in <see cref="ParkedButton.xaml.cs"/> when the button
            /// on <see cref="ParkedButton.xaml"/> is pressed
            /// </remarks>
            Background.TaskScheduler.RemoveScheduledFunction("EnableButton");

            /// <remarks>
            /// Disables the button on <see cref="ParkedButton.xaml"/> page
            /// </remarks>
            PButton.IsEnabled = false;

            Background.TaskScheduler.ScheduleFunctionForExecution("EnableButton5Min", 99, DateTime.Now.AddMinutes(5),
                () => {
                    Debug.Write("Enabling Button START!");
                    
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        /// <remarks>
                        /// Enables the button on <see cref="ParkedButton.xaml"/> page
                        /// </remarks>
                        PButton.IsEnabled = true;
                    });
                    Debug.Write("Enabling Button END!");
                    return 0;
                }
                );

            Debug.Write("PIN WAS SENT AND RECEIVED??? --> "+ await RestService.service.PostNewPinAsync(Position.Longitude, Position.Latitude, Lot.Id, (float)(VolumeSlider.Value)/100.00));
            await MapContentPage.UpdatePins();
            await Navigation.PopAsync();
            }catch(NullReferenceException e)
            {
                Debug.Write(e.StackTrace);
            }
        }

        /// <summary>
        /// When the Deny button on <see cref="ParkedConfirm.xaml"/> is pressed
        /// it navigates back to <see cref="ParkedButton.xaml"/> and unlocks the I Parked button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void DenyPressed(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }

        /// <summary>
        /// When the Slider is moved on <see cref="ParkedConfirm"/> it updates what
        /// is shown on the page just above the slider, letting users know what value
        /// they are reporting for Lot fullness
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void SliderUpdated(object sender, ValueChangedEventArgs args)
        {
            int value = (int) args.NewValue;
            displayLabel.Text = String.Format("Lot Fullness is {0} %", value);
        }

    }
}