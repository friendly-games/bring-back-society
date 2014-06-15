using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BringBackSociety.Controllers;
using BringBackSociety.Items;
using Moq;
using Xunit;

namespace Tests.Items
{
  public class StorageContainerTests
  {
    private readonly StorageContainer _container;
    private readonly IItemModel _model1;
    private readonly IItemModel _model3;
    private IItemModel _model2;

    public StorageContainerTests()
    {
      _container = new StorageContainer(10);

      _model1 = Mock.Of<IItemModel>(
        model => model.StackAmount == 10 && model.Name == "Model 1");
      _model2 = Mock.Of<IItemModel>(
        model => model.StackAmount == 20 && model.Name == "Model 2");
      _model3 = Mock.Of<IItemModel>(
        model => model.StackAmount == 30 && model.Name == "Model 3");

      PutInto(5, _model1, 5);
      PutInto(1, _model2, 10);
      PutInto(0, _model3, 30);
    }

    private void PutInto(int slot, IItemModel model, int amount)
    {
      var cursor = _container.GetCursor(slot);
      _container.Put(cursor, new InventoryStack(model, amount));
    }

    private InventoryStack Get(int slot)
    {
      var cursor = _container.GetCursor(slot);
      return _container.GetStack(cursor);
    }

    [Fact]
    [Description]
    public void Initialized_correctly()
    {
      Assert.Equal(new InventoryStack(_model1, 5), Get(5));
    }

    [Fact]
    public void Adding_fills_existing_first()
    {
      _container.AddToStorage(new InventoryStack(_model1, 5));

      Assert.Equal(new InventoryStack(_model1, 10), Get(5));
    }

    [Fact]
    [Description("Cannot exceed model limit")]
    public void Cannot_exceed_model_limit()
    {
      var remaining = _container.AddToStorage(new InventoryStack(_model1, 10));

      var stack = Get(5);

      // still should be ten
      Assert.Equal(new InventoryStack(_model1, 10), stack);

      // with the remaining added to the third
      Assert.Equal(new InventoryStack(_model1, 0), remaining);
      Assert.Equal(new InventoryStack(_model1, 5), Get(2));
    }

    [Fact]
    [Description("Cursor fill returns any extras")]
    public void Cursor_fill_returns_any_extras()
    {
      var cursor = _container.GetCursor(5);
      var remaining = _container.Fill(cursor, new InventoryStack(_model1, 10));

      // filled (to 10), remaining is 5
      Assert.Equal(new InventoryStack(_model1, 10), Get(5));
      Assert.Equal(new InventoryStack(_model1, 5), remaining);
    }

    [Fact]
    [Description("Put in an empty slot puts contents there")]
    public void Put_in_an_empty_slot_puts_contents_there()
    {
      Assert.Equal(InventoryStack.Empty, Get(2));

      var remaining = _container.Put(_container.GetCursor(2), new InventoryStack(_model1, 5));

      Assert.Equal(new InventoryStack(_model1, 5), Get(5));
      Assert.Equal(InventoryStack.Empty, remaining);
    }

    [Fact]
    [Description("Put in a non_empty slot returns the original amount")]
    public void Put_in_a_non_empty_slot_returns_the_original_amount()
    {
      var contentsOfSlotZero = new InventoryStack(_model3, 30);
      Assert.Equal(contentsOfSlotZero, Get(0));

      var remaining = _container.Put(_container.GetCursor(0), new InventoryStack(_model1, 5));

      Assert.Equal(contentsOfSlotZero, Get(0));

      // put value is returned
      Assert.Equal(new InventoryStack(_model1, 5), remaining);
    }

    [Fact]
    [Description("Fill in non_empty other slot returns the original")]
    public void Fill_in_non_empty_other_slot_returns_the_original()
    {
      var contentsOfSlotZero = new InventoryStack(_model3, 30);
      Assert.Equal(contentsOfSlotZero, Get(0));

      var remaining = _container.Fill(_container.GetCursor(0), new InventoryStack(_model1, 5));

      Assert.Equal(contentsOfSlotZero, Get(0));

      // put value is returned
      Assert.Equal(new InventoryStack(_model1, 5), remaining);
    }

    [Fact]
    [Description("Removing a stack removes those items")]
    public void Removing_a_stack_removes_those_items()
    {
      Assert.Equal(new InventoryStack(_model1, 5), Get(5));

      var removed = _container.RemoveStack(_container.GetCursor(5));

      // is now empty
      Assert.Equal(InventoryStack.Empty, Get(5));
      // and we returned what we removed
      Assert.Equal(new InventoryStack(_model1, 5), removed);
    }
  }
}