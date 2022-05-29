using FestivalPlaner.Messages;
using FestivalPlaner.Utils;
using System;
using System.ComponentModel;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FestivalPlaner.Views
{
    public partial class FestivalPlaner : ContentPage
    {
        public static FestivalPlaner Instance { get; private set; }
        public static bool calendarToggle = true;
        public static bool gpsToggle = true;
        public static int gpsSearchRadius = 5;
        public static float gpsSearchIntervall = 0.01f;
        public FestivalPlaner()
        {
            InitializeComponent();
        }

        private void calendarBool_Toggled(object sender, ToggledEventArgs e)
        {
            if (calendarToggle) calendarToggle = false;
            else calendarToggle = true;
        }

        private void gpsBool_Toggled(object sender, ToggledEventArgs e)
        {
            if (gpsToggle)
            {
                var message = new StopServiceMessage();
                MessagingCenter.Send(message, "ServiceStopped");
                SecureStorage.SetAsync(Constants.SERVICE_STATUS_KEY, "0");
                gpsToggle = false;
            }
            else
            {
                var message = new StartServiceMessage();
                MessagingCenter.Send(message, "ServiceStarted");
                SecureStorage.SetAsync(Constants.SERVICE_STATUS_KEY, "1");
                gpsToggle = true;
            }
        }

        private void searchRadius_Completed(object sender, EventArgs e)
        {
            gpsSearchRadius = int.Parse(searchRadius.Text);

        }

        private void searchInterval_Completed(object sender, EventArgs e)
        {
            gpsSearchIntervall = float.Parse(searchInterval.Text);
        }
    }
}