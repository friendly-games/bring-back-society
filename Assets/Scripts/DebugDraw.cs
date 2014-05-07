using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BringBackSociety;
using BringBackSociety.Chunks.Loaders;
using BringBackSociety.Services;
using UnityEngine;
using System.Collections;
using Debug = UnityEngine.Debug;

public class DebugDraw : MonoBehaviour, IStartAndUpdate
{
  private LinkedList<Ray[]> _raysToDraw;
  private static DebugDraw Instance;

  private class PerlinNoise : IPerlinNoise
  {
    public float Noise(float x, float y)
    {
      return Mathf.PerlinNoise(x/Chunk.Length*10, y/Chunk.Length*10);
    }

    public long Seed { get; private set; }
  }

  /// <inheritdoc />
  public void Start()
  {
    _raysToDraw = new LinkedList<Ray[]>();
    Instance = this;

    World = new World(new PerlinChunkLoader(new PerlinNoise()));

    int i = 0;

    var wall = Prefabs.GetPrefab("Wall");
    var allWalls = GameObject.Find("All.Walls");

    for (int x = 0; x < Chunk.Length; x++)
    {
      for (int z = 0; z < Chunk.Length; z++)
      {
        var coordinate = new TileCoordinate(x, z);
        var tile = World.CenterNode.Chunk.Tiles[coordinate.Index];

        if (tile.GroundType > 128)
        {
          var newWall = wall.Clone(new Vector3(x, wall.transform.localScale.y/2, z)/2);
          newWall.transform.parent = allWalls.transform;
        }
      }
    }

    foreach (var tile in World.CenterNode.Chunk.Tiles)
    {
    }
  }

  public World World { get; set; }

  /// <inheritdoc />
  public void Update()
  {
    foreach (var rays in _raysToDraw)
    {
      foreach (var ray in rays)
      {
        Debug.DrawRay(ray.origin, ray.direction);
      }
    }
  }

  /// <summary> Draws the given ray. </summary>
  /// <param name="ray"> The ray to draw. </param>
  public static void Draw(Ray ray)
  {
    Debug.Log("Draw");
    Draw(new[] {ray});
  }

  /// <summary> Draws the given ray. </summary>
  /// <param name="ray"> The ray to draw. </param>
  public static void Draw(Ray[] rays)
  {
    Instance.StartCoroutine(Instance.DrawCoroutine(rays));
  }

  private IEnumerator DrawCoroutine(Ray[] rays)
  {
    _raysToDraw.AddLast(rays);
    yield return new WaitForSeconds(1);
    _raysToDraw.Remove(rays);
  }
}