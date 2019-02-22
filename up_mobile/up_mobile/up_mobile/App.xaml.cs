using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace up_mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //need to make if statement based on if logged in or not to pick which page to start the app with for each user
            //User is for logged in users of our app
            //Guest is for non logged in users of our app

            //CURRENTLY IT IS JUST SET TO GUEST

            MainPage = new NavigationPage(new Guest());

            //MainPage = new NavigationPage(new User());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

