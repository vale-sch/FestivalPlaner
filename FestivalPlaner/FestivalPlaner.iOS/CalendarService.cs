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

        }
        void GetCalendars()
        {
            EKCalendar[] calendars = eventStore.GetCalendars(EKEntityType.Event);
            foreach (EKCalendar calendar in calendars)
            {
                Console.WriteLine(calendar);
            }
        }
    }
}