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
	public partial class MondaySurvey : ContentPage
	{
        // Constructor for MondaySurvey class - Initializes the page on load
        public MondaySurvey ()
		{
			InitializeComponent ();
		}

        // TuesdaySurveyPage function - takes the user to the Tuesday page of the New User Survey
        async void TuesdaySurveyPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new TuesdaySurvey());
        }
    }
}