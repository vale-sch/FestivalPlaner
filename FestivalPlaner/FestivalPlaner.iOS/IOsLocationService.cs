using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using FestivalPlaner.Messages;
using FestivalPlaner.Services;

namespace FestivalPlaner.iOS
{
	public class IOsLocationService
	{
		nint _taskId;
		CancellationTokenSource _cts;
		public bool isStarted = false;

		public async Task Start()
		{
			_cts = new CancellationTokenSource();
			_taskId = UIApplication.SharedApplication.BeginBackgroundTask("com.company.Festival", OnExpiration);

			try
			{
				isStarted = true;
				await GeoLocationService.Run(_cts.Token);

			}
			catch (OperationCanceledException)
			{
			}
			finally
			{
				if (_cts.IsCancellationRequested)
				{
					var message = new StopServiceMessage();
					Device.BeginInvokeOnMainThread(
						() => MessagingCenter.Send(message, "ServiceStopped")
					);
				}
			}

			var time = UIApplication.SharedApplication.BackgroundTimeRemaining;

			UIApplication.SharedApplication.EndBackgroundTask(_taskId);
		}

		public void Stop()
		{
			isStarted = false;
			_cts.Cancel();
		}

		void OnExpiration()
		{
			UIApplication.SharedApplication.EndBackgroundTask(_taskId);
		}
	}
}