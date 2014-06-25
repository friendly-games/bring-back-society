using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> An entity that contains one or more components. </summary>
  public interface IDatity
  {
    /// <summary> Puts a component of type T into the entity.</summary>
    /// <typeparam name="T"> The type of entity to add to the entity. </typeparam>
    /// <param name="component"> The component to add. </param>
    void Put<T>(T component)
      where T : class, IComponent;

    /// <summary> Retrieves a component of type T from the entity. </summary>
    /// <typeparam name="T"> The type of entity to retrieve from the entity. </typeparam>
    /// <returns>
    ///  The component found in the entity, or null if the entity does not exist in the entity.
    /// </returns>
    T Get<T>()
      where T : class, IComponent;

    /// <summary> Makes a deep copy of this object. </summary>
    /// <returns> A copy of this object. </returns>
    IDatity Clone();
  }

  /// <summary> An entity that can be destroyed. </summary>
  internal interface IDestroyable : IComponent
  {
    /// <summary> Invoked when the object should be destroyed. </summary>
    void Destroy();
  }

  internal class DestroyableSystem
  {
    public void ApplyDamage(IDatity sourceDamage, IDatity destinationDatity)
    {
      var health = destinationDatity.Get<IHealth>();
      var resistance = destinationDatity;
    }
  }
}