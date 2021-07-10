## Store Review Plugin for Xamarin

**Platform Support**

|Platform|Version|
| ------------------- | :------------------: |
|Xamarin.iOS|iOS 10.3+|
|Xamarin.tvOS|All|
|Xamarin.Android|API 21+|
|UWP|API 10+|
|macOS|All|


### Build Status
![](https://jamesmontemagno.visualstudio.com/_apis/public/build/definitions/6b79a378-ddd6-4e31-98ac-a12fcd68644c/12/badge)

### NuGet
[![NuGet](https://img.shields.io/nuget/vpre/Xamarin.Essentials.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.StoreReview/)

### API

#### Open store listing
This will open the related app store for your app identifier.
iOS: This is found on your iTunes connect page for your app
Android: This is your package Id from your Android Manifest.
UWP:  This is the Store ID: You can find the link to your app's Store listing on the App identity page, in the App management section of each app in your dashboard.

```csharp
/// <summary>
/// Opens the store listing.
/// </summary>
/// <param name="appId">App identifier.</param>
void OpenStoreListing(string appId);
```

#### Open to Review Page
Launches app directly to Review Page if possible

```csharp
/// <summary>
/// Opens the store review page.
/// </summary>
/// <param name="appId">App identifier.</param>
void OpenStoreReviewPage(string appId);
```

#### Request In-App Review
UWP (all versions), Android and iOS only to prompt for the user to review the app. Only on Android 5.0 (API level 21) & iOS 10.3+ devices:

Read for iOS: [Requesting Reviews with iOS 10.3’s SKStoreReviewController](https://devblogs.microsoft.com/xamarin/requesting-reviews-ios-10-3s-skstorereviewcontroller/?WT.mc_id=friends-0000-jamont)

Read for Android: [In-app reviews for your Android apps](https://devblogs.microsoft.com/xamarin/android-in-app-reviews/?WT.mc_id=friends-0000-jamont)


```csharp
/// <summary>
/// Requests the review.
/// </summary>
void RequestReview();
```

### Android setup

Ensure that you follow the [Xamarin.Essentials setup steps](https://docs.microsoft.com/xamarin/essentials/get-started?WT.mc_id=friends-0000-jamont). And follow the steps below if you linker behavior is not set to `Don't Link`.

#### Android code shrinker (Proguard & r8)

If you use the plugin with `Link SDK assemblies only`/`Link all`, you have to do the following:

1. Create a `proguard.txt` file in your android project and add the following:

```
    -keep class com.google.android.play.core.common.PlayCoreDialogWrapperActivity
    -keep class com.google.android.play.core.review.** { *; }
    -keep class com.google.android.play.core.tasks.** { *; }
```

2. Include it to your project
3. Properties > Build Action > ProguardConfiguration
4. Go to you Android project options and set your `Code Shrinker` to `ProGuard` or `r8`

### Testing & Debugging issues

#### iOS

* You cannot submit a review on iOS while developing, but the review popup dialog displays in your simulator/device.
* However, when you download the app from Testflight, the popup dialog does not display at all, as [mentioned here](https://developer.apple.com/documentation/storekit/skstorereviewcontroller/2851536-requestreview):
> When you call this method while your app is still in development mode, a rating/review request view is always displayed so that you can test the user interface and experience. However, this method has no effect when you call it in an app that you distribute using TestFlight."

#### Android

* Unlike iOS, you cannot see the review popup dialog while developing or if you distribute it manually. As you can [see here](https://developer.android.com/guide/playcore/in-app-review/test), you have to download the app from the Play Store to see the popup. I recommend using Android Play Store's [“Internal App Sharing”](https://play.google.com/console/about/internalappsharing/) feature to test.
* Occasionally, some devices may not show the popup at all as [seen here](https://github.com/jamesmontemagno/StoreReviewPlugin/pull/27#issuecomment-877410136). One way to test whether your device is affected by it, is by downloading [this game that uses v3.1 of this nuget, target SDK version 30, target framework v11.0](https://play.google.com/store/apps/details?id=com.tfp.numberbomb) and win the game once to see the popup. Additionally, you can debug the error using adb locat, as you can [see here](https://github.com/jamesmontemagno/StoreReviewPlugin/issues/26#issue-940942211)
* The [most common issue/crash type](https://github.com/jamesmontemagno/StoreReviewPlugin/issues/20) is that developers release the app in the release configuration but they only test in the debug configuration. They do not realize that they have set Linker behavior to `Link SDK assemblies only`/`Link all`, and did not follow the proguard steps mentioned above

#### License
Under MIT, see LICENSE file.

