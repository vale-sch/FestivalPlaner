using System;
using Plugin.LocalNotification;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android;
using Android.Content;

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
        protected override void OnStart()
        {
            base.OnStart();
            if((int) Build.VERSION.SdkInt >= 23)
            {
                if(CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
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
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            NotificationCenter.NotifyNotificationTapped(Intent);
            LoadApplication(new App());
        }
        protected override void OnNewIntent(Intent intent)
        {
            NotificationCenter.NotifyNotificationTapped(intent);
            base.OnNewIntent(intent);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {

            if(requestCode == RequestedLocationId)
            {
                if((grantResults.Length == 1) && grantResults[0] == Permission.Granted)
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
    }
}