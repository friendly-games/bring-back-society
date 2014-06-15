using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Items
{
  /// <summary> A class whose state can be captured in a snapshot. </summary>
  internal interface ISnapshotable
  {
    /// <summary> A snapshot id of the current state. </summary>
    SnapshotToken SnapshotToken { get; }

    /// <summary> Force the item to create a new snapshot, from an external event. </summary>
    void ForceSnapshot();
  }
}