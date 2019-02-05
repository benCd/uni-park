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
	public partial class TuesdaySurvey : ContentPage
	{
        // Constructor for TuesdaySurvey class - Initializes the page on load
        public TuesdaySurvey ()
		{
			InitializeComponent ();
		}

        // WednesdaySurveyPage function - takes the user to the Wednesday page of the New User Survey
        async void WednesdaySurveyPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new WednesdaySurvey());
        }
    }
}