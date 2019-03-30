using System.Collections.Generic;
using Android.Content;
using Android.Gms.Maps;
using up_mobile.Droid.DroidMapUtils;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using up_mobile.Map.Utils;
using Android.Gms.Maps.Model;
using Rg.Plugins.Popup.Services;
using System;

[assembly: ExportRenderer(typeof(LotMap), typeof(DroidCustomMapRenderer))]
namespace up_mobile.Droid.DroidMapUtils
{
    /// <summary>
    /// <see cref="MapRenderer"/> for Android platform
    /// </summary>
    public class DroidCustomMapRenderer : MapRenderer//, GoogleMap.IInfoWindowAdapter
    {
        List<ParkingPin> parkingPins;
        List<MapPolygon> mapPolygons;

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
                mapPolygons = formsMap.MapPolygons;
                Control.GetMapAsync(this);
            }
            System.Diagnostics.Debug.Write("Exiting OnElementChanged in DroidCustomMapRenderer");

        }
        
        protected override void OnMapReady(GoogleMap map)
        {
            System.Diagnostics.Debug.Write("Calling OnMapReady in DroidCustomMapRenderer");

            base.OnMapReady(map);

            googleMap = map;

            googleMap.MarkerClick += OnMarkerClicked;

            foreach (MapPolygon poly in mapPolygons)
                NativeMap.AddPolygon(CreatePolygon(poly));

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
        /// Method creating <see cref="PolygonOptions"/> to be added to the map
        /// </summary>
        /// <returns></returns>
        protected PolygonOptions CreatePolygon(MapPolygon polygon)
        {
            var options = new PolygonOptions();

            foreach (Position pos in polygon.Points)
                options.Add(new LatLng(pos.Latitude, pos.Latitude));

            var colour = ColorMapper.MapPercentageToRGB(polygon.Percentage);
            var hex = String.Format("0X{0:X2}{1:X2}{2:X2}{3:X2}", 120, colour.R, colour.G, colour.B);

            options.InvokeFillColor(Convert.ToInt32(hex, 16));
            options.InvokeStrokeColor(0X000000);

            options.Clickable(true);

            return options;
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