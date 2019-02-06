using AirportRouteFinder.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirportRouteFinder.Utilities
{
    /// <summary>
    /// Class to store and manage parsed data.
    /// </summary>
    public class DataManager
    {
        private readonly AirportsImporter _airportsImporter;
        private readonly RoutesImporter _routesImporter;

        private readonly Dictionary<string, Airport> _airports;
        private int _airportCount;
        private List<Route> _routes;
        private FlightNetwork _network;

        public DataManager(AirportsImporter airportsImporter, RoutesImporter routesImporter)
        {
            _airportsImporter = airportsImporter;
            _routesImporter = routesImporter;

            _airports = new Dictionary<string, Airport>();
        }

        public Dictionary<string, Airport> AirportDictionary => _airports;

        public async Task GetAirports()
        {
            await Task.Run(() =>
            {
                _airportCount = _airportsImporter.ImportAirports(_airports);
            });
        }

        public async Task GetRoutes()
        {
            await Task.Run(() =>
            {
                _routes = _routesImporter.ImportRoutes();
            });
        }

        public async Task BuildGraph()
        {
            await Task.Run(() =>
            {
                _network = new FlightNetwork(_airportCount, _airports, _routes);
            });
        }

        public async Task<List<string>> Search(string origin, string destination)
        {
            return await Task.Run(() => _network.ShortestPath(_airports, origin, destination));
        }
    }
}
