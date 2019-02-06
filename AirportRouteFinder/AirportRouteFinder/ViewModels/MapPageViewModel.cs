using AirportRouteFinder.Models;
using AirportRouteFinder.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AirportRouteFinder.ViewModels
{
    public class MapPageViewModel : NotifyPropertyChanged
    {
        private const double ZoomRadius = 500.0; // km

        private readonly Task<List<string>> _searchTask;
        private readonly CustomMap _map;
        private readonly DataManager _dataManager;

        private string _userSearch;
        private bool _searchComplete;
        private bool _routePossible;
        private List<string> _route;

        /// <summary>
        /// The ViewModel to manage the MapPage view.
        /// Initializes properties, then waits for the user search to complete in a background thread.
        /// </summary>
        /// <param name="search">The task which performs the shortest path search.</param>
        /// <param name="map">The CustomMap object from the view.</param>
        /// <param name="dataManager">The DataManager object.</param>
        /// <param name="userSearch">The descriptive string of the user's search.</param>
        public MapPageViewModel(Task<List<string>> search, CustomMap map, DataManager dataManager, string userSearch)
        {
            _searchTask = search;
            _map = map;
            _dataManager = dataManager;

            UserSearch = userSearch;
            SearchComplete = false;
            RoutePossible = false;

            WaitForSearch();
        }

        /// <summary>
        /// The string describing the user's search. Bound to the page's title element.
        /// </summary>
        public string UserSearch
        {
            get
            {
                return _userSearch;
            }

            private set
            {
                if (_userSearch != value)
                {
                    _userSearch = value;
                    OnPropertyChanged(nameof(UserSearch));
                }
            }
        }

        /// <summary>
        /// The flag which represents if a search is complete.
        /// </summary>
        public bool SearchComplete
        {
            get
            {
                return _searchComplete;
            }

            set
            {
                if (_searchComplete != value)
                {
                    _searchComplete = value;
                    OnPropertyChanged(nameof(SearchComplete));
                }
            }
        }

        /// <summary>
        /// The flag which represents if a route is possible for the user's search.
        /// </summary>
        public bool RoutePossible
        {
            get
            {
                return _routePossible;
            }

            set
            {
                if (_routePossible != value)
                {
                    _routePossible = value;
                    OnPropertyChanged(nameof(RoutePossible));
                }
            }
        }

        /// <summary>
        /// Asynchronously waits for the user search to complete.
        /// </summary>
        /// <returns></returns>
        private async Task WaitForSearch()
        {
            await Task.Run(async () =>
            {
                await _searchTask;
                SearchComplete = true;
                RoutePossible = _searchTask.Result.Count != 0;

                if (_routePossible)
                {
                    _route = _searchTask.Result;
                    // Must invoke UpdateMap to run on the UI thread.
                    Device.BeginInvokeOnMainThread(UpdateMap);
                }
            });
        }

        /// <summary>
        /// Updates the map to display pins for each airport and mark the route.
        /// </summary>
        private void UpdateMap()
        {
            foreach (var airport in _route)
            {
                var latitude = _dataManager.AirportDictionary[airport].Latitude;
                var longitude = _dataManager.AirportDictionary[airport].Longitude;
                var position = new Position(latitude, longitude);

                _map.RouteCoordinates.Add(position);
                
                _map.Pins.Add(new Pin
                {
                    Position = position,
                    Label = airport,
                    Type = PinType.Generic
                });
            }

            // Center view on origin.
            _map.MoveToRegion(MapSpan.FromCenterAndRadius(_map.RouteCoordinates[0], Distance.FromKilometers(ZoomRadius)));
        }
    }
}
