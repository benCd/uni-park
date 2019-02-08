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
	public partial class NewUserSurvey : ContentPage
	{
        // Constructor for NewUserSurvey class - Initializes the page on load
        public NewUserSurvey ()
		{
			InitializeComponent ();
		}

        // NewUserSurveyClicked function - When the Begin Survey button is pressed it will navigate to MondaySurvey.xaml page
        async void NewUserSurveyClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new MondaySurvey());
        }
    }
}