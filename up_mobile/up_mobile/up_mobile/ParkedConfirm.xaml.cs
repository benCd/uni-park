using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Button PButton;

        /// <summary>
        /// Loads the ParkedConfirm page <see cref="ParkedConfirm.xaml"/>
        /// </summary>
        /// <param name="button"></param> Button from ParkedButton page
		public ParkedConfirm (Button button)
		{
            PButton = button;
			InitializeComponent ();
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

            await Navigation.PopAsync();
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
            ParkedDisplayLabel.Text = String.Format("Lot Fullness is {0} %", value);
        }

    }
}