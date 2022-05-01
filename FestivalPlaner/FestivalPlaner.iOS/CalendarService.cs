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
            });
            CreateEvent();
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
                var calendarEvents = eventStore.GetCalendarItems(eKCalendar.CalendarIdentifier);
               // NSError e;
               // eventStore.SaveEvent(newEvent, EKSpan.ThisEvent, out e);
            });

        }

    }
}

