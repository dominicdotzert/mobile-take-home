using AirportRouteFinder.Droid.CustomRenderers;
using AirportRouteFinder.Models;
using Android.Content;
using System.Collections.Generic;
using Android.Gms.Maps.Model;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(RouteDrawer))]
namespace AirportRouteFinder.Droid.CustomRenderers
{
    public class RouteDrawer : MapRenderer
    {
        private const int LineColour = 0x66FF0000;

        private List<Position> _routeCoordinates;

        public RouteDrawer(Context context) : base(context)
        {
        }

        protected void OnElementChanged()
        {
            OnElementChanged(null);
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            var map = (CustomMap) e.NewElement;
            _routeCoordinates = map.RouteCoordinates;
            Control.GetMapAsync(this);
        }

        protected override void OnMapReady(Android.Gms.Maps.GoogleMap map)
        {
            base.OnMapReady(map);

            var polylineOptions = new PolylineOptions();
            polylineOptions.InvokeColor(LineColour);

            foreach (var position in _routeCoordinates)
            {
                polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
            }

            NativeMap.AddPolyline(polylineOptions);
        }
    }
}