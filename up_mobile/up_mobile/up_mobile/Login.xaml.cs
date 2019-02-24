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
    /// Login page
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        /// <summary>
        /// Loads the Login page <see cref="Login.xaml"/>
        /// </summary>
        public Login()
        {
            InitializeComponent();
        }
        /// <summary>
        /// When Login button on the Login page <see cref="Login.xaml"/> is pressed
        /// it attempts to log the user in. If it is their first time logging in they 
        /// are redirected to the New User Survey <see cref="NewUserSurvey.xaml"/>
        /// 
        /// FOR NOW IT JUST GOES TO THE NEW USER SURVEY PAGE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void LoginAttempt(object sender, EventArgs args)
        {
            Button button = (Button)sender;

            var LogEmail = LoginEmail.Text;
            var LogPassword = LoginPassword.Text;

            //await Navigation.PushAsync(new NewUserSurvey());
            
            await Navigation.PushAsync(new Map.MapContentPage(0));
        }
    }
}