using System;
using System.Collections.Generic;
using System.Linq;
using Assets.BringBackSociety.Editor.Tests;
using NUnit.Framework;
using Scripts;
using UnityEngine;

namespace Assets.BringBackSociety.Tests
{
  [Category("InUnity")]
  public class TargetLocatorTests
  {
    private InputManager.TargetLocator _locator;

    [SetUp]
    public void Setup()
    {
      _locator = new InputManager.TargetLocator(100, 100, 5, 10);
    }

    [Test]
    [Description("Creation works")]
    public void Creation_works()
    {
      _locator.UpdatePosition(new Vector2(50, 50));
    }

    [Test]
    [Description("Strength at 50 is zero")]
    public void Strength_at_50_is_zero()
    {
      _locator.UpdatePosition(new Vector2(50, 50));

      Assert.AreEqual(0, _locator.Strength);
    }

    [Test]
    [Description("Strength at 0 is 100")]
    public void Strength_at_0_is_100()
    {
      _locator.UpdatePosition(new Vector2(100, 100));

      Assert.AreEqual(100, _locator.Strength, 3);
    }

    [Test]
    [Description("Strength at 7 is 40")]
    public void Strength_at_7_is_40()
    {
      _locator.UpdatePosition(new Vector2(57, 50));
      Assert.AreEqual(40, _locator.Strength, 3);

      _locator.UpdatePosition(new Vector2(50, 57));
      Assert.AreEqual(40, _locator.Strength, 3);

      _locator.UpdatePosition(new Vector2(43, 50));
      Assert.AreEqual(40, _locator.Strength, 3);

      _locator.UpdatePosition(new Vector2(50, 43));
      Assert.AreEqual(40, _locator.Strength, 3);
    }

    [Test]
    [Description("Positive x faces right")]
    public void Positive_x_faces_right()
    {
      _locator.UpdatePosition(new Vector2(57, 50));

      AssertEqual(Quaternion.AngleAxis(90, Vector3.up), _locator.Direction);
    }

    [Test]
    [Description("Negative x faces left")]
    public void Negative_x_faces_left()
    {
      _locator.UpdatePosition(new Vector2(43, 50));

      AssertEqual(Quaternion.AngleAxis(270, Vector3.up), _locator.Direction);
    }

    [Test]
    [Description("Positive y faces back")]
    public void Positive_y_faces_back()
    {
      _locator.UpdatePosition(new Vector2(50, 57));

      AssertEqual(Quaternion.AngleAxis(0, Vector3.up), _locator.Direction);
    }

    [Test]
    [Description("Negative y faces front")]
    public void Negative_y_faces_front()
    {
      _locator.UpdatePosition(new Vector2(50, 43));

      AssertEqual(Quaternion.AngleAxis(180, Vector3.up), _locator.Direction);
    }

    public void AssertEqual(Quaternion expectedQuat, Quaternion actualQuat)
    {
      Vector3 expected = expectedQuat.eulerAngles;
      Vector3 actual = actualQuat.eulerAngles;

      Assert.IsTrue(expected.x.IsCloseTo(0) || expected.x.IsCloseTo(180), "X");
      Assert.AreEqual(expected.y, actual.y, 0.0001f, "Y");
      Assert.IsTrue(expected.z.IsCloseTo(0) || expected.z.IsCloseTo(180), "Z");
    }
  }
}