using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using ObjCRuntime;
using UIKit;
using Plugin.LocalNotification;


using Xamarin.Forms;
using CoreLocation;
using FestivalPlaner.Messages;

namespace FestivalPlaner.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        IOsLocationService locationService;
        readonly CLLocationManager locMgr = new CLLocationManager();
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            locationService = new IOsLocationService();
            SetServiceMethods();
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            //Background Location Permissions
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locMgr.RequestAlwaysAuthorization();
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                locMgr.AllowsBackgroundLocationUpdates = true;
            }

            return base.FinishedLaunching(app, options);
        }

        void SetServiceMethods()
        {
            MessagingCenter.Subscribe<StartServiceMessage>(this, "ServiceStarted", async message =>
            {
                if (!locationService.isStarted)
                    await locationService.Start();
            });

            MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStopped", message =>
            {
                if (locationService.isStarted)
                    locationService.Stop();
            });
        }

        public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            try
            {
                completionHandler(UIBackgroundFetchResult.NewData);
            }
            catch (Exception)
            {
                completionHandler(UIBackgroundFetchResult.NoData);
            }
        }
    }
}
