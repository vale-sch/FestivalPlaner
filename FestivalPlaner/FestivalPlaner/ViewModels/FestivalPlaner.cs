using FestivalPlaner.Messages;
using FestivalPlaner.Utils;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FestivalPlaner.ViewModels
{
    public class FestivalPlaner : BaseViewModel
    {

        #region vars
        public string userMessage;
        #endregion vars

        #region properties
        public string UserMessage
        {
            get => userMessage;
            set => SetProperty(ref userMessage, value);
        }
        #endregion properties


        readonly ILocationConsent locationConsent;

        public FestivalPlaner()
        {
            locationConsent = DependencyService.Get<ILocationConsent>();
            HandleReceivedMessages();
            locationConsent.GetLocationConsent();
            SecureStorage.SetAsync(Constants.SERVICE_STATUS_KEY, "1");
            ValidateStatus();
        }


        void ValidateStatus()
        {
            var status = SecureStorage.GetAsync(Constants.SERVICE_STATUS_KEY).Result;
            if (status != null && status.Equals("1"))
            {
                Start();
            }
        }

        void Start()
        {
            var message = new StartServiceMessage();
            MessagingCenter.Send(message, "ServiceStarted");
            UserMessage = "Location Service has been started!";
            SecureStorage.SetAsync(Constants.SERVICE_STATUS_KEY, "1");
        }
       /* void Stop()
        {
            var message = new StopServiceMessage();
            MessagingCenter.Send(message, "ServiceStopped");
            UserMessage = "Location Service has been stoppped!";
            SecureStorage.SetAsync(Constants.SERVICE_STATUS_KEY, "0");
        }*/

        void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<LocationMessage>(this, "Location", message => {
                Device.BeginInvokeOnMainThread(() => {
                    UserMessage = "Location Updated";
                });
            });
            MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStopped", message => {
                Device.BeginInvokeOnMainThread(() => {
                    UserMessage = "Location Service has been stopped!";
                });
            });
            MessagingCenter.Subscribe<LocationErrorMessage>(this, "LocationError", message => {
                Device.BeginInvokeOnMainThread(() => {
                    UserMessage = "There was an error updating location!";
                });
            });
        }
    }
}
