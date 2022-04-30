using EventKit;
using EventKitUI;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

namespace FestivalPlaner.iOS
{
    public class CalendarService
    {
        private EKEventStore eventStore = AppEvent.CurrentEvent.EventStore;
        private CreateEventEditViewDelegate createEventEditViewDelegate;
        private EKCalendar eKCalendar = null;
        private static UIViewController viewController;
        public  EKEvent newEvent;
        public CalendarService(DateTime _startTime, DateTime _endTime, string _title, string _description)
        {
            newEvent.AddAlarm(EKAlarm.FromDate((NSDate)(_startTime - TimeSpan.FromDays(2))));
            // make the event start 20 minutes from now and last 30 minutes
            newEvent.StartDate = (NSDate)_startTime;
            newEvent.EndDate = (NSDate)_endTime;
            newEvent.Title = _title;
            newEvent.Notes = _description;
            CreateEvent();
        }
    
        void CreateEvent()
        {

            EKCalendar[] calendars = eventStore.GetCalendars(EKEntityType.Event);
            foreach (EKCalendar calendar in calendars)
            {
                if(calendar.Title == "Calendar")
                {
                    eKCalendar = calendar;
                }
            }
            Device.BeginInvokeOnMainThread(() =>
            {

                EventKitUI.EKEventEditViewController eventController = new EventKitUI.EKEventEditViewController();

                eventController.EventStore = eventStore;

                createEventEditViewDelegate = new CreateEventEditViewDelegate(eventController);
                eventController.EditViewDelegate = createEventEditViewDelegate;

                viewController = new UIViewController();
                viewController.PresentViewController(eventController, true, null);

                newEvent = EKEvent.FromStore(eventStore);
              

                newEvent.Calendar = eKCalendar;
                
                NSError e;
                eventStore.SaveEvent(newEvent, EKSpan.ThisEvent, out e);
            });
           
        }

    }
    public class CreateEventEditViewDelegate : EventKitUI.EKEventEditViewDelegate
    {
        // we need to keep a reference to the controller so we can dismiss it
        protected EventKitUI.EKEventEditViewController eventController;

        public CreateEventEditViewDelegate(EventKitUI.EKEventEditViewController eventController)
        {
            // save our controller reference
            this.eventController = eventController;
        }

        // completed is called when a user eith
        public override void Completed(EventKitUI.EKEventEditViewController controller, EKEventEditViewAction action)
        {
            eventController.DismissViewController(true, null);

            switch (action)
            {

                case EKEventEditViewAction.Canceled:
                    break;
                case EKEventEditViewAction.Deleted:
                    break;
                case EKEventEditViewAction.Saved:
                    // if you wanted to modify the event you could do so here,
                    // and then save:
                    //App.Current.EventStore.SaveEvent ( controller.Event, )
                    break;
            }
        }
    }
}

