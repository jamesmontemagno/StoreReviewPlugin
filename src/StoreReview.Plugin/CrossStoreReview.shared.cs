using Plugin.StoreReview.Abstractions;
using System;

namespace Plugin.StoreReview
{
    public enum ReviewStatus
    {
        Succeeded,
        Error,
        CanceledByUser,
        NetworkError,
        Unknown
    }
	/// <summary>
	/// Cross platform StoreReview implemenations
	/// </summary>
	public class CrossStoreReview
	{
		static Lazy<IStoreReview> implementation = new Lazy<IStoreReview>(() => CreateStoreReview(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

		/// <summary>
		/// Gets if the plugin is supported on the current platform.
		/// </summary>
		public static bool IsSupported => implementation.Value == null ? false : true;

		/// <summary>
		/// Current plugin implementation to use
		/// </summary>
		public static IStoreReview Current
		{
			get
			{
				var ret = implementation.Value;
                return ret is null ? throw NotImplementedInReferenceAssembly() : ret;
            }
        }

		static IStoreReview CreateStoreReview()
		{
#if ANDROID || IOS || MACCATALYST || MACOS || WINDOWS
			return new StoreReviewImplementation();
#else 
            return null;
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
			new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
		
	}
}
