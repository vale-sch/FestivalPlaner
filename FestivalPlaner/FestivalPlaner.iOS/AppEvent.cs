using EventKit;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace FestivalPlaner.iOS
{
    internal class AppEvent
    {

        public static AppEvent CurrentEvent
        {
            get { return currentEvent; }
        }
        private static AppEvent currentEvent;

        public EKEventStore EventStore
        {
            get { return eventStore; }
        }
        protected EKEventStore eventStore;

        static AppEvent()
        {
            currentEvent = new AppEvent();
        }
        protected AppEvent()
        {
            eventStore = new EKEventStore();
        }
        public static async Task Start()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            AppEvent.currentEvent.EventStore.RequestAccess(EKEntityType.Event,
    (bool granted, NSError e) =>
    {
        if (granted)
        {

        }
        //do something here
        else
            new UIAlertView("Access Denied",
"User Denied Access to Calendar Data", null,
"ok", null).Show();
    });
        }
    }
}