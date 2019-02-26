using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <summary>
        /// Loads the ParkedConfirm page <see cref="ParkedConfirm.xaml"/>
        /// </summary>
		public ParkedConfirm ()
		{
			InitializeComponent ();
		}

        /// <summary>
        /// When Confirm button on <see cref="ParkedConfirm.xaml"/> is pressed
        /// it sends the information to the server and locks the I Parked button
        /// on <see cref="ParkedButton"/> page for 10 minutes, then navigates back to Home <see cref="Home.xaml"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void ConfirmPressed(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new User());
        }

        /// <summary>
        /// When the Deny button on <see cref="ParkedConfirm.xaml"/> is pressed
        /// it navigates back to <see cref="ParkedButton.xaml"/> and unlocks the I Parked button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void DenyPressed(object sender, EventArgs args)
        {
            Button button = (Button)sender;
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