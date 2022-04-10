using FestivalPlaner.Models;
using FestivalPlaner.Services;
using FestivalPlaner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FestivalPlaner.Views
{
    public partial class MapPupUp : Popup
    {
        private NewItemPage newItemPage;
        private Pin pin;
         public  MapPupUp(NewItemPage _newItemPage)
        {
            InitializeComponent();
            newItemPage = _newItemPage;
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(GeoLocationService.actualLocation.Latitude, GeoLocationService.actualLocation.Longitude), Distance.FromMeters(1)));

        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
            if (pin != null)
                newItemPage.OnSafe(pin.Label + "\n" + pin.Address);

        }

        private async void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            try
            {
                Map.Pins.Clear();
                var result = await Geocoding.GetPlacemarksAsync(e.Position.Latitude, e.Position.Longitude);
                if (!result.Any()) return;
                pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(result.FirstOrDefault().Location.Latitude, result.FirstOrDefault().Location.Longitude),
                    Label = result.FirstOrDefault().CountryCode + " " + result.FirstOrDefault().CountryName + ", " + result.FirstOrDefault().SubAdminArea,
                    Address = result.FirstOrDefault().Locality + ", " + result.FirstOrDefault().PostalCode + ", " + result.FirstOrDefault().Thoroughfare + " " + result.FirstOrDefault().SubThoroughfare
                };
                Map.Pins.Add(pin);
                NewItemViewModel.place = pin.Label + "\n" + pin.Address;
                NewItemViewModel.latitude = result.FirstOrDefault().Location.Latitude;
                NewItemViewModel.longitude = result.FirstOrDefault().Location.Longitude;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Location not found", ex.ToString(), "Cancel");
            }

        }
    }
}