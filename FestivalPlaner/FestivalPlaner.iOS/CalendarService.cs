using EventKit;
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
        public CalendarService()
        {
            GetCalendars();
        }
        void GetCalendars()
        {
            EKCalendar[] calendars = eventStore.GetCalendars(EKEntityType.Event);
            foreach (EKCalendar calendar in calendars)
            {
                Console.WriteLine(calendar);
            }
            CreateEvent();
        }
        void CreateEvent()
        {
            //protected CreateEventEditViewDelegate eventControllerDelegate;

            EventKitUI.EKEventEditViewController eventController = new EventKitUI.EKEventEditViewController();

            // set the controller's event store - it needs to know where/how to save the event
            eventController.EventStore = AppEvent.CurrentEvent.EventStore;
            EKEvent newEvent = EKEvent.FromStore(AppEvent.CurrentEvent.EventStore);
            eventController.Event = newEvent;

            // set the alarm for 10 minutes from now
            newEvent.AddAlarm(EKAlarm.FromDate((NSDate)DateTime.Now.AddMinutes(10)));
            // make the event start 20 minutes from now and last 30 minutes
            newEvent.StartDate = (NSDate)DateTime.Now.AddMinutes(20);
            newEvent.EndDate = (NSDate) DateTime.Now.AddMinutes(50);
            newEvent.Title = "Get outside and exercise!";
            newEvent.Notes = "This is your reminder to go and exercise for 30 minutes.";

        }

    }

}