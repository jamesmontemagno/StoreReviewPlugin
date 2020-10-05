## Store Review Plugin for Xamarin

**Platform Support**

|Platform|Version|
| ------------------- | :------------------: |
|Xamarin.iOS|iOS 7+|
|Xamarin.tvOS|All|
|Xamarin.Android|API 10+|
|UWP|API 10+|


### Build Status
![](https://jamesmontemagno.visualstudio.com/_apis/public/build/definitions/6b79a378-ddd6-4e31-98ac-a12fcd68644c/12/badge?WT.mc_id=storereviewplugin-github-jamont)

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
UWP (all versions) and iOS only to prompt for the user to review the app. Only on iOS 10.3+ devices:
Read: https://blog.xamarin.com/requesting-reviews-ios-10-3s-skstorereviewcontroller/

```csharp
/// <summary>
/// Requests the review.
/// </summary>
void RequestReview();
```

#### License
Under MIT, see LICENSE file.

