using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary> An object that initializes and is called on every frame. </summary>
internal interface IStartAndUpdate
{
  void Start();

  void Update();
}