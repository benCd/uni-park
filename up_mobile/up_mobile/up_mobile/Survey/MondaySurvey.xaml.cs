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
    /// MondaySurvey page
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MondaySurvey : ContentPage
	{
        /// <summary>
        /// Loads MondaySurvey page
        /// </summary>
        public MondaySurvey ()
		{
			InitializeComponent ();
		}

        /// <summary>
        /// When the Next Page button on MondaySurvey page 
        /// <see cref="MondaySurvey.xaml"/> is pressed it navigates 
        /// to the TuesdaySurvey page <see cref="TuesdaySurvey.xaml"/>
        /// </summary>
        async void TuesdaySurveyPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new TuesdaySurvey());
        }
    }
}