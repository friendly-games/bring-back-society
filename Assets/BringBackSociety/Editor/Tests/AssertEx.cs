using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.BringBackSociety.Editor.Tests
{
  public static class AssertEx
  {

    public static bool IsCloseTo(this float value, float other, float delta = 0.0001f)
    {
      return Math.Abs(value - other) < delta;
    }

  }
}
