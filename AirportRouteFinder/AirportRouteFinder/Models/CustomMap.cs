using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace AirportRouteFinder.Models
{
    /// <summary>
    /// Custom map will be attached to a CustomRenderer.
    /// </summary>
    public class CustomMap : Map
    {
        public CustomMap()
        {
            IsShowingUser = false;
            RouteCoordinates = new List<Position>();
        }

        /// <summary>
        /// List of Positions will be used to draw lines between airports.
        /// </summary>
        public List<Position> RouteCoordinates { get; set; }
    }
}
