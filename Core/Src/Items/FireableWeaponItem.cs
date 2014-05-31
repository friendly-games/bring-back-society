namespace BringBackSociety.Items
{
  /// <summary> An item which holds the fireable weapon. </summary>
  internal class FireableWeaponItem : IItemModel
  {
    /// <summary> Constructor. </summary>
    /// <param name="model"> The ui component associated with the item. </param>
    /// <param name="stats"> The damage stats for the item. </param>
    /// <param name="resource"> The ui resource associated with the item. </param>
    public FireableWeaponItem(IFireableWeaponModel model,
      FireableWeaponStats stats,
      IUiResource resource)
    {
      Model = model;
      Stats = stats;
      Resource = resource;
    }

    /// <summary> The ui component associated with the item. </summary>
    public IFireableWeaponModel Model { get; private set; }

    /// <summary> The damage stats for the item. </summary>
    public FireableWeaponStats Stats { get; private set; }

    /// <summary> Info about the weapon that is passed to the ui. </summary>
    public IUiResource Resource { get; private set; }

    /// <inheritdoc />
    public int StackAmount
    {
      // we can only ever hold 1 weapon in a slot at a time
      get { return 1; }
    }

    public string Name
    {
      get { return "Gun"; }
    }
  }
}