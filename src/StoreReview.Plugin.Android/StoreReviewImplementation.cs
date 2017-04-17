using Android.App;
using Android.Content;
using Android.OS;
using Plugin.StoreReview.Abstractions;
using System;
using System.Diagnostics;

namespace Plugin.StoreReview
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class StoreReviewImplementation : IStoreReview
    {
        public void OpenStoreListing(string appId)
        {
            OpenStoreReviewPage(appId);
        }

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

            return intent;
        }

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

        public void RequestReview()
        {
        }
    }
}