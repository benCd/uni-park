using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        // Constructor for Register class - Initializes the page on load
        public Register()
        {
            InitializeComponent();
        }

        // RegisterAttempt function - When the Register button in Register.xaml is clicked this function 
        // checks that the email and password meet the requirements for registration, then registers the user, (NOT DONE YET)
        // after which it navigates them to the ParkingSurvey.xaml page. (FOR NOW IT JUST GOES THERE WHEN YOU PRESS IT)
        async void RegisterAttempt(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new ParkingSurvey());
        }
    }
}