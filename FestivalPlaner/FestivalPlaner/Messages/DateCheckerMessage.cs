using System;
namespace FestivalPlaner.Messages
{
	public class DateCheckerMessage
	{
		public bool isFree { get; set; }
		public DateTime startTime { get; }
		public DateTime endTime { get; }
		public DateCheckerMessage(DateTime _startTime, DateTime _endTime)
		{
			startTime = _startTime;
			endTime = _endTime;
		}
	}
}

