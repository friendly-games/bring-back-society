using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;
using NUnit.Framework;

namespace Tests.Weapons
{
  public class FireableWeaponStatsTests
  {
    public void DamageAtHalf()
    {
      var stats = new FireableWeaponStats()
                  {
                    DamagePerShot = 10,
                    MaxDistance = 100,
                    FalloffDistance = 50
                  };

      Assert.AreEqual(5.0f, stats.CalculateDamage(75), 3);
    }

    [Test]
    [Description("Damage at full")]
    public void Damage_at_full()
    {
      var stats = new FireableWeaponStats()
                  {
                    DamagePerShot = 10,
                    MaxDistance = 100,
                    FalloffDistance = 50
                  };

      Assert.AreEqual(10f, stats.CalculateDamage(50), 3);
    }

    [Test]
    [Description("No damage at end")]
    public void No_damage_at_end()
    {
      var stats = new FireableWeaponStats()
                  {
                    DamagePerShot = 10,
                    MaxDistance = 100,
                    FalloffDistance = 50
                  };

      Assert.AreEqual(0, stats.CalculateDamage(100), 3);
    }
  }
}