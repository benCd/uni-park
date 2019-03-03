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

        /// <summary>
        /// When the Begin Survey button on NewUserSurvey page 
        /// <see cref="NewUserSurvey.xaml"/> is pressed it navigates 
        /// to the MondaySurvey page <see cref="MondaySurvey.xaml"/>
        /// </summary>
        async void NewUserSurveyClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new MondaySurvey());
        }
    }
}