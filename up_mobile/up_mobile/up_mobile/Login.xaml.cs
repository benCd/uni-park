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
    public partial class Login : ContentPage
    {
        // Constructor for Login class - Initializes the page on load
        public Login()
        {
            InitializeComponent();
        }

        // LoginAttempt function - When the Login button in Login.xaml is clicked this function 
        // attempts to log the user in. If it is their first time logging in, it takes them to the new user survey 
        // FOR NOW IT JUST GOES THERE WHEN YOU PRESS LOGIN
        async void LoginAttempt(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new ParkingSurvey());
        }
    }
}