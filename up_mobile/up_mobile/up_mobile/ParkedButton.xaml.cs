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

        async void ParkedButtonTimer()
        {
            await Task.Delay(2000);
        }

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
        /// It locks the button from being pressed for 10 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void ParkedButtonPressed(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new ParkedConfirm());

            // Disables the button
            button.IsEnabled = false;
             
            // 10 seconds (in milliseconds)
            await Task.Delay(10000);

            // Enables the button
            button.IsEnabled = true;

        }
    }
}