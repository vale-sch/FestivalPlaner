using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using ObjCRuntime;
using UIKit;
using Plugin.LocalNotification;


using Xamarin.Forms;

namespace FestivalPlaner.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            Xamarin.FormsMaps.Init();
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            return base.FinishedLaunching(app, options);
        }
        public async override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            // Check for new data, and display it
            
            await Services.GeoLocationService.GetCurrentLocation();

            // Inform system of fetch results
            completionHandler(UIBackgroundFetchResult.NewData);
        }
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            if (Services.GeoLocationService.newNearFestival)
            {
                completionHandler(UIBackgroundFetchResult.NewData);
            }
        }
     
         public override void WillEnterForeground(UIApplication uiApplication)
         {
             Plugin.LocalNotification.NotificationCenter.ResetApplicationIconBadgeNumber(uiApplication);
         }
    }
}
