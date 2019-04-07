using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace up_mobile.Calendar
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalendarSelection : ContentPage 
	{
		public CalendarSelection ()
		{
			InitializeComponent ();
		}

        private async void FillCalendarSelection()
        {
            //TODO IMPLEMENT CALENDAR LOADING
            var calendars = new List<(int,string)>();

            //foreach (var cal in calendars)

        }

	}
}