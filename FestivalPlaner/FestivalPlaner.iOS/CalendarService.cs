using EventKit;
using EventKitUI;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace FestivalPlaner.iOS
{
    public class CalendarService
    {
        private EKEventStore eventStore = AppEvent.CurrentEvent.EventStore;
        private CreateEventEditViewDelegate createEventEditViewDelegate;
        private EKCalendar eKCalendar = null;
        private static UIViewController viewController = new UIViewController();
        public CalendarService()
        {
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
            EventKitUI.EKEventEditViewController eventController =
           new EventKitUI.EKEventEditViewController();

            // set the controller's event store - it needs to know where/how to save the event
            eventController.EventStore = eventStore;

            // wire up a delegate to handle events from the controller
            createEventEditViewDelegate = new CreateEventEditViewDelegate(eventController);
            eventController.EditViewDelegate = createEventEditViewDelegate;

            // show the event controller
            viewController.PresentViewController(eventController, true, null);
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

