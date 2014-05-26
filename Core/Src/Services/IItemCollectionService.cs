using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BringBackSociety.Controllers;

namespace BringBackSociety.Services
{
  /// <summary> Implements the collecting of items for a player. </summary>
  public interface IItemCollectionService
  {
    /// <summary> Have the service collect the given item for the designated actor. </summary>
    /// <param name="stack"> The collectible. </param>
    void Collect(InventoryStack stack);
  }
}