// Decompiled with JetBrains decompiler
// Type: UnityTest.ActionBaseGeneric`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace UnityTest
{
  public abstract class ActionBaseGeneric<T> : ActionBase
  {
    protected override bool Compare(object objVal) => this.Compare((T) objVal);

    protected abstract bool Compare(T objVal);

    public override Type[] GetAccepatbleTypesForA()
    {
      return new Type[1]{ typeof (T) };
    }

    public override Type GetParameterType() => typeof (T);

    protected override bool UseCache => true;
  }
}
