using System;
using System.Collections.Generic;
using System.Text;
namespace FestivalPlaner.Messages
{
    public class CalendarMessage
    {
        public DateTime startTime { get; }
        public DateTime endTime { get; }
        public string title { get; }
        public string description { get; }

        public CalendarMessage( DateTime _startTime, DateTime _endTime, string _title, string _description)
        {
            startTime = _startTime; 
            endTime = _endTime; 
            title = _title; 
            description = _description; 
        }
    }
}
