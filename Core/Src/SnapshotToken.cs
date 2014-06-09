using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BringBackSociety
{
  /// <summary> Represents a value indicating the number of times something has changed. </summary>
  public struct SnapshotToken : IEquatable<SnapshotToken>
  {
    private readonly int _changeCount;

    /// <summary> Constructor. </summary>
    /// <param name="changeCount"> The id of the current change count. </param>
    private SnapshotToken(int changeCount)
    {
      _changeCount = changeCount;
    }

    /// <summary> Gets the next change counter in the series. </summary>
    /// <returns> A ChangeCount representing one more change. </returns>
    [Pure]
    public SnapshotToken Next()
    {
      return new SnapshotToken(_changeCount + 1);
    }

    /// <summary> Tests if this ChangeCount is considered equal to another. </summary>
    /// <param name="other"> The change count to compare to this object. </param>
    /// <returns> true if the objects are considered equal, false if they are not. </returns>
    public bool Equals(SnapshotToken other)
    {
      return _changeCount == other._changeCount;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;

      return obj is SnapshotToken && Equals((SnapshotToken) obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      return _changeCount;
    }

    /// <summary> Equality operator. </summary>
    /// <param name="left"> The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> True if they are equal, false otherwise. </returns>
    public static bool operator ==(SnapshotToken left, SnapshotToken right)
    {
      return left.Equals(right);
    }

    /// <summary> Inequality operator. </summary>
    /// <param name="left"> The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> True if they are in-equal, false otherwise. </returns>
    public static bool operator !=(SnapshotToken left, SnapshotToken right)
    {
      return !left.Equals(right);
    }
  }
}