using System.Diagnostics;
using Plugin.StoreReview.Abstractions;
using StoreKit;

namespace Plugin.StoreReview;

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
    public Task<bool> OpenStoreListing(string appId)
    {

        var url = $"itms-apps://itunes.apple.com/app/id{appId}";

        try
        {
            return UIApplication.SharedApplication.OpenUrlAsync(new NSUrl(url), new UIApplicationOpenUrlOptions());
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
        var url = $"itms-apps://itunes.apple.com/app/id{appId}?action=write-review";

        try
        {
            return UIApplication.SharedApplication.OpenUrlAsync(new NSUrl(url), new UIApplicationOpenUrlOptions());
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Unable to launch app store: " + ex.Message);
        }

        return Task.FromResult(false);
    }

    /// <summary>
    /// Requests an app review.
    /// </summary>
    public Task<ReviewStatus> RequestReview(bool testMode)
    {
        if (IsiOS103)
        {
            if (IsiOS14)
            {
                var windowScene = UIApplication.SharedApplication?.ConnectedScenes?.ToArray<UIScene>()?.FirstOrDefault(x => x.ActivationState == UISceneActivationState.ForegroundActive) as UIWindowScene;
                if (windowScene is not null)
                {
#pragma warning disable CA1422 // Validate platform compatibility
                    SKStoreReviewController.RequestReview(windowScene);
#pragma warning restore CA1422 // Validate platform compatibility
                    return Task.FromResult(ReviewStatus.Unknown);
                }
            }
#pragma warning disable CA1422 // Validate platform compatibility
            SKStoreReviewController.RequestReview();
#pragma warning restore CA1422 // Validate platform compatibility
        }
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

    static bool IsiOS103 => UIDevice.CurrentDevice.CheckSystemVersion(10, 3);
    static bool IsiOS14 => UIDevice.CurrentDevice.CheckSystemVersion(14, 0);
}
