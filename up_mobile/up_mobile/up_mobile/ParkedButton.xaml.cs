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

            
            //TODO Set map to current parking lot
            await Navigation.PushAsync(new ParkedConfirm((Button)sender));
        }
    }
}