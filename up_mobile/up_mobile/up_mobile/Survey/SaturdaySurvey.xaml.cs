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
    /// SaturdaySurvey page
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SaturdaySurvey : ContentPage
	{
        /// <summary>
        /// Loads SaturdaySurvey page
        /// </summary>
        public SaturdaySurvey ()
		{
			InitializeComponent ();
		}

        /// <summary>
        /// When the Next Page button on SaturdaySurvey page <see cref="SaturdaySurvey.xaml"/> is pressed it 
        /// will navigate to the SundaySurvey page <see cref="SundaySurvey.xaml"/>
        /// </summary>
        async void SundaySurveyPage(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new SundaySurvey());
        }
    }
}