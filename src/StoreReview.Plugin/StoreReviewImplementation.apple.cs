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
	[Preserve(AllMembers = true)]
	public class StoreReviewImplementation : IStoreReview
    {
        /// <summary>
        /// Opens the store listing.
        /// </summary>
        /// <param name="appId">App identifier.</param>
        public void OpenStoreListing(string appId)
        {
#if __IOS__
			var url = $"itms-apps://itunes.apple.com/app/id{appId}";
#elif __TVOS__
			var url = $"com.apple.TVAppStore://itunes.apple.com/app/id{appId}";
#endif
			try
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to launch app store: " + ex.Message);
            }
        }

        /// <summary>
        /// Opens the store review page.
        /// </summary>
        /// <param name="appId">App identifier.</param>
        public void OpenStoreReviewPage(string appId)
        {
#if __IOS__
            var url = $"itms-apps://itunes.apple.com/app/id{appId}?action=write-review";
#elif __TVOS__
			var url = $"com.apple.TVAppStore://itunes.apple.com/app/id{appId}?action=write-review";
#endif
			try
			{
                UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to launch app store: " + ex.Message);
            }
        }

        /// <summary>
        /// Requests an app review.
        /// </summary>
        public void RequestReview()
        {
#if __IOS__
            if (IsiOS103)
            {
                SKStoreReviewController.RequestReview();
            }
#endif
        }

        bool IsiOS103 => UIDevice.CurrentDevice.CheckSystemVersion(10, 3);
    

    }
}
