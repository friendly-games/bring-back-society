using System;
using System.Collections.Generic;
using System.Linq;

/// <summary> An object that initializes and is called on every frame. </summary>
internal interface IStartAndUpdate : IStart
{
  /// <summary> Invoked on every frame of the game. </summary>
  void Update();
}