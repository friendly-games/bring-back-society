using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BringBackSociety.Items;
using Xunit;

namespace Tests.Weapons
{
  public class FireableWeaponStatsTests
  {
    [Fact]
    public void DamageAtHalf()
    {
      var stats = new FireableWeaponStats()
                  {
                    DamagePerShot = 10,
                    MaxDistance = 100,
                    FalloffDistance = 50
                  };

      Assert.Equal(5.0f, stats.CalculateDamage(75), 3);
    }

    [Fact]
    [Description("Damage at full")]
    public void Damage_at_full()
    {
      var stats = new FireableWeaponStats()
                  {
                    DamagePerShot = 10,
                    MaxDistance = 100,
                    FalloffDistance = 50
                  };

      Assert.Equal(10f, stats.CalculateDamage(50), 3);
    }

    [Fact]
    [Description("No damage at end")]
    public void No_damage_at_end()
    {
      var stats = new FireableWeaponStats()
                  {
                    DamagePerShot = 10,
                    MaxDistance = 100,
                    FalloffDistance = 50
                  };

      Assert.Equal(0, stats.CalculateDamage(100), 3);
    }
  }
}