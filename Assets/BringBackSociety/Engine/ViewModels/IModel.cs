using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.ViewModels
{
  /// <summary> Base class for all UI models. </summary>
  internal interface IModel
  {
    /// <summary> Destroys the model. </summary>
    void Destroy();
  }
}