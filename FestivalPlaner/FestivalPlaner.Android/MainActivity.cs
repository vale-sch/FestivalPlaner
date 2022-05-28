using System;
using Plugin.LocalNotification;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android;
using Android.Content;
using FestivalPlaner.Droid.Services;
using Xamarin.Forms;
using FestivalPlaner.Messages;
using System.Threading.Tasks;


namespace FestivalPlaner.Droid
{
    [Activity(Label = "FestivalPlaner", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int RequestedLocationId = 0;
        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };
        readonly string[] CalendarPermissions =
      {
            Manifest.Permission.ReadCalendar,
            Manifest.Permission.WriteCalendar
        };
        Intent serviceIntent;

        private const int RequestCode = 5469;

        protected override void OnStart()
        {

            base.OnStart();
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestedLocationId);
                }
                else
                {
                    // Permission already granted
                }

            }

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            NotificationCenter.CreateNotificationChannel();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            serviceIntent = new Intent(this, typeof(AndroidLocationService));

            SetServiceMethods();

            Xamarin.FormsMaps.Init(this, savedInstanceState);
            NotificationCenter.NotifyNotificationTapped(Intent);
            if (CheckSelfPermission(Manifest.Permission.ReadCalendar) != Permission.Granted)
            {
                RequestPermissions(CalendarPermissions, 1);
            }
            else
            {
                // Permission already granted
            }
            LoadApplication(new App());
         
        }
     

        protected override void OnNewIntent(Intent intent)
        {
            NotificationCenter.NotifyNotificationTapped(intent);
            base.OnNewIntent(intent);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {

            if (requestCode == RequestedLocationId)
            {
                if ((grantResults.Length == 1) && grantResults[0] == Permission.Granted)
                {
                    //permission granted
                }
                else
                {
                    // permission denied
                }

            }
            else
            {
                Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }


            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        [Obsolete]
        void SetServiceMethods()
        {
            MessagingCenter.Subscribe<StartServiceMessage>(this, "ServiceStarted", message =>
            {
                if (!IsServiceRunning(typeof(AndroidLocationService)))
                {
                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                    {
                        StartForegroundService(serviceIntent);
                    }
                    else
                    {
                        StartService(serviceIntent);
                    }
                }
            });

            MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStopped", message =>
            {
                if (IsServiceRunning(typeof(AndroidLocationService)))
                    StopService(serviceIntent);
            });
            MessagingCenter.Subscribe<CalendarMessage>(this, "CreateCalendar", message =>
            {
                new CalendarService(message.startTime, message.endTime, message.title, message.description);
            });
            MessagingCenter.Subscribe<DateCheckerMessage>(this, "DateCheckerMessage", message =>
            {
                Task.Run(async () =>
                {
                    message.isFree =  await CalendarService.CheckIsDateIsAvailable(message.startTime, message.endTime); 
                });
            });
        }

        [Obsolete]
        private bool IsServiceRunning(System.Type cls)
        {
            ActivityManager manager = (ActivityManager)GetSystemService(Context.ActivityService);
            foreach (var service in manager.GetRunningServices(int.MaxValue))
            {
                if (service.Service.ClassName.Equals(Java.Lang.Class.FromType(cls).CanonicalName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}