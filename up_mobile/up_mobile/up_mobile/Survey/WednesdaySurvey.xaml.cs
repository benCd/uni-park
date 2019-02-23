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
    /// WednesdaySurvey page
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WednesdaySurvey : ContentPage
	{
        /// <summary>
        /// Loads WednesdaySurvey page
        /// </summary>
        public WednesdaySurvey ()
		{
			InitializeComponent ();
		}

        /// <summary>
        /// When the Next Page button on WednesdaySurvey page 
        /// <see cref="WednesdaySurvey.xaml"/> is pressed it navigates
        /// to the ThursdaySurvey page <see cref="ThursdaySurvey.xaml"/>
        /// </summary>
        async void ThursdaySurveyPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new ThursdaySurvey());
        }
    }
}