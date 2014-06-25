using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> Represents the number of items that can be stacked. </summary>
  internal class StackableData : SimpleComponent<IStackable>, IStackable
  {
    /// <summary> Constructor. </summary>
    /// <param name="stackAmount"> The number of items that can be stacked together. </param>
    public StackableData(int stackAmount)
    {
      if (stackAmount <= 1)
        throw new ArgumentException("Amount must be >= 2", "stackAmount");

      StackAmount = stackAmount;
    }

    /// <inheritdoc />
    public int StackAmount { get; private set; }
  }
}