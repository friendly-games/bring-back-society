using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;

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

  /// <summary> Extension methods for  IModelHost. </summary>
  internal static class ModelHostExtensions
  {
    /// <summary>
    ///  Destroy the current model and use a copy of <paramref name="model"/> instead.
    /// </summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    /// <param name="host"> The host of the model. </param>
    /// <param name="model"> The model to use instead of the CurrentModel. </param>
    public static void SwitchToCopyOf<T>(this IModelHost<T> host, T model)
      where T : IModel, ICopyable<T>
    {
      var currentModel = host.CurrentModel;

      if (currentModel != null)
      {
        currentModel.Destroy();
      }

      host.Use(model.Copy());
    }
  }
}