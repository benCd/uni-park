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
        public Queue<String> SurveyNavigationQueue = new Queue<String>();

        /// <summary>
        /// List that holds the New User Survey data, see SurveyData class and constructor at bottom of <see cref="NewUserSurvey.xaml.cs"/>
        /// </summary>
        List<SurveyData> SubmissionData = new List<SurveyData>();

        /// <summary>
        /// Adds all days which are toggled on to the SurveyNavigationQueue <see cref="NewUserSurvey.xaml.cs"/>
        /// and when the Begin Survey button on NewUserSurvey page <see cref="NewUserSurvey.xaml"/> is pressed it 
        /// navigates to <see cref="SurveyPage.xaml"/>
        /// </summary>
        async void NewUserSurveyClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;

            /// <remarks>
            /// This Clear operation is needed to avoid mix up of page titles if the 
            /// user were to navigate back to this page and alter the selected days.
            /// </remarks>
            SurveyNavigationQueue.Clear();

            // Only allows them to navigate to the next page if at least 1 day is selected
            if (this.MondayToggle.On == true || this.TuesdayToggle.On == true || this.WednesdayToggle.On == true || this.ThursdayToggle.On == true || this.FridayToggle.On == true || this.SaturdayToggle.On == true || this.SundayToggle.On == true)
            {
                if (this.MondayToggle.On == true)
                {
                    SurveyNavigationQueue.Enqueue("Monday");
                }
                if (this.TuesdayToggle.On == true)
                {
                    SurveyNavigationQueue.Enqueue("Tuesday");
                }
                if (this.WednesdayToggle.On == true)
                {
                    SurveyNavigationQueue.Enqueue("Wednesday");
                }
                if (this.ThursdayToggle.On == true)
                {
                    SurveyNavigationQueue.Enqueue("Thursday");
                }
                if (this.FridayToggle.On == true)
                {
                    SurveyNavigationQueue.Enqueue("Friday");
                }
                if (this.SaturdayToggle.On == true)
                {
                    SurveyNavigationQueue.Enqueue("Saturday");
                }
                if (this.SundayToggle.On == true)
                {
                    SurveyNavigationQueue.Enqueue("Sunday");
                }

                await Navigation.PushAsync(new SurveyPage(SurveyNavigationQueue, SubmissionData));
            }
            else
            {
                await DisplayAlert("You have not selected any days", "Please select one", "OK");
            }        
        }
    }

    /// <summary>
    /// Container to hold all of the New User Survey data
    /// </summary>
    public class SurveyData
    {
        public string Day { get; set; }
        public string ParkingLot { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public double StartVolume { get; set; }
        public double EndVolume { get; set; }
        public SurveyData(string day, string parkinglot, string starttime, string endtime, double startvolume, double endvolume)
        {
            Day = day;
            ParkingLot = parkinglot;
            StartTime = starttime;
            EndTime = endtime;
            StartVolume = startvolume;
            EndVolume = endvolume;
        }
    }
}