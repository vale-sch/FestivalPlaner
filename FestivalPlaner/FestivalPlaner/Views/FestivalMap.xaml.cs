using FestivalPlaner.Models;
using FestivalPlaner.Services;
using FestivalPlaner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FestivalPlaner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FestivalMap : ContentPage
    {
        public FestivalMap()
        {
            InitializeComponent();
            
            if (FestivalPlaner.gpsToggle)
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(GeoLocationService.actualLocation.Latitude, GeoLocationService.actualLocation.Longitude), Distance.FromKilometers(1)));
            GenerateMapContent();

        }
        private void GenerateMapContent()
        {
            foreach(FestivalModel festival in App.festivals)
            {
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(festival.latitude,festival.longitude),
                    Label = festival.name,
                    Address = festival.place
                };
                Map.Pins.Add(pin);
            }
        }
    }
}