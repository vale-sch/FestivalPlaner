using System;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Essentials;

namespace FestivalPlaner.Services
{

    public static class GeoLocationService
    {
        public static double Latitude { get; set; }
        public static double Longitude { get; set; }
        public static double Altitude { get; set; }

        public static CancellationTokenSource cts;
        readonly static bool isGettingLocation = true;
        public static async Task GetCurrentLocation()
        {
            var permission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.LocationAlways>();
            if (permission == PermissionStatus.Granted)
                try
                {
                   
                    while (isGettingLocation)
                    {
                        var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(45));
                        cts = new CancellationTokenSource();
                        var location = await Geolocation.GetLocationAsync(request, cts.Token);
                        Longitude = location.Longitude;
                        Latitude = location.Latitude;
                        Altitude = (double)location.Altitude;
                        Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                        await Task.Delay(30000);
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

        private static void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            OnDisappearing();
        }
    }
}
