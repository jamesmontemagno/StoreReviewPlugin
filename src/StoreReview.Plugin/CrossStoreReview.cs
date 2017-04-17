using Plugin.StoreReview.Abstractions;
using System;

namespace Plugin.StoreReview
{
  /// <summary>
  /// Cross platform StoreReview implemenations
  /// </summary>
  public class CrossStoreReview
  {
    static Lazy<IStoreReview> Implementation = new Lazy<IStoreReview>(() => CreateStoreReview(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static IStoreReview Current
    {
      get
      {
        var ret = Implementation.Value;
        if (ret == null)
        {
          throw NotImplementedInReferenceAssembly();
        }
        return ret;
      }
    }

    static IStoreReview CreateStoreReview()
    {
#if PORTABLE
        return null;
#else
        return new StoreReviewImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
