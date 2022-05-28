using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Calendars;
using Plugin.Calendars.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FestivalPlaner.Droid
{
    public class CalendarService
    {
        private CalendarEvent newEvent = new CalendarEvent();
        private DateTime startTime;
        private DateTime endTime;

        public CalendarService(DateTime _startTime, DateTime _endTime, string _title, string _description)
        {

            newEvent = new CalendarEvent()
            {
                Name = _title,
                Description = _description,
                Start = _startTime,
                End = _endTime,
                Reminders = new List<CalendarEventReminder> { new CalendarEventReminder() }
            };
            startTime = _startTime;
            endTime = _endTime;
            Task.Run(async () =>
            {
                await CreateEvent();
            });

        }
        public async Task CreateEvent()
        {
            var calendars = await CrossCalendars.Current.GetCalendarsAsync();
            var myCalendar = calendars[0];
            await CrossCalendars.Current.AddOrUpdateEventAsync(myCalendar, newEvent);
        }

        public static async Task<bool> CheckIsDateIsAvailable(DateTime startTime, DateTime endTime)
        {
            bool isAvailable = false;
            var calendars = await CrossCalendars.Current.GetCalendarsAsync();
            var myCalendar = calendars[0];
            var possibleEvents = await CrossCalendars.Current.GetEventsAsync(myCalendar, startTime, endTime);
            if (possibleEvents.Count == 0)
                isAvailable =  true;
            return isAvailable;
        }
    }
}