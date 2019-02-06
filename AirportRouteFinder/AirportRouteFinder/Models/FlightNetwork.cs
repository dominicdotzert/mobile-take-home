using System.Collections.Generic;

namespace AirportRouteFinder.Models
{
    /// <summary>
    /// Class to generate and store the graph of airports and routes.
    /// </summary>
    public class FlightNetwork
    {
        private readonly int _vertices;
        private readonly List<string>[] _adjList;

        /// <summary>
        /// On construction, build an adjacency list graph representation of airports and routes.
        /// </summary>
        /// <param name="count">Number of vertices (airports).</param>
        /// <param name="airports">Dictionary of airports.</param>
        /// <param name="routes">Collection of routes.</param>
        public FlightNetwork(int count, IReadOnlyDictionary<string, Airport> airports, IEnumerable<Route> routes)
        {
            _vertices = count;
            _adjList = new List<string>[count];

            // Initialize vertices.
            // Each identified airport will correspond to an integer on [0,_vertices].
            for (var i = 0; i < _vertices; i++)
            {
                _adjList[i] = new List<string>();
            }

            // Add edges.
            foreach (var route in routes)
            {
                // Only add routes between known airports
                if (!airports.ContainsKey(route.Origin) ||
                    !airports.ContainsKey(route.Destination))
                {
                    continue;
                }

                var originIndex = airports[route.Origin].Index;
                _adjList[originIndex].Add(route.Destination);
            }
        }

        /// <summary>
        /// Does a BFS to find the first possible shortest route.
        /// </summary>
        /// <param name="airports">The dictionary of airports.</param>
        /// <param name="origin">The origin airport.</param>
        /// <param name="destination">The destination airport.</param>
        /// <returns>
        /// Returns the list of airports required to get from the origin to destination.
        /// Returns an empty list if there is no possible route.
        /// </returns>
        public List<string> ShortestPath(Dictionary<string, Airport> airports, string origin, string destination)
        {
            // Keep track of visited airports with a hashset.
            var visited = new HashSet<string>();

            // For each visited airport, store its "sub-origin".
            var previous = new string[_vertices];

            var queue = new Queue<string>();
            queue.Enqueue(origin);
            visited.Add(origin);
            while (queue?.Count != 0)
            {
                var subOrigin = queue.Dequeue();
                var subOriginIndex = airports[subOrigin].Index;
                for (var i = 0; i < _adjList[subOriginIndex].Count; i++)
                {
                    var subDestination = _adjList[subOriginIndex][i];

                    if (!visited.Contains(subDestination))
                    {
                        var adjListIndex = airports[subDestination].Index;
                        previous[adjListIndex] = subOrigin;

                        // Once the destination is found, construct the path list and return.
                        if (subDestination == destination)
                        {
                            return GetPath(destination, previous, airports);
                        }
                        
                        visited.Add(subDestination);
                        queue.Enqueue(subDestination);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Uses the array of previous airports to construct the list of airports from origin to destination.
        /// </summary>
        /// <param name="destination">The destination airport.</param>
        /// <param name="previous">The array of sub-origin airports.</param>
        /// <param name="airports">The dictionary of airports.</param>
        /// <returns></returns>
        private static List<string> GetPath(string destination, string[] previous, IReadOnlyDictionary<string, Airport> airports)
        {
            // In reverse order, construct a list from destination to origin.
            var path = new List<string> {destination};

            var i = airports[destination].Index;
            while (!string.IsNullOrEmpty(previous[i]))
            {
                path.Add(previous[i]);
                i = airports[previous[i]].Index;
            }

            // Reverse before returning, so list is from origin -> destination.
            path.Reverse();

            return path;
        }
    }
}
