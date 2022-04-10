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

    public class GeoLocationService
    {
        public CancellationTokenSource cts;
        public static Location actualLocation;
        private bool isGettingLocation = false;
        private FestivalModel nearFestival;
        private static List<int> notificationIds = new List<int>();
        public async Task GetCurrentLocation()
        {
            var permissionWhenUsing = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            var permissionAlways = await Permissions.RequestAsync<Permissions.LocationAlways>();


            try
            {
                while (!isGettingLocation)
                {
                    isGettingLocation = true;
                    var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(45));
                    cts = new CancellationTokenSource();
                    actualLocation = await Geolocation.GetLocationAsync(request, cts.Token);
                    await Task.Delay(TimeSpan.FromMinutes(5));
                    foreach (FestivalModel festivalModel in App.festivals)
                    {
                        double nearestLocationFestival = Location.CalculateDistance(actualLocation, new Location(festivalModel.latitude, festivalModel.longitude), DistanceUnits.Kilometers);
                        if (nearestLocationFestival < 50)
                        {
                            nearFestival = festivalModel;

                            notificationIds.Add((int)festivalModel.latitude + (int)festivalModel.longitude);
                            var notification = new NotificationRequest
                            {

                                BadgeNumber = (int)festivalModel.latitude + (int)festivalModel.longitude,
                                Title = "We have find a near Festival at your location!" + festivalModel.name + "\n",
                                Description = festivalModel.place + "\n" + "Entfernung: " + nearestLocationFestival + " km",
                                NotificationId = (int)festivalModel.latitude + (int)festivalModel.longitude

                            };
                            await NotificationCenter.Current.Show(notification);
                            //NotificationCenter.Current.NotificationReceived += Current_NotificationReceived;
                            NotificationCenter.Current.NotificationTapped += Current_NotificationTapped;
                        }
                    }
                    await Task.Delay(TimeSpan.FromMinutes(5));
                    isGettingLocation = false;
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

        private async void Current_NotificationTapped(Plugin.LocalNotification.EventArgs.NotificationEventArgs e)
        {
            await new ItemsViewModel().ExecuteLoadItemsCommand();
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={nearFestival._id}");

        }
        /* private void Current_NotificationReceived(Plugin.LocalNotification.EventArgs.NotificationEventArgs e)
         {
             Device.BeginInvokeOnMainThread(() =>
             {
                 App.Current.MainPage.DisplayAlert(e.ToString(), e.ToString(), "OK");

             });

         }*/
        private void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            OnDisappearing();
        }
    }
}
