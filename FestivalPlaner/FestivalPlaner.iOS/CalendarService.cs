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
        private EKCalendar eKCalendar = null;
        private DateTime startTime;
        private DateTime endTime;
        public EKEvent newEvent;
        public CalendarService(DateTime _startTime, DateTime _endTime, string _title, string _description)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                newEvent = EKEvent.FromStore(eventStore);

                newEvent.AddAlarm(EKAlarm.FromDate((NSDate)(_startTime - TimeSpan.FromDays(1))));
                newEvent.StartDate = (NSDate)_startTime;
                newEvent.EndDate = (NSDate)_endTime;
                newEvent.Title = _title;
                newEvent.Notes = _description;
                newEvent.Availability = EKEventAvailability.Busy;
            });
            startTime = _startTime;
            endTime = _endTime;
            CreateEvent();
        }

        public static bool CheckIsDateIsAvailable(DateTime startTime, DateTime endTime)
        {
            bool isFree = false;
            var predicate = AppEvent.CurrentEvent.EventStore.PredicateForEvents((NSDate)startTime, (NSDate)endTime, AppEvent.CurrentEvent.EventStore.GetCalendars(EKEntityType.Event));
            var possibleEvents = AppEvent.CurrentEvent.EventStore.EventsMatching(predicate);
            if (possibleEvents.ToArray().Length > 0)
                foreach (EKEvent ev in possibleEvents.ToArray())
                {
                    if (ev.Calendar.Title != "Deutsche Feiertage")
                    {
                        Console.WriteLine(ev.Title);
                        isFree = false;
                        break;
                    }
                    else isFree = true;
                }
            else
                isFree = true;

            return isFree;
        }

        void CreateEvent()
        {

            EKCalendar[] calendars = eventStore.GetCalendars(EKEntityType.Event);
            foreach (EKCalendar calendar in calendars)
            {
                if (calendar.Title == "Calendar")
                {
                    eKCalendar = calendar;
                }
            }
            Device.BeginInvokeOnMainThread(() =>
            {

                newEvent.Calendar = eKCalendar;
                NSError e;
                eventStore.SaveEvent(newEvent, EKSpan.ThisEvent, out e);


            });

        }

    }
}

