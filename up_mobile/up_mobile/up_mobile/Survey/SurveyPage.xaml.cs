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
    /// SurveyPage page
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SurveyPage : ContentPage
	{
        Queue<String> SurveyNavigationQueue = new Queue<String>();

        /// <summary>
        /// Loads SurveyPage  page
        /// </summary>
		public SurveyPage (Queue<String> q)
		{
            SurveyNavigationQueue = q;
            this.Title = SurveyNavigationQueue.Dequeue();
			InitializeComponent ();
		}

        /// <summary>
        /// When the Next Page button on SurveyPage page 
        /// <see cref="SurveyPage.xaml"/> is pressed it navigates 
        /// to the next version of the page
        /// </summary>
        async void NextSurveyPageButtonClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new SurveyPage(SurveyNavigationQueue));
        }
    }
}