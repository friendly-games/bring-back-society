using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Items
{
  /// <summary> Interface an object that can be cloned. </summary>
  internal interface ICopyable
  {
    /// <summary> Makes a deep copy of this object. </summary>
    /// <returns> A copy of this object. </returns>
    object Copy();
  }

  /// <summary> Interface an object that can be cloned. </summary>
  /// <typeparam name="T"> The type of object to be cloned. </typeparam>
  internal interface ICopyable<T> : ICopyable
  {
    /// <summary> Makes a deep copy of this object. </summary>
    /// <returns> A copy of this object. </returns>
    new T Copy();
  }
}