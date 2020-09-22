using System.Threading.Tasks;

namespace Plugin.StoreReview.Abstractions
{
	/// <summary>
	/// Interface for StoreReview
	/// </summary>
	public interface IStoreReview
    {
        /// <summary>
        /// Opens the store listing.
        /// </summary>
        /// <param name="appId">App identifier.</param>
        void OpenStoreListing(string appId);

        /// <summary>
        /// Opens the store review page.
        /// </summary>
        /// <param name="appId">App identifier.</param>
        void OpenStoreReviewPage(string appId);

        /// <summary>
        /// Requests an app review.
        /// </summary>
        Task RequestReview(bool testMode);
    }
}
