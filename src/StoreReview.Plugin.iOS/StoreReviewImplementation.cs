using Foundation;
using Plugin.StoreReview.Abstractions;

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UIKit;
using StoreKit;

namespace Plugin.StoreReview
{
    /// <summary>
    /// Implementation for StoreReview
    /// </summary>
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class StoreReviewImplementation : IStoreReview
    {
     
        public void OpenStoreListing(string appId)
        {
            var url = $"itms-apps://itunes.apple.com/app/id{appId}";
            try
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to launch app store: " + ex.Message);
            }
        }

        public void OpenStoreReviewPage(string appId)
        {
            var url = $"itms-apps://itunes.apple.com/app/id{appId}?action=write-review";
            try
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to launch app store: " + ex.Message);
            }
        }

        public void RequestReview()
        {
            if (IsiOS103)
            {
                SKStoreReviewController.RequestReview();
            }
        }

        bool IsiOS103 => UIDevice.CurrentDevice.CheckSystemVersion(10, 3);
        bool IsiOS8 => UIDevice.CurrentDevice.CheckSystemVersion(8, 0);
        bool IsiOS7 => UIDevice.CurrentDevice.CheckSystemVersion(7, 0);

    }
}
