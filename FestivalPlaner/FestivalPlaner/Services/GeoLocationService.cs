using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FestivalPlaner.Models;
using FestivalPlaner.ViewModels;
using FestivalPlaner.Views;
using Plugin.LocalNotification;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FestivalPlaner.Services
{

    public static class GeoLocationService
    {
        public static int notificationIncrement = 0;
        public static ItemsViewModel viewModel = new ItemsViewModel();
        public static List<NotificationFestival> nearFestivals = new List<NotificationFestival>();

        public static CancellationTokenSource cts;
        public static Location actualLocation;
        private static bool isCheckingLocation = false;
        public static bool newNearFestival = false;
        public static async Task GetCurrentLocation()
        {

            var permissionWhenUsing = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if(permissionWhenUsing == PermissionStatus.Denied)
            {
                await GetCurrentLocation();
                return;
            }
            var permissionAlways = await Permissions.RequestAsync<Permissions.LocationAlways>();


            try
            {
                while (!isCheckingLocation)
                {
                    isCheckingLocation = true;
                    var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(45));
                    cts = new CancellationTokenSource();
                    actualLocation = await Geolocation.GetLocationAsync(request, cts.Token);
                    await Task.Delay(TimeSpan.FromMinutes(1));
                    foreach (FestivalModel festivalModel in App.festivals)
                    {
                        double nearestLocationFestival = Location.CalculateDistance(actualLocation, new Location(festivalModel.latitude, festivalModel.longitude), DistanceUnits.Kilometers);
                        if (nearestLocationFestival < 5)
                        {
                            newNearFestival = true;
                            foreach (NotificationFestival nearFestival in nearFestivals.ToArray())
                                if (nearFestival.Festival == festivalModel) newNearFestival = false;
                            if (newNearFestival)
                            {
                                var rndVerficationNumber = new Random().Next();
                                notificationIncrement++;                              
                                var notification = new NotificationRequest
                                {
                                    BadgeNumber = notificationIncrement,
                                    Title = "Festival at your location!",
                                    Subtitle = festivalModel.name + "\n" + festivalModel.place,
                                    Description = "Entfernung: " + nearestLocationFestival + " km",
                                    NotificationId = rndVerficationNumber
                                };
                                var tempNearFestival = new NotificationFestival(festivalModel, notification);
                                nearFestivals.Add(tempNearFestival);
                                await Task.Delay(TimeSpan.FromSeconds(10));
                            }
                        }
                    }
                    isCheckingLocation = false;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await App.Current.MainPage.DisplayAlert("Handle not supported on device exception.", fnsEx.Message, "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await App.Current.MainPage.DisplayAlert("Handle not enabled on device exception.", fneEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await App.Current.MainPage.DisplayAlert("Handle permission exception.", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Unable to get location.", ex.Message, "OK");
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
