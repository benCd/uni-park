using System;
using System.Collections.Generic;
using System.Text;

namespace up_mobile.Models
{
    public class Calendar
    {
        public string Cal_name { get; set; }
        public string Cal_id { get; set; }
    }

    public class CalendarHolder
    {
        public Calendar[] Calendars { get; set; }
    }
}
