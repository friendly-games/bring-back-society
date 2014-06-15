using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Assets.BringBackSociety.Editor.Tests
{
  internal class Fixture
  {
    [TestFixtureSetUp]
    public void SetupFixture()
    {
      GlobalSettings.DefaultFloatingPointTolerance = 0.0001;
    }
  }
}