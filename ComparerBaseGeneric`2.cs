// Decompiled with JetBrains decompiler
// Type: UnityTest.ComparerBaseGeneric`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace UnityTest
{
  [Serializable]
  public abstract class ComparerBaseGeneric<T1, T2> : ComparerBase
  {
    public T2 constantValueGeneric = default (T2);

    public override object ConstValue
    {
      get => (object) this.constantValueGeneric;
      set => this.constantValueGeneric = (T2) value;
    }

    public override object GetDefaultConstValue() => (object) default (T2);

    private static bool IsValueType(Type type) => type.IsValueType;

    protected override bool Compare(object a, object b)
    {
      Type type = typeof (T2);
      return b != null || !ComparerBaseGeneric<T1, T2>.IsValueType(type) ? this.Compare((T1) a, (T2) b) : throw new ArgumentException("Null was passed to a value-type argument");
    }

    protected abstract bool Compare(T1 a, T2 b);

    public override Type[] GetAccepatbleTypesForA()
    {
      return new Type[1]{ typeof (T1) };
    }

    public override Type[] GetAccepatbleTypesForB()
    {
      return new Type[1]{ typeof (T2) };
    }

    protected override bool UseCache => true;
  }
}
