using Foundation;
using Plugin.StoreReview.Abstractions;

using System;
using System.Diagnostics;
using System.Threading.Tasks;
#if !__MACOS__
using UIKit;
using System.Linq;
#endif
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
#elif __MACOS__
			var url = $"macappstore://itunes.apple.com/app/id{appId}?mt=12";
#endif
			try
            {
#if __MACOS__
				AppKit.NSWorkspace.SharedWorkspace.OpenUrl(new NSUrl(url));
#else
				UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
#endif
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
#elif __MACOS__
			var url = $"macappstore://itunes.apple.com/app/id{appId}?action=write-review";
#endif
			try
			{
#if __MACOS__
				AppKit.NSWorkspace.SharedWorkspace.OpenUrl(new NSUrl(url));
#else
				UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
#endif
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to launch app store: " + ex.Message);
            }
        }

        /// <summary>
        /// Requests an app review.
        /// </summary>
        public Task<ReviewStatus> RequestReview(bool testMode)
		{
#if __IOS__
            if (IsiOS103)
            {
				if (IsiOS14)
				{
                	var windowScene = UIApplication.SharedApplication?.ConnectedScenes?.ToArray<UIScene>()?.FirstOrDefault(x => x.ActivationState == UISceneActivationState.ForegroundActive) as UIWindowScene;
					if (windowScene != null)
					{
						SKStoreReviewController.RequestReview(windowScene);
						return Task.FromResult(ReviewStatus.Unknown);
					}
				}
                SKStoreReviewController.RequestReview();
            }
#elif __MACOS__
			using var info = new NSProcessInfo();
			if (ParseVersion(info.OperatingSystemVersion.ToString()) >= new Version(10, 14))
			{
				SKStoreReviewController.RequestReview();
			}
#endif

            return Task.FromResult(ReviewStatus.Unknown);
        }

		internal static Version ParseVersion(string version)
		{
			if (Version.TryParse(version, out var number))
				return number;

			if (int.TryParse(version, out var major))
				return new Version(major, 0);

			return new Version(0, 0);
		}

#if __IOS__
		bool IsiOS103 => UIDevice.CurrentDevice.CheckSystemVersion(10, 3);
		bool IsiOS14 => UIDevice.CurrentDevice.CheckSystemVersion(14, 0);
#endif

	}
}
