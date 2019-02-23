﻿using System;
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
        /// When Next Page button on SundaySurvey page <see cref="SundaySurvey.xaml"/> 
        /// is pressed it thanks the user via a pop up, and navigates to Home page <see cref="Home.xaml"/>
        /// </summary>
        async void SubmitNewUserSurvey(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await DisplayAlert("Thanks!", "This information really helps us", "OK");
            await Navigation.PushAsync(new User());
        }
    }
}