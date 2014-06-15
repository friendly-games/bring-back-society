using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Core;
using NUnit.Framework;
using UnityEngine;
using UnityTest;

namespace Assets.BringBackSociety.Editor.Tests
{
  public static class AssertEx
  {
    public static bool IsCloseTo(this float value, float other, float delta = 0.0001f)
    {
      return Math.Abs(value - other) < delta;
    }

    public static void AreEqual(Vector2 expected, Vector2 actual)
    {
      Assert.AreEqual(expected.x, actual.x, 0.0001f, "X");
      Assert.AreEqual(expected.y, actual.y, 0.0001f, "Y");
    }
  }
}