## Store Review Plugin for Xamarin

**Platform Support**

|Platform|Version|
| ------------------- | :------------------: |
|Xamarin.iOS|iOS 7+|
|Xamarin.tvOS|All|
|Xamarin.Android|API 10+|
|UWP|API 10+|


### Build Status
![](https://jamesmontemagno.visualstudio.com/_apis/public/build/definitions/6b79a378-ddd6-4e31-98ac-a12fcd68644c/12/badge)

### NuGet
https://www.nuget.org/packages/Plugin.StoreReview/

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

#### Request Review
UWP (all versions), Android and iOS only to prompt for the user to review the app. Only on Android 5.0 (API level 21) & iOS 10.3+ devices:
Read: https://blog.xamarin.com/requesting-reviews-ios-10-3s-skstorereviewcontroller/

```csharp
/// <summary>
/// Requests the review.
/// </summary>
void RequestReview();
```

### Android setup

You need to add the following line to your OnCreate method in the main activity class:
```csharp
Xamarin.Essentials.Platform.Init(this, bundle);
```

It should look like this:
```csharp
protected override void OnCreate(Bundle savedInstanceState)
{
	// Rest of method omitted for simplicity
	
	Xamarin.Essentials.Platform.Init(this, savedInstanceState);
	global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
	LoadApplication(new App());
}
```

### Android code shrinker (Proguard & r8)

If you use the plugin with Link all, Release Mode and ProGuard/r8 enabled, you have to do the following:

1. Create a `proguard.txt` file in your android project and add the following:

```
    -keep class com.google.android.play.core.common.PlayCoreDialogWrapperActivity
    -keep class com.google.android.play.core.review.** { *; }
    -keep class com.google.android.play.core.tasks.** { *; }
```

2. Include it to your project
3. Properties > Build Action > ProguardConfiguration

#### License
Under MIT, see LICENSE file.

