using Xamarin.Forms.Maps;

namespace up_mobile.Map.Utils
{
    public class ParkingPin : Pin
    {
        /// <summary>
        /// The text for pin
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// DB id for the pin
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// Upvotes on this pin
        /// </summary>
        public int Upvotes { get; set; }

        /// <summary>
        /// Downvotes on this pin
        /// </summary>
        public int Downvotes { get; set; }

        /// <summary>
        /// User id for the owner of this pin
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// The reported volume connected to this pin
        /// </summary>
        public int Volume { get; set; }

        /// <summary>
        /// Constructor for an empty <see cref="ParkingPin"/>
        /// </summary>
        public ParkingPin() : base()
        {

        }

        /// <summary>
        /// Updates a <see cref="ParkingPin"/> with the information from a <see cref="Models.ParkingPin"/>
        /// 
        /// NOTE: The passed <see cref="Models.ParkingPin"/> must have the same ID as the <see cref="ParkingPin"/>
        /// </summary>
        /// <param name="pin">Pin for the new data</param>
        public void UpdateWith(Models.ParkingPin pin)
        {
            if (ID == pin.Id)
            {
                Upvotes = pin.Upvotes;
                Downvotes = pin.Downvotes;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is ParkingPin && ((ParkingPin)obj).ID == this.ID;
        }

    }
}
