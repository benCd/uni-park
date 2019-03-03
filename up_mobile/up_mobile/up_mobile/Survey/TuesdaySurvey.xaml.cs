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
    /// TuesdaySurvey page
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TuesdaySurvey : ContentPage
	{
        /// <summary>
        /// Loads TuesdaySurvey page
        /// </summary>
        public TuesdaySurvey ()
		{
			InitializeComponent ();
		}

        /// <summary>
        /// When the Next Page button on TuesdaySurvey page 
        /// <see cref="TuesdaySurvey.xaml"/> is pressed it navigates
        /// to the WednesdaySurvey page <see cref="WednesdaySurvey.xaml"/>
        /// </summary>
        async void WednesdaySurveyPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new WednesdaySurvey());
        }
    }
}