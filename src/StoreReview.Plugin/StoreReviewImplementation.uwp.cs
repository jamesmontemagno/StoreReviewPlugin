using Plugin.StoreReview.Abstractions;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Services.Store;

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
		public void RequestReview() => StoreRequestHelper.SendRequestAsync(StoreContext.GetDefault(), 16, string.Empty).WatchForError();

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

	internal static partial class PlatformExtensions
	{
		internal static void WatchForError(this IAsyncAction self) =>
			self.AsTask().WatchForError();

		internal static void WatchForError<T>(this IAsyncOperation<T> self) =>
			self.AsTask().WatchForError();

		internal static void WatchForError(this Task self)
		{
			var context = SynchronizationContext.Current;
			if (context == null)
				return;

			self.ContinueWith(
				t =>
				{
					var exception = t.Exception.InnerExceptions.Count > 1 ? t.Exception : t.Exception.InnerException;

					context.Post(e => { throw (Exception)e; }, exception);
				}, CancellationToken.None,
				TaskContinuationOptions.OnlyOnFaulted,
				TaskScheduler.Default);
		}
	}
}
