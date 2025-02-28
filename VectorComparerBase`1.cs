// Decompiled with JetBrains decompiler
// Type: UnityTest.VectorComparerBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace UnityTest
{
  public abstract class VectorComparerBase<T> : ComparerBaseGeneric<T>
  {
    protected bool AreVectorMagnitudeEqual(float a, float b, double floatingPointError)
    {
      return (double) Math.Abs(a) < floatingPointError && (double) Math.Abs(b) < floatingPointError || (double) Math.Abs(a - b) < floatingPointError;
    }
  }
}
