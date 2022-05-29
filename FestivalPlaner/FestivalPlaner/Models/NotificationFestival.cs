using FestivalPlaner.Services;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using FestivalPlaner.Messages;
using FestivalPlaner.ViewModels;
using FestivalPlaner.Views;
using System.Threading.Tasks;
using Plugin.LocalNotification.EventArgs;

namespace FestivalPlaner.Models
{
    public class NotificationFestival
    {
        public FestivalModel Festival { get; set; }

        public NotificationRequest NotificationRequest { get; set; }

        public NotificationFestival(FestivalModel festivalModel, NotificationRequest notificationRequest)
        {
            Festival = festivalModel;
            NotificationRequest = notificationRequest;
            Task.Run(() => GeoLocationService.viewModel.ExecuteLoadItemsCommand()).Wait();
            Task.Run(() => this.StartNotification()).Wait();
        }

        public async Task StartNotification()
        {
            await NotificationCenter.Current.Show(this.NotificationRequest);

            NotificationCenter.Current.NotificationTapped += this.OnLocalNotificationTapped;
        }
        private async void OnLocalNotificationTapped(NotificationEventArgs e)
        {

            if (this.NotificationRequest.NotificationId == e.Request.NotificationId)
            {
                GeoLocationService.nearFestivals.Remove(this);
                await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={this.Festival._id}");
                if (Views.FestivalPlaner.calendarToggle)
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var calendarMessage = new CalendarMessage(this.Festival.startDate, this.Festival.endDate, this.Festival.name, this.Festival.place);
                        MessagingCenter.Send(calendarMessage, "CreateCalendar");
                    });
                NotificationCenter.Current.NotificationTapped -= this.OnLocalNotificationTapped;
                GeoLocationService.notificationIncrement--;

            }
        }
    }
}
