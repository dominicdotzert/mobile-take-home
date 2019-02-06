namespace AirportRouteFinder.Models
{
    public class Route
    {
        public Route(string airline, string origin, string destination)
        {
            Airline = airline;
            Origin = origin;
            Destination = destination;
        }

        public string Airline { get; }
        public string Origin { get; }
        public string Destination { get; }
    }
}
