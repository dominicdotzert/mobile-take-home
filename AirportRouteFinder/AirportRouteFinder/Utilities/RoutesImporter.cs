using AirportRouteFinder.Models;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AirportRouteFinder.Utilities
{
    /// <summary>
    /// Class to handle data parsing from the 'routes.csv' file.
    /// </summary>
    public class RoutesImporter
    {
        private readonly string _path;

        /// <summary>
        /// Initializes the object.
        /// </summary>
        /// <param name="resourcePath">The resource path to the routes.csv file.</param>
        public RoutesImporter(string resourcePath)
        {
            _path = resourcePath;
        }

        /// <summary>
        /// Imports route data by opening a stream to the embedded resource file and
        /// parses data line by line. This method assumes the data to be valid.
        /// </summary>
        /// <returns>Returns the list of routes.</returns>
        public List<Route> ImportRoutes()
        {
            var data = new List<Route>();

            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(_path))
            using (var reader = new StreamReader(stream))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        continue;
                    }

                    var split = line.Split(',');
                    var airline = split[0];
                    var origin = split[1];
                    var destination = split[2];

                    data.Add(new Route(airline, origin, destination));
                }
            }

            return data;
        }
    }
}
