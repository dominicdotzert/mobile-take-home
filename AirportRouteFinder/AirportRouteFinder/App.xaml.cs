using AirportRouteFinder.Utilities;
using AirportRouteFinder.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AirportRouteFinder
{
    public partial class App : Application
    {
        private const string AirportCsvPath = "AirportRouteFinder.Data.airports.csv";
        private const string RoutesCsvPath = "AirportRouteFinder.Data.routes.csv";

        public App()
        {
            InitializeComponent();

            var airportsImporter = new AirportsImporter(AirportCsvPath);
            var routesImporter = new RoutesImporter(RoutesCsvPath);
            var dataManager = new DataManager(airportsImporter, routesImporter);
            MainPage = new NavigationPage(new MainPage(dataManager));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
