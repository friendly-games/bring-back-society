using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BringBackSociety
{
  /// <summary> Represents a value indicating the number of times something has changed. </summary>
  public struct ChangeCount : IEquatable<ChangeCount>
  {
    private readonly int _changeCount;

    private ChangeCount(int changeCount)
    {
      _changeCount = changeCount;
    }

    /// <summary> Gets the next change counter in the series. </summary>
    /// <returns> A ChangeCount representing one more change. </returns>
    [Pure]
    public ChangeCount Next()
    {
      return new ChangeCount(_changeCount + 1);
    }

    /// <summary> Tests if this ChangeCount is considered equal to another. </summary>
    /// <param name="other"> The change count to compare to this object. </param>
    /// <returns> true if the objects are considered equal, false if they are not. </returns>
    public bool Equals(ChangeCount other)
    {
      return _changeCount == other._changeCount;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;

      return obj is ChangeCount && Equals((ChangeCount) obj);
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
    public static bool operator ==(ChangeCount left, ChangeCount right)
    {
      return left.Equals(right);
    }

    /// <summary> Inequality operator. </summary>
    /// <param name="left"> The left. </param>
    /// <param name="right"> The right. </param>
    /// <returns> True if they are in-equal, false otherwise. </returns>
    public static bool operator !=(ChangeCount left, ChangeCount right)
    {
      return !left.Equals(right);
    }
  }
}