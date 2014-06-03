using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.ViewModels
{
  /// <summary> Interface for an object which can have a model hosted in it. </summary>
  internal interface IModelHost<T>
    where T : IModel
  {
    /// <summary> Informs the host to use the given model when displaying its current item. </summary>
    /// <param name="model"> The model to use. </param>
    void Use(T model);

    /// <summary> The current model being used in the host. </summary>
    T CurrentModel { get; }
  }
}