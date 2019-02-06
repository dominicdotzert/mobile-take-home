using AirportRouteFinder.Models;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AirportRouteFinder.Utilities
{
    /// <summary>
    /// Class to handle data parsing from the 'airports.csv' file.
    /// </summary>
    public class AirportsImporter
    {
        private readonly string _path;

        /// <summary>
        /// Initializes the object.
        /// </summary>
        /// <param name="resourcePath">The resource path to the airports.csv file.</param>
        public AirportsImporter(string resourcePath)
        {
            _path = resourcePath;
        }

        /// <summary>
        /// Imports airport data by opening a stream to the embedded resource file and
        /// parses data line by line. This method assumes the data to be valid.
        /// </summary>
        /// <param name="airports">The dictionary in which to store the airport data.</param>
        /// <returns>Returns the total number of airports</returns>
        public int ImportAirports(Dictionary<string, Airport> airports)
        {
            var count = 0;

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

                    // since airport names may contain commas, use the last three
                    // indices of the split array to get the code, latitude, and longitude
                    var codeIndex = split.Length - 3;
                    var latIndex = split.Length - 2;
                    var longIndex = split.Length - 1;

                    var code = split[codeIndex];
                    var latitude = double.Parse(split[latIndex]);
                    var longitude = double.Parse(split[longIndex]);

                    if (code.Length != 3)
                    {
                        continue;
                    }

                    // The current airport count will serve as the unique int index for each airport.
                    if (!airports.ContainsKey(code))
                    {
                        airports.Add(code, new Airport(code, latitude, longitude, count));
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
