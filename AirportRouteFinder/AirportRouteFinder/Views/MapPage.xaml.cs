using AirportRouteFinder.Utilities;
using AirportRouteFinder.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AirportRouteFinder.Views
{
    public partial class MapPage : ContentPage
	{
		public MapPage(Task<List<string>> search, DataManager dataManager, string searchString)
		{
			InitializeComponent();
            BindingContext = new MapPageViewModel(search, Map, dataManager, searchString);
        }
	}
}