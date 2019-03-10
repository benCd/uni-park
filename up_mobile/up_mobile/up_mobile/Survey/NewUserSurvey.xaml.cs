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
    /// NewUserSurvey page - Intermediary page shown before the user takes their survey
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewUserSurvey : ContentPage
	{
        /// <summary>
        /// Loads the NewUserSurvey page <see cref="NewUserSurvey.xaml"/>
        /// </summary>
        public NewUserSurvey ()
		{
			InitializeComponent ();

            // Suppresses the back button at the top of the page
            NavigationPage.SetHasBackButton(this, false);
        }

        /// <summary>
        /// Override for OnBackButtonPressed - Preventing Android Hardware Back Button from 
        /// going back to the Login page <see cref="Login.xaml"/> after logging in successfully 
        /// and being prompted with the survey
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        // Queue to hold the selected days for the new user survey
        Queue<string> SurveyNavigationQueue = new Queue<string>();

        /// <summary>
        /// When the Begin Survey button on NewUserSurvey page 
        /// <see cref="NewUserSurvey.xaml"/> is pressed it navigates 
        /// to the MondaySurvey page <see cref="MondaySurvey.xaml"/>
        /// </summary>
        async void NewUserSurveyClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;

            if (this.MondayToggle.On == true)
            {
                SurveyNavigationQueue.Enqueue("Monday");
            }
            else if (this.TuesdayToggle.On == true)
            {
                SurveyNavigationQueue.Enqueue("Tuesday");
            }
            else if (this.WednesdayToggle.On == true)
            {
                SurveyNavigationQueue.Enqueue("Wednesday");
            }
            else if (this.ThursdayToggle.On == true)
            {
                SurveyNavigationQueue.Enqueue("Thursday");
            }
            else if (this.FridayToggle.On == true)
            {
                SurveyNavigationQueue.Enqueue("Friday");
            }
            else if (this.SaturdayToggle.On == true)
            {
                SurveyNavigationQueue.Enqueue("Saturday");
            }
            else if (this.SundayToggle.On == true)
            {
                SurveyNavigationQueue.Enqueue("Sunday");
            }

            //await Navigation.PushAsync(new SurveyPage());
        }
    }
}