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
	public partial class SaturdaySurvey : ContentPage
	{
        // Constructor for SaturdaySurvey class - Initializes the page on load
        public SaturdaySurvey ()
		{
			InitializeComponent ();
		}

        // SundaySurveyPage function - takes the user to the Sunday page of the New User Survey
        async void SundaySurveyPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new SundaySurvey());
        }
    }
}