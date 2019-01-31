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
	public partial class ParkingSurvey : ContentPage
	{
        // Constructor for ParkingSurvey class - Initializes the page on load
        public ParkingSurvey ()
		{
			InitializeComponent ();
        }

        //  void Submission(object sender, EventArgs args)
        //  {
        //     
        //  }

        // SwitchToggled function - Logic that will generate fields for day & time data when a switch
        // is toggled in ParkingSurvey.xaml SurveyGrid field
        void SwitchToggled(object sender, ToggledEventArgs e)
        {
           // ParkingSurvey.SurveyGrid.P1label.IsVisible = true;
        }

    }
}