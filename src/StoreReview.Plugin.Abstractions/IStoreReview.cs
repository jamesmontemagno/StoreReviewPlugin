using System;
using System.Threading.Tasks;

namespace Plugin.StoreReview.Abstractions
{
    /// <summary>
    /// Interface for StoreReview
    /// </summary>
    public interface IStoreReview
    {

        void OpenStoreListing(string appId);
        void OpenStoreReviewPage(string appId);
        void RequestReview();
    }
}
