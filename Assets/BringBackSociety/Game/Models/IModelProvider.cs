﻿using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;

namespace Models
{
  /// <summary> Interface for a mono-behavior that provides an actual implementation for a model. </summary>
  /// <typeparam name="T"> Generic type parameter. </typeparam>
  public interface IModelProvider<T> : IComponent
  {
    T ModelImplementation { get; }
  }
}