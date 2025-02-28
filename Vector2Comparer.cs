// Decompiled with JetBrains decompiler
// Type: UnityTest.Vector2Comparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace UnityTest
{
  public class Vector2Comparer : VectorComparerBase<Vector2>
  {
    public Vector2Comparer.CompareType compareType;
    public float floatingPointError = 0.0001f;

    protected override bool Compare(Vector2 a, Vector2 b)
    {
      switch (this.compareType)
      {
        case Vector2Comparer.CompareType.MagnitudeEquals:
          return this.AreVectorMagnitudeEqual(a.magnitude, b.magnitude, (double) this.floatingPointError);
        case Vector2Comparer.CompareType.MagnitudeNotEquals:
          return !this.AreVectorMagnitudeEqual(a.magnitude, b.magnitude, (double) this.floatingPointError);
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
