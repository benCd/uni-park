using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using up_mobile.Backend;
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
            FillCalendarSelection();
		}

        private async void FillCalendarSelection()
        {
            //TODO IMPLEMENT CALENDAR LOADING
            var calendars = await RestService.service.GetCalendars();

            Device.BeginInvokeOnMainThread(
                () =>
                {
                    CalendarList.Children.Clear();
                    foreach(var cal in calendars)
                    {
                        CalendarList.Children.Add(new RButton(cal.Cal_name, cal.Cal_id));
                    }
                }
                );
        }

        public async void RefreshHandler(object sender, EventArgs e)
        {
            FillCalendarSelection();
        }

        private class RButton : Button
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="label"></param>
            /// <param name="id"></param>
            public RButton(string label, string id)
            {
                Text = label;
                Clicked += async (object s, EventArgs e) => {
                    if (true/*await RestService.service.PostCalendar(id)*/)
                        await Navigation.PushAsync(new User());
                };
            }
        }

    }
}