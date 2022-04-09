using System;
using System.Threading;
using System.Threading.Tasks;
using FestivalPlaner.Models;
using Xamarin.Essentials;

namespace FestivalPlaner.Services
{

    public  class GeoLocationService
    {
        public  CancellationTokenSource cts;
        private Location actualLocation;
        private bool isGettingLocation = false;
        public  async Task GetCurrentLocation()
        {
            var permissionWhenUsing = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            var permissionAlways = await Permissions.RequestAsync<Permissions.LocationAlways>();
            
            if (permissionAlways == PermissionStatus.Granted)
            {
                try
                {
                    
                    while (!isGettingLocation)
                    {
                        isGettingLocation = true;
                        var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(45));
                        cts = new CancellationTokenSource();
                        actualLocation = await Geolocation.GetLocationAsync(request, cts.Token);
                        await Task.Delay(15000);
                        foreach (FestivalModel festivalModel in App.festivals)
                        {
                            double nearestLocationFestival = Location.CalculateDistance(actualLocation, new Location(festivalModel.latitude, festivalModel.longitude), DistanceUnits.Kilometers);
                            Console.WriteLine("Distance: " + nearestLocationFestival);
                            if (nearestLocationFestival < 50)
                                await App.Current.MainPage.DisplayAlert("We have find a near Festival at your location!", festivalModel.name + "\n" + festivalModel.place + "\n"+ "Entfernung: " + nearestLocationFestival + " km", "OKAY GEIL");
                        }
                        await Task.Delay(15000);
                        isGettingLocation = false;
                    }
                   
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    Console.WriteLine(fnsEx.Message);
                    // Handle not supported on device exception
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    Console.WriteLine(fneEx.Message);
                    // Handle not enabled on device exception
                }
                catch (PermissionException pEx)
                {
                    Console.WriteLine(pEx.Message);
                    // Handle permission exception
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // Unable to get location
                }
            }
            else
            {
                Console.WriteLine("NOT GRANTED");
            }
               
        }

        private  void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            OnDisappearing();
        }
    }
}
