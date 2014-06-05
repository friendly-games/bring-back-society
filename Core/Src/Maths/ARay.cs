using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BringBackSociety.Maths
{
  /// <summary> Represents a position and a direction. </summary>
  public struct ARay
  {
    /// <summary> The starting position of the ray. </summary>
    public AVector3 Position;

    /// <summary> The direction of the ray. </summary>
    public AVector3 Direction;

    /// <summary> Constructor. </summary>
    /// <param name="position"> The starting position of the ray. </param>
    /// <param name="direction"> The direction of the ray. </param>
    public ARay(AVector3 position, AVector3 direction)
    {
      Position = position;
      Direction = direction;
    }

    /// <inheritdoc />
    public override string ToString()
    {
      return "Position: " + Position + ", Orientation: " + Direction;
    }
  }
}