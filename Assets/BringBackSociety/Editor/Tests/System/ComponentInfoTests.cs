using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BringBackSociety.Engine.System;
using NUnit.Framework;

namespace Assets.BringBackSociety.Editor.Tests.System
{
  internal class ComponentInfoTests
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [Description("Success")]
    public void Success()
    {
      var info = ComponentInfo<IFake>.Info;
      Assert.AreEqual(0, info.Requirements.Length);
    }

    [Test]
    [Description("Not an interface")]
    public void Not_an_interface()
    {
      Assert.Throws<TypeInitializationException>(() => { var info = ComponentInfo<FakeComponent>.Info; });
    }

    [Test]
    [Description("Readwrite when not IWriteComponent")]
    public void Readwrite_when_not_IWriteComponent()
    {
      Assert.Throws<TypeInitializationException>(() => { var info = ComponentInfo<IFakeWritable>.Info; });
    }

    [Test]
    [Description("Writable is okay")]
    public void Writable_is_okay()
    {
      var info = ComponentInfo<IReadWriteComponent>.Info;
      Assert.AreEqual(0, info.Requirements.Length);
    }

    [Test]
    [Description("All must be writable")]
    public void All_must_be_writable()
    {
      Assert.Throws<TypeInitializationException>(() => { var info = ComponentInfo<IAllMustBeWritable>.Info; });
    }

    [Test]
    [Description("Requirements are fullfilled")]
    public void Requirements_are_fullfilled()
    {
      var info = ComponentInfo<IRequiresOthers>.Info;

      Assert.AreEqual(2, info.Requirements.Length);
      Assert.IsTrue(info.Requirements.Contains(ComponentInfo<IRealWritable>.Info));
      Assert.IsTrue(info.Requirements.Contains(ComponentInfo<IFake>.Info));
    }

    private interface IFake : IComponent
    {
      int Data { get; }
    }

    private interface IFakeWritable : IComponent
    {
      int Data { get; set; }
    }

    private interface IRealWritable : IReadWriteComponent
    {
      int Data { get; set; }
    }

    private interface IAllMustBeWritable : IReadWriteComponent
    {
      int Data { get; set; }

      string Name { get; }
    }

    private interface IRequiresOthers : IComponent,
                                        IRequiresComponent<IRealWritable>,
                                        IRequiresComponent<IFake>
    {
    }

    private class FakeComponent : IFake, IFakeWritable
    {
      public int Data
      {
        get { throw new NotImplementedException(); }
        set { throw new NotImplementedException(); }
      }

      /// <inheritdoc />
      public ComponentInfo GetComponentInfo()
      {
        return ComponentInfo<IFake>.Info;
      }
    }
  }
}