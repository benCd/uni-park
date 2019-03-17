using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile
{
    /// <summary>
    /// SurveyPage page
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SurveyPage : ContentPage
	{
        /// <summary>
        /// Queue to hold the selected days for the new user survey which is
        /// passed over from a Queue of the same name in <see cref="NewUserSurvey.xaml.cs"/>
        /// </summary>
        Queue<String> SurveyNavigationQueue = new Queue<String>();

        /// <summary>
        /// List to hold the Survey Data (see <see cref="NewUserSurvey.xaml.cs"/> for constructor)
        /// </summary>
        List<SurveyData> SubmissionData = new List<SurveyData>();

        /// <summary>
        /// Loads SurveyPage  page
        /// </summary>
		public SurveyPage (Queue<String> q, List<SurveyData> s)
		{
            SurveyNavigationQueue = q;
            SubmissionData = s;

            /// <remarks>
            /// If something is still remaining in the Queue it sets the next thing 
            /// as this page <see cref="SurveyPage.xaml"/>'s Title
            /// </remarks>
            if (SurveyNavigationQueue.Count() > 0)
            {
                this.Title = SurveyNavigationQueue.Peek();
            }

			InitializeComponent ();
		}

        /// <summary>
        /// When the Submit button on SurveyPage page 
        /// <see cref="SurveyPage.xaml"/> is pressed it navigates 
        /// to the next version of the page. If the Queue is empty it navigates
        /// to the User page <see cref="User.xaml"/>
        /// </summary>
        async void NextSurveyPageButtonClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;

            if (SurveyNavigationQueue.Count() > 0)
            {
                // Adding the data submitted to the List of SurveyData objects
                SubmissionData.Add(new SurveyData(SurveyNavigationQueue.Peek(), LotSelection.SelectedItem.ToString(), StartTime.Time.ToString(), EndTime.Time.ToString()));

                // Removing the day just handled from the Queue
                SurveyNavigationQueue.Dequeue();

                await Navigation.PushAsync(new SurveyPage(SurveyNavigationQueue, SubmissionData));
            }
            if (SurveyNavigationQueue.Count() == 0)
            {
                // Pop up to thank the user for taking the survey
                await DisplayAlert("Thanks!", "This information really helps us", "OK");

                /// <remarks>
                /// Storing this value in <see cref="Settings.cs"/> so it does not give them the survey for future log ins
                /// </remarks>
                Helpers.Settings.TookNewUserSurvey = true;

                // For loop to send the data in the list of SurveyData objects to the database
                for (int i = 0; i == SubmissionData.Count(); i++)
                {
                    //Debug statements to make sure the data is being put into the SurveyData List correctly
                    Debug.WriteLine("Day: " + SubmissionData[i].Day);
                    Debug.WriteLine("Lot: " + SubmissionData[i].ParkingLot);
                    Debug.WriteLine("StartTime: " + SubmissionData[i].StartTime);
                    Debug.WriteLine("EndTime: " + SubmissionData[i].EndTime);
                }
               
                await Navigation.PushAsync(new User());
            }
        }

        /// <summary>
        /// When the Slider is moved on <see cref="SurveyPage.xaml"/> it updates what
        /// is shown on the page just above the slider, letting users know what value
        /// they are reporting for Lot fullness
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void StartSliderUpdated(object sender, ValueChangedEventArgs args)
        {
            int value = (int)args.NewValue;
            StartDisplayLabel.Text = String.Format("Lot Fullness Upon Arrival was {0} %", value);
        }

        void EndSliderUpdated(object sender, ValueChangedEventArgs args)
        {
            int value = (int)args.NewValue;
            EndDisplayLabel.Text = String.Format("Lot Fullness Upon Leaving was {0} %", value);
        }
    }
}