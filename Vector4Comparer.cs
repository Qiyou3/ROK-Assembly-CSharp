// Decompiled with JetBrains decompiler
// Type: UnityTest.Vector4Comparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace UnityTest
{
  public class Vector4Comparer : VectorComparerBase<Vector4>
  {
    public Vector4Comparer.CompareType compareType;
    public double floatingPointError;

    protected override bool Compare(Vector4 a, Vector4 b)
    {
      switch (this.compareType)
      {
        case Vector4Comparer.CompareType.MagnitudeEquals:
          return this.AreVectorMagnitudeEqual(a.magnitude, b.magnitude, this.floatingPointError);
        case Vector4Comparer.CompareType.MagnitudeNotEquals:
          return !this.AreVectorMagnitudeEqual(a.magnitude, b.magnitude, this.floatingPointError);
        default:
          throw new Exception();
      }
    }

    public override int GetDepthOfSearch() => 3;

    public enum CompareType
    {
      MagnitudeEquals,
      MagnitudeNotEquals,
    }
  }
}
