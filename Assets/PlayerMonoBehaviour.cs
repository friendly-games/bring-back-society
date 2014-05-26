using Behavior;
using Services;
using UnityEngine;
using System.Collections;

public class PlayerMonoBehaviour : MonoBehaviour, IObjectProvider<Player>
{
  Player IObjectProvider<Player>.Component
  {
    get { return new Player(this.gameObject); }
  }
}