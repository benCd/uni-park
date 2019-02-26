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
        }
    }
}