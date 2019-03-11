using Xamarin.Forms.Maps;

namespace up_mobile.Map.Utils
{
    public class ParkingPin : Pin
    {
        public string Text { get; set; }
        public int ID { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }

        public ParkingPin() : base()
        {

        }

    }
}
