using System;
using System.Collections.Generic;
using CoreGraphics;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;
using up_mobile.Map.Utils;

[assembly: ExportRenderer(typeof(up_mobile.Map.Utils.LotMap), typeof(up_mobile.iOS.iOSMapUtils.iOSCustomMapRenderer))]
namespace up_mobile.iOS.iOSMapUtils
{
    /// <summary>
    /// Custom <see cref="Xamarin.Forms.Maps.Map"/> Renderer (<seealso cref="MapRenderer"/>).
    /// This class provides functionality for custom pins as well as custom map drawings such 
    /// as shapes and a heatmap.
    /// </summary>
    public class iOSCustomMapRenderer : MapRenderer
    {
        UIView pinView;
        List<ParkingPin> parkingPins;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if(e.OldElement != null)
            {
                var nativeMap = Control as MKMapView;
                if(nativeMap != null)
                {
                    nativeMap.RemoveAnnotations(nativeMap.Annotations);
                    nativeMap.GetViewForAnnotation = null;
                    nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
                    nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                    nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
                }
            }


            if (e.NewElement != null)
            {
                var formsMap = (LotMap)e.NewElement;
                var nativeMap = Control as MKMapView;
                parkingPins = formsMap.ParkingPins;

                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
                nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
            }
        }

        private void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
        {
            throw new NotImplementedException();
        }


        protected override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
                return null;

            var parkingPin = GetParkingPin(annotation as MKPointAnnotation);
            if (parkingPin == null)
            {
                throw new Exception("Custom pin not found");
            }

            annotationView = mapView.DequeueReusableAnnotation(parkingPin.Id.ToString());
            if (annotationView == null)
            {
                annotationView = new ParkingMKAnnotationView(annotation, parkingPin.Id.ToString());
                
                ((ParkingMKAnnotationView)annotationView).Id = parkingPin.Id.ToString();
                ((ParkingMKAnnotationView)annotationView).Text = parkingPin.Text;
            }
            annotationView.CanShowCallout = true;

            return annotationView;
        }

        private ParkingPin GetParkingPin(MKPointAnnotation mKPointAnnotation)
        {
            throw new NotImplementedException();
        }
    }
}