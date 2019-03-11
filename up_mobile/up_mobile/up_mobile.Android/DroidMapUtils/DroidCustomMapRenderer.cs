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

[assembly: ExportRenderer(typeof(LotMap), typeof(DroidCustomMapRenderer))]
namespace up_mobile.Droid.DroidMapUtils
{
    /// <summary>
    /// <see cref="MapRenderer"/> for Android platform
    /// </summary>
    public class DroidCustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
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
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
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

            //googleMap = map;

            System.Diagnostics.Debug.Write("Calling OnMapReady in DroidCustomMapRenderer");
            /*
            if (googleMap != null)
            {
                UpdatePins();
            }
            */
            NativeMap.InfoWindowClick += OnInfoWindowClick;
            NativeMap.SetInfoWindowAdapter(this);

            System.Diagnostics.Debug.Write("Exiting OnMapReady in DroidCustomMapRenderer");
        }
        /*
        private void UpdatePins(bool firstUpdate = true)
        {
            if (googleMap == null) return;

            if (formsMap.ParkingPins != null)
            {
                foreach (var pin in formsMap.ParkingPins)
                {
                    AddPin(pin);
                }

                if (firstUpdate)
                {
                    var observable = formsMap.ParkingPins as INotifyCollectionChanged;
                    if (observable != null)
                    {
                        observable.CollectionChanged += OnCustomPinsCollectionChanged;
                    }
                }
            }
        }

        private void OnCustomPinsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            
        }

        private void AddPin(ParkingPin pin)
        {
            var marker = new MarkerOptions();

            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Text);
            marker.SetSnippet(pin.Label);
            marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));

            googleMap.AddMarker(marker);
        }
        */

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



        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;

                var parkingPin = GetParkingPin(marker);
                if (parkingPin == null)
                {
                    return null;//throw new Exception("Custom pin not found");
                }

                view = inflater.Inflate(Resource.Layout.DroidInfoWindow, null);

                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);

                if (infoTitle != null)
                {
                    infoTitle.Text = marker.Title;
                }
                if (infoSubtitle != null)
                {
                    infoSubtitle.Text = marker.Snippet;
                }

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
            foreach (var pin in formsMap.ParkingPins)
                System.Diagnostics.Debug.Write("Longitude: " + pin.Position.Latitude + "Latitude: " + pin.Position.Longitude);
            System.Diagnostics.Debug.Write("Marker: " + marker.Position.Latitude + " : " + marker.Position.Longitude);

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

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            
            var parkingPin = GetParkingPin(e.Marker);
            if (parkingPin == null)
            {
                throw new Exception("Custom pin not found");
            }
            System.Diagnostics.Debug.Write("InfoWindowClicked!");

            System.Diagnostics.Debug.Write(sender.GetType().ToString());
        }
    }
}