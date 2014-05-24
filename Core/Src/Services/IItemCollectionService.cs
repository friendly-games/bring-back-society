using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BringBackSociety.Services
{
  /// <summary> Implements the collecting of items for a player. </summary>
  public interface IItemCollectionService
  {
    /// <summary> Have the service collect the given item for the designated actor. </summary>
    /// <param name="collectible"> The collectible. </param>
    void Collect(ICollectible collectible);
  }
}