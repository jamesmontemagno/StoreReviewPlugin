using Plugin.StoreReview.Abstractions;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Services.Store;
using Windows.ApplicationModel.Store;
using System.Collections.Generic;
using Windows.System;

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
        public Task<bool> OpenStoreListing(string appId)
        {
            try
            {
                return OpenUrl($"ms-windows-store://pdp/?ProductId={appId}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to launch app store: " + ex.Message);
            }
            return Task.FromResult(false);
        }

        /// <summary>
        /// Opens the store review page.
        /// </summary>
        /// <param name="appId">App identifier.</param>
        public Task<bool> OpenStoreReviewPage(string appId)
        {
            try
            {
                return OpenUrl($"ms-windows-store://review/?ProductId={appId}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to launch app store: " + ex.Message);
            }
            return Task.FromResult(false);
        }

#if NET6_0_OR_GREATER
        public static object Window { get; set; }
#endif
        /// <summary>
        /// Requests an app review.
        /// </summary>
        public async Task<ReviewStatus> RequestReview(bool testMode)
        {
            try
            {

                var context = StoreContext.GetDefault();

#if NET6_0_OR_GREATER
                if(Window is null)
                    throw new NullReferenceException("WindowObject is null. Please set the WindowObject property before calling RequestReview.");
                    
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(Window);
                
                WinRT.Interop.InitializeWithWindow.Initialize(context, hwnd);
#endif

                var result = await context.RequestRateAndReviewAppAsync();
                return result.Status switch
                {
                    StoreRateAndReviewStatus.Succeeded => ReviewStatus.Succeeded,
                    StoreRateAndReviewStatus.CanceledByUser => ReviewStatus.CanceledByUser,
                    StoreRateAndReviewStatus.Error => ReviewStatus.Error,
                    StoreRateAndReviewStatus.NetworkError => ReviewStatus.NetworkError,
                    _ => ReviewStatus.Error,
                };
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                return ReviewStatus.Error;
            }
            finally
            {
#if NET6_0_OR_GREATER
                Window = null;
#endif
            }
        }

		Task<bool> OpenUrl(string url)
        {
            try
            {
                return Windows.System.Launcher.LaunchUriAsync(new Uri(url)).AsTask();
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to open store: " + ex.Message);
            }

            return Task.FromResult(false);
        }
    }
}
