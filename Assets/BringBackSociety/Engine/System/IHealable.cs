using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  internal interface IHealable : IComponent, IRequiresComponent<IHealth>
  {
    /// <summary> The maximum health of the destroyable. </summary>
    int MaxHealth { get; }
  }
}