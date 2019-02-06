namespace AirportRouteFinder.Models
{
    public class Airport
    {
        public Airport(string code, double latitude, double longitude, int index)
        {
            Code = code;
            Latitude = latitude;
            Longitude = longitude;
            Index = index;
        }

        public string Code { get; }
        public double Latitude { get; }
        public double Longitude { get; }
        public int Index { get; }
    }
}
