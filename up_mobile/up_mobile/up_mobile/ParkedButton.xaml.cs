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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void ParkedButtonPressed(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new ParkedConfirm());

            // Sets the button to be disabled
            button.IsEnabled = false;

            // Stop watch for use in unlocking the button, unlocks after 10,000 ms (10 seconds)
            System.Diagnostics.Stopwatch stopper = System.Diagnostics.Stopwatch.StartNew();

            if (stopper.ElapsedMilliseconds >= 10000) //600,000 for 10 minutes
            {
                
                button.IsEnabled = true;
                stopper.Stop();
            }
        }
    }
}