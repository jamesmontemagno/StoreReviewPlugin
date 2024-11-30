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
        Task<bool> OpenStoreListing(string appId);

        /// <summary>
        /// Opens the store review page.
        /// </summary>
        /// <param name="appId">App identifier.</param>
        Task<bool> OpenStoreReviewPage(string appId);

        /// <summary>
        /// Requests an app review.
        /// </summary>
        Task<ReviewStatus> RequestReview(bool testMode);
    }
}
