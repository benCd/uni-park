using System;

namespace up_mobile.Models
{
    public class ParkingReport //represents one row in lotdata
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Lot_id { get; set; }
        public DateTime Date_time { get; set; }
        public float volume { get; set; }
    }
}
