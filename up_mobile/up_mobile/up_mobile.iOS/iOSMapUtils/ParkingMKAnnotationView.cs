using CoreGraphics;
using MapKit;
using UIKit;

namespace up_mobile.iOS.iOSMapUtils
{
    internal class ParkingMKAnnotationView : MKAnnotationView
    {
        public string Id { get; set; }
        public string Text { get; set; }

        public ParkingMKAnnotationView(IMKAnnotation annotation, string id) : base(annotation, id)
        {
            //TODO insert pin image
            //Image = UIImage.FromFile("pin.png");
            CalloutOffset = new CGPoint(0, 0);
            //TODO insert user image
            //LeftCalloutAccessoryView = new UIImageView(UIImage.FromFile("monkey.png"));
            RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);
        }
    }
}