using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BringBackSociety.Tasks;

namespace BringBackSociety.Services
{
  /// <summary> Contains all of the services in the game </summary>
  public static class AllServices
  {
    /// <summary> The raycast implementation for the game. </summary>
    /// <remarks> UI thread only. </remarks>
    public static IRaycastService RaycastService { get; set; }

    /// <summary> UI thread only. </summary>
    public static CoroutineDispatcher Dispatcher { get; set; }
  }
}