using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FestivalPlaner.Messages;
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
        public static bool newNearFestival = false;
        public static bool isCeckingDatabase = false;
        public static async Task Run(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                while (Views.FestivalPlaner.gpsToggle)
                {
                    token.ThrowIfCancellationRequested();
                    try
                    {

                        await Task.Delay(TimeSpan.FromMinutes(Views.FestivalPlaner.gpsSearchIntervall));
                        await GeoLocationService.GetCurrentLocation();
                        if (GeoLocationService.actualLocation != null)
                        {
                            var message = new LocationMessage();
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                MessagingCenter.Send<LocationMessage>(message, "Location");
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            var errormessage = new LocationErrorMessage();
                            MessagingCenter.Send(errormessage, ex.ToString());
                        });
                    }
                }
                return;
            }, token);
        }
        public static async Task GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(45));
                cts = new CancellationTokenSource();
                actualLocation = await Geolocation.GetLocationAsync(request, cts.Token);
                isCeckingDatabase = true;
                foreach (FestivalModel festivalModel in App.festivals)
                {
                    double nearestLocationFestival = Location.CalculateDistance(actualLocation, new Location(festivalModel.latitude, festivalModel.longitude), DistanceUnits.Kilometers);
                    if (nearestLocationFestival < Views.FestivalPlaner.gpsSearchRadius)
                    {
                        newNearFestival = true;
                        foreach (NotificationFestival nearFestival in nearFestivals.ToArray())
                            if (nearFestival.Festival == festivalModel) newNearFestival = false;
                        if (newNearFestival)
                        {
                            var dateCheckerMessage = new DateCheckerMessage(festivalModel.startDate, festivalModel.endDate);
                            if (Views.FestivalPlaner.calendarToggle)
                            {

                                MessagingCenter.Send(dateCheckerMessage, "DateCheckerMessage");
                                await Task.Delay(TimeSpan.FromSeconds(1));
                            }

                            if (!Views.FestivalPlaner.calendarToggle)
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
                            }

                            else if (dateCheckerMessage.isFree)
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
                            }
                        }
                    }
                }
                isCeckingDatabase = false;
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
