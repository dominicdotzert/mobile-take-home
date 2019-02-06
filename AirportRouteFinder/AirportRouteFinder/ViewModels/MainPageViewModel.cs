using AirportRouteFinder.Resources;
using AirportRouteFinder.Utilities;
using AirportRouteFinder.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AirportRouteFinder.ViewModels
{
    public class MainPageViewModel : NotifyPropertyChanged
    {
        private readonly INavigation _navigation;
        private readonly DataManager _dataManager;

        private string _text;
        private bool _dataIsLoaded;
        private bool _inputError;
        private string _errorText;

        /// <summary>
        /// The ViewModel to manage the MainPage view.
        /// Initializes properties, then loads data in a background thread.
        /// </summary>
        /// <param name="navigation">The Navigation object for the view.</param>
        /// <param name="dataManager">The DataManager object.</param>
        public MainPageViewModel(INavigation navigation, DataManager dataManager)
        {
            _navigation = navigation;
            _dataManager = dataManager;

            Text = Strings.LoadingRoutesLabel;
            DataIsLoaded = false;
            InputError = false;
            ErrorText = string.Empty;
            SearchCommand = new Command(Search);

            LoadData();
        }

        /// <summary>
        /// The main descriptive text shown in the view. Bound to the central text element.
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }

            private set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        /// <summary>
        /// The flag which represents if the data has finished loading.
        /// </summary>
        public bool DataIsLoaded
        {
            get
            {
                return _dataIsLoaded;
            }

            private set
            {
                if (_dataIsLoaded != value)
                {
                    _dataIsLoaded = value;
                    OnPropertyChanged(nameof(DataIsLoaded));
                }
            }

        }

        /// <summary>
        /// The flag which displays the error label for invalid user input.
        /// </summary>
        public bool InputError
        {
            get
            {
                return _inputError;
            }

            private set
            {
                if (_inputError != value)
                {

                    _inputError = value;
                    OnPropertyChanged(nameof(InputError));
                }
            }

        }

        /// <summary>
        /// The text displayed in the error label.
        /// </summary>
        public string ErrorText
        {
            get
            {
                return _errorText;
            }

            private set
            {
                if (_errorText != value)
                {
                    _errorText = value;
                    OnPropertyChanged(nameof(ErrorText));
                }
            }
        }

        /// <summary>
        /// The ICommand property bound to the search button.
        /// </summary>
        public ICommand SearchCommand { get; }

        /// <summary>
        /// The string property bound to the origin text field.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// The string property bound to the destination text field.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Asynchronously waits for the data to be loaded.
        /// </summary>
        /// <returns></returns>
        private async Task LoadData()
        {
            await Task.Run(async () =>
            {
                var loadAirports = _dataManager.GetAirports();
                var loadRoutes = _dataManager.GetRoutes();
                await Task.WhenAll(loadAirports, loadRoutes);

                await _dataManager.BuildGraph();

                DataIsLoaded = true;
                Text = Strings.EnterDestinationLabel;
            });
        }

        /// <summary>
        /// If user search is valid, begins a search for the shortest route and opens the MapPage.
        /// </summary>
        private async void Search()
        {
            var origin = Origin?.ToUpper();
            var destination = Destination?.ToUpper();

            if (!ValidateEntries(origin, destination))
            {
                return;
            }

            var search = _dataManager.Search(origin, destination);
            await _navigation.PushAsync(new MapPage(search, _dataManager, $"{origin} {Strings.To} {destination}"));
        }

        /// <summary>
        /// Checks if the user entries are valid and displays an error message if they are not.
        /// </summary>
        /// <returns>Returns if the user entries are valid.</returns>
        private bool ValidateEntries(string origin, string destination)
        {
            ErrorText = string.Empty;

            var errors = new List<string>();
            if (string.IsNullOrEmpty(origin) || !_dataManager.AirportDictionary.ContainsKey(origin))
            {
                errors.Add(Strings.InvalidOrigin);
            }

            if (string.IsNullOrEmpty(destination) || !_dataManager.AirportDictionary.ContainsKey(destination))
            {
                errors.Add(Strings.InvalidDestination);
            }

            foreach (var error in errors)
            {
                ErrorText += $"{error}\n";
            }

            InputError = errors.Count != 0;

            return !InputError;
        }
    }
}
