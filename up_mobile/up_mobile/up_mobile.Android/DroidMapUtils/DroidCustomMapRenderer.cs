using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using up_mobile.Droid.DroidMapUtils;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using up_mobile.Map.Utils;
using Android.Gms.Maps.Model;

[assembly: ExportRenderer(typeof(LotMap), typeof(DroidCustomMapRenderer))]
namespace up_mobile.Droid.DroidMapUtils
{
    /// <summary>
    /// <see cref="MapRenderer"/> for Android platform
    /// </summary>
    public class DroidCustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        List<ParkingPin> parkingPins;

        public DroidCustomMapRenderer(Context context) : base(context) { }



        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.Maps.Map> e)
        {
            System.Diagnostics.Debug.Write("Calling OnElementChanged in DroidCustomMapRenderer");

            base.OnElementChanged(e);


            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (LotMap)e.NewElement;
                parkingPins = formsMap.ParkingPins;
                Control.GetMapAsync(this);
            }
            System.Diagnostics.Debug.Write("Exiting OnElementChanged in DroidCustomMapRenderer");

        }



        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            System.Diagnostics.Debug.Write("Calling OnMapReady in DroidCustomMapRenderer");

            NativeMap.InfoWindowClick += OnInfoWindowClick;
            NativeMap.SetInfoWindowAdapter(this);
            System.Diagnostics.Debug.Write("Exiting OnMapReady in DroidCustomMapRenderer");
        }



        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));

            if (pin is ParkingPin)
            {
                var pp = pin as ParkingPin;

                marker.SetTitle(pp.Text);
                marker.SetSnippet(pp.Label);
                //marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
            }
            else
            {

            }
            return marker;
        }



        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;

                var parkingPin = GetParkingPin(marker);
                if (parkingPin == null)
                {
                    throw new Exception("Custom pin not found");
                }

                view = inflater.Inflate(Resource.Layout.DroidInfoWindow, null);

                /*var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                /var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);

                if (infoTitle != null)
                {
                    infoTitle.Text = marker.Title;
                }
                if (infoSubtitle != null)
                {
                    infoSubtitle.Text = marker.Snippet;
                }*/

                return view;
            }
            return null;
        }


        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }



        private ParkingPin GetParkingPin(Marker marker)
        {
            throw new NotImplementedException();
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            /*
            var parkingPin = GetParkingPin(e.Marker);
            if (parkingPin == null)
            {
                throw new Exception("Custom pin not found");
            }
            */

            System.Diagnostics.Debug.Write(sender.GetType().ToString());


        }
    }
}