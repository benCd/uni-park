using System;
using System.Collections.Generic;
using Android.Content;
using Android.Gms.Maps;
using Android.Widget;
using up_mobile.Droid.DroidMapUtils;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using up_mobile.Map.Utils;
using Android.Gms.Maps.Model;
using up_mobile.Backend;
using Rg.Plugins.Popup.Services;

[assembly: ExportRenderer(typeof(LotMap), typeof(DroidCustomMapRenderer))]
namespace up_mobile.Droid.DroidMapUtils
{
    /// <summary>
    /// <see cref="MapRenderer"/> for Android platform
    /// </summary>
    public class DroidCustomMapRenderer : MapRenderer//, GoogleMap.IInfoWindowAdapter
    {
        List<ParkingPin> parkingPins;

        GoogleMap googleMap;

        LotMap formsMap;

        public DroidCustomMapRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.Maps.Map> e)
        {
            System.Diagnostics.Debug.Write("Calling OnElementChanged in DroidCustomMapRenderer");

            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                //NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                formsMap = (LotMap)e.NewElement;
                parkingPins = formsMap.ParkingPins;
                Control.GetMapAsync(this);
            }
            System.Diagnostics.Debug.Write("Exiting OnElementChanged in DroidCustomMapRenderer");

        }
        
        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);


            googleMap = map;

            googleMap.MarkerClick += OnMarkerClicked;

            System.Diagnostics.Debug.Write("Calling OnMapReady in DroidCustomMapRenderer");

            System.Diagnostics.Debug.Write("Exiting OnMapReady in DroidCustomMapRenderer");
        }

        /// <summary>
        /// Creates <see cref="MarkerOptions"/> for in <see cref="GoogleMap"/> displayed <see cref="Android.Gms.Maps.Model.Marker"/>
        /// </summary>
        /// <param name="pin">The pin that the marker is based on</param>
        /// <returns><see cref="MarkerOptions"/> for the passed <see cref="Pin"/></returns>
        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            System.Diagnostics.Debug.Write("Making pin!");
            var pp = pin as ParkingPin;
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pp.Text);
            marker.SetSnippet(pp.Label);
            marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
            return marker;
        }

        /// <summary>
        /// Handles the <see cref="GoogleMap.MarkerClick"/> event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        private async void OnMarkerClicked(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new PinInfo(GetParkingPin(e.Marker)));
            e.Handled = true;
        }

        /// <summary>
        /// Finds the corresponding <see cref="ParkingPin"/> for a <see cref="Android.Gms.Maps.Model.Marker"/>
        /// </summary>
        /// <param name="marker">Marker object to find the pin for</param>
        /// <returns>Corresponding <see cref="ParkingPin"/> object</returns>
        private ParkingPin GetParkingPin(Marker marker)
        {
            /*!!!----DEBUG----!!!
            foreach (var pin in formsMap.ParkingPins)
               System.Diagnostics.Debug.Write("Longitude: " + pin.Position.Latitude + "Latitude: " + pin.Position.Longitude);
            System.Diagnostics.Debug.Write("Marker: " + marker.Position.Latitude + " : " + marker.Position.Longitude);
            */

            var position = new Position(marker.Position.Latitude, marker.Position.Longitude);
            //TODO MIGHT BE QUESTIONABLE CODE, REVIEW! using formsMap.ParkingPins vs. parkingPins
            foreach (var pin in formsMap.ParkingPins)
            {
                if (pin.Position.Equals(position))
                {
                    return pin;
                }
            }
            return null;
        }
    }
}