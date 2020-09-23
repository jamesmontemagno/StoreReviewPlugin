using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Com.Google.Android.Play.Core.Review;
using Com.Google.Android.Play.Core.Review.Testing;
using Com.Google.Android.Play.Core.Tasks;
using Plugin.StoreReview.Abstractions;
using System;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Plugin.StoreReview
{
	/// <summary>
	/// Implementation for Feature
	/// </summary>
	[Preserve(AllMembers = true)]
	public class StoreReviewImplementation : Java.Lang.Object, IStoreReview, IOnCompleteListener
    {
        /// <summary>
        /// Opens the store listing.
        /// </summary>
        /// <param name="appId">App identifier.</param>
        public void OpenStoreListing(string appId) => 
			OpenStoreReviewPage(appId);
        

        Intent GetRateIntent(string url)
        {
            var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));

            intent.AddFlags(ActivityFlags.NoHistory);
            intent.AddFlags(ActivityFlags.MultipleTask);
            if((int)Build.VERSION.SdkInt >= 21)
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
        public void OpenStoreReviewPage(string appId)
        {
            var url = $"market://details?id={appId}";
            try
            {
                var intent = GetRateIntent(url);
                Application.Context.StartActivity(intent);
                return;
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to launch app store: " + ex.Message);
            }
        }

		IReviewManager manager;
		TaskCompletionSource<bool> tcs;
		/// <summary>
		/// Requests an app review.
		/// </summary>
		public async Task RequestReview(bool testMode)
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
			await tcs.Task;
			manager.Dispose();
            request.Dispose();
        }

		Activity Activity =>
			Xamarin.Essentials.Platform.CurrentActivity ?? throw new NullReferenceException("Current Activity is null, ensure that the MainActivity.cs file is configuring Xamarin.Essentials in your source code so the In App Billing can use it.");

		bool forceReturn;
        Com.Google.Android.Play.Core.Tasks.Task launchTask;
        public void OnComplete(Com.Google.Android.Play.Core.Tasks.Task task)
		{
			if (!task.IsSuccessful || forceReturn)
			{
				tcs.TrySetResult(forceReturn);
                launchTask?.Dispose();
                return;
			}

			try
			{
				var reviewInfo = (ReviewInfo)task.GetResult(Java.Lang.Class.FromType(typeof(ReviewInfo)));
				forceReturn = true;
                launchTask = manager.LaunchReviewFlow(Activity, reviewInfo);
                launchTask.AddOnCompleteListener(this);
			}
			catch (Exception ex)
			{
				tcs.TrySetResult(false);
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}
	}
}