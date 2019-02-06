using AirportRouteFinder.Utilities;
using AirportRouteFinder.ViewModels;
using Xamarin.Forms;

namespace AirportRouteFinder.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(DataManager dataManager)
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(Navigation, dataManager);
        }
    }
}
