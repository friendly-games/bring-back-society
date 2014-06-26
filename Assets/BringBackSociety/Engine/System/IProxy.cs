using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> An object that needs to be released after its done being used. </summary>
  internal interface IProxy : IDisposable
  {
    
  }
}