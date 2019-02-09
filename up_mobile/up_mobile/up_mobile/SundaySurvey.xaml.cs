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
    /// SundaySurvey page
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SundaySurvey : ContentPage
	{
        /// <summary>
        /// Loads SundaySurvey page
        /// </summary>
        public SundaySurvey ()
		{
			InitializeComponent ();
		}

        /// <summary>
        /// When the Next Page button on SundaySurvey page <see cref="SundaySurvey.xaml"/> is pressed it 
        /// will navigate to the FinishedSurvey page <see cref="FinishedSurvey.xaml"/>
        /// </summary>
        async void SubmitNewUserSurvey(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new FinishedSurvey());
        }
    }
}