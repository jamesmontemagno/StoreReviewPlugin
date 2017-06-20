using Plugin.StoreReview.Abstractions;
using System;
using System.Diagnostics;

namespace Plugin.StoreReview
{
    /// <summary>
    /// Implementation for StoreReview
    /// </summary>
    public class StoreReviewImplementation : IStoreReview
    {
        /// <summary>
        /// Opens the store listing.
        /// </summary>
        /// <param name="appId">App identifier.</param>
        public void OpenStoreListing(string appId) =>
            OpenUrl($"ms-windows-store://pdp/?ProductId={appId}");

        /// <summary>
        /// Opens the store review page.
        /// </summary>
        /// <param name="appId">App identifier.</param>
        public void OpenStoreReviewPage(string appId) =>
            OpenUrl($"ms-windows-store://review/?ProductId={appId}");

        /// <summary>
        /// Requests an app review.
        /// </summary>
        public void RequestReview()
        {
        }

        void OpenUrl(string url)
        {
            try
            {
                Windows.System.Launcher.LaunchUriAsync(new Uri(url)).AsTask().ContinueWith(success =>
                {
                    Debug.WriteLine("Opened up windows store");
                });
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to open store: " + ex.Message);
            }
        }
    }
}
