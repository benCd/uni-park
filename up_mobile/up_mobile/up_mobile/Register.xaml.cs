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
        // checks that the email and password meet the requirements for registration, then registers the user
        // while sending them a confirmation e-mail, then it redirects them to the Login page
        // FOR NOW IT JUST GOES TO THE LOGIN PAGE
        async void RegisterAttempt(object sender, EventArgs args)
        {
            Button button = (Button)sender;

            var RegEmail = RegisterEmail.Text;
            var RegPassword = RegisterPassword.Text;

            await Navigation.PushAsync(new Login());
        }
    }
}