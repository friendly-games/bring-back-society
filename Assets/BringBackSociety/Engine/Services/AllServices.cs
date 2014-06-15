using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BringBackSociety.Tasks;

namespace BringBackSociety.Services
{
  /// <summary> Contains all of the services in the game </summary>
  internal static class AllServices
  {
    /// <summary> The raycast implementation for the game. </summary>
    /// <remarks> UI thread only. </remarks>
    public static IRaycastService RaycastService { get; set; }

    /// <summary> UI thread only. </summary>
    public static CoroutineDispatcher Dispatcher { get; set; }

    /// <summary> SynchronizationContext for the UI thread. </summary>
    public static SynchronizationContext SynchronizationContext { get; set; }

    /// <summary> The collection services for the primary player. </summary>
    /// <remarks> At some point, this should be removed. </remarks>
    public static IItemCollectionService CollectionService { get; set; }

    /// <summary> The random number generator. </summary>
    public static IRandomNumberGenerator RandomNumberGenerator { get; set; }
  }
}