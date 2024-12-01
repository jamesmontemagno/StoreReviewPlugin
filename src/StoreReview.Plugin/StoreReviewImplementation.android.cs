using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Microsoft.Maui.ApplicationModel;
using Plugin.StoreReview.Abstractions;
using Xamarin.Google.Android.Play.Core.Review;
using Xamarin.Google.Android.Play.Core.Review.Testing;


namespace Plugin.StoreReview;

public class StoreReviewImplementation : Java.Lang.Object, IStoreReview, IOnCompleteListener
{
    /// <summary>
    /// Opens the store listing.
    /// </summary>
    /// <param name="appId">App identifier.</param>
    public Task<bool> OpenStoreListing(string appId) =>
            OpenStoreReviewPage(appId);

    static Intent GetRateIntent(string url)
    {
        var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));

        intent.AddFlags(ActivityFlags.NoHistory);
        intent.AddFlags(ActivityFlags.MultipleTask);
        if ((int)Build.VERSION.SdkInt >= 21)
        {
            intent.AddFlags(ActivityFlags.NewDocument);
        }
        else
        {
            intent.AddFlags(ActivityFlags.ClearWhenTaskReset);
        }
        intent.SetFlags(ActivityFlags.ClearTop);
        intent.SetFlags(ActivityFlags.NewTask);
        return intent;
    }

    /// <summary>
    /// Opens the store review page.
    /// </summary>
    /// <param name="appId">App identifier.</param>
    public Task<bool> OpenStoreReviewPage(string appId)
    {
        var url = $"market://details?id={appId}";
        try
        {
            var intent = GetRateIntent(url);
            Application.Context.StartActivity(intent);
            return System.Threading.Tasks.Task.FromResult(true);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Unable to launch app store: " + ex.Message);
        }

        url = $"https://play.google.com/store/apps/details?id={appId}";
        try
        {
            var intent = GetRateIntent(url);
            Application.Context.StartActivity(intent);
            return System.Threading.Tasks.Task.FromResult(true);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Unable to launch app store: " + ex.Message);
        }
        return System.Threading.Tasks.Task.FromResult(false);
    }

    IReviewManager? manager;
    TaskCompletionSource<bool>? tcs;
    /// <summary>
    /// Requests an app review.
    /// </summary>
    public async Task<ReviewStatus> RequestReview(bool testMode)
    {
        tcs?.TrySetCanceled();
        tcs = new TaskCompletionSource<bool>();

        if (testMode)
            manager = new FakeReviewManager(Application.Context);
        else
            manager = ReviewManagerFactory.Create(Application.Context);

        forceReturn = false;
        var request = manager.RequestReviewFlow();
        request.AddOnCompleteListener(this);
        var status = await tcs.Task;
        manager.Dispose();
        request.Dispose();

        return status ? ReviewStatus.Succeeded : ReviewStatus.Error;
    }

    Activity Activity =>
        Platform.CurrentActivity ?? throw new NullReferenceException("Current Activity is null, ensure that .NET MAUI is configured for Essentials.");

    bool forceReturn;
    Android.Gms.Tasks.Task? launchTask;
    public void OnComplete(Android.Gms.Tasks.Task task)
    {
        if (!task.IsSuccessful || forceReturn)
        {
            tcs?.TrySetResult(forceReturn);
            launchTask?.Dispose();
            return;
        }

        try
        {
            if (task.GetResult(Java.Lang.Class.FromType(typeof(ReviewInfo))) is not ReviewInfo reviewInfo)
            {
                tcs?.TrySetResult(false);
                return;
            }

            forceReturn = true;
            launchTask = manager?.LaunchReviewFlow(Activity, reviewInfo);
            launchTask?.AddOnCompleteListener(this);
        }
        catch (Exception ex)
        {
            tcs?.TrySetResult(false);
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
}