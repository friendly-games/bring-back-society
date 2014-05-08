using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety;
using BringBackSociety.Services;
using UnityEngine;

namespace Services
{
  /// <summary> Implementation of perlin noise for unity. </summary>
  public class PerlinNoise : IPerlinNoise
  {
    public long Seed { get; private set; }

    public float Noise(float x, float y)
    {
      return Mathf.PerlinNoise(x/Chunk.Length*10, y/Chunk.Length*10);
    }
  }
}