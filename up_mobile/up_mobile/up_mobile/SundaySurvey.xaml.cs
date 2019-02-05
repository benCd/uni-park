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
	public partial class SundaySurvey : ContentPage
	{
        // Constructor for SundaySurvey class - Initializes the page on load
        public SundaySurvey ()
		{
			InitializeComponent ();
		}

        // SubmitNewUserSurvey function - Submits the user's New User Survey and navigates them to ???
        //async void SubmitNewUserSurvey(object sender, EventArgs args)
        //{
        //    Button button = (Button)sender;
        //    await Navigation.PushAsync(new DestinationPage());
        //}
    }
}