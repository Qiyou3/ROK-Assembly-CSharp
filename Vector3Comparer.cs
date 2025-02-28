// Decompiled with JetBrains decompiler
// Type: UnityTest.Vector3Comparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace UnityTest
{
  public class Vector3Comparer : VectorComparerBase<Vector3>
  {
    public Vector3Comparer.CompareType compareType;
    public double floatingPointError = 9.9999997473787516E-05;

    protected override bool Compare(Vector3 a, Vector3 b)
    {
      switch (this.compareType)
      {
        case Vector3Comparer.CompareType.MagnitudeEquals:
          return this.AreVectorMagnitudeEqual(a.magnitude, b.magnitude, this.floatingPointError);
        case Vector3Comparer.CompareType.MagnitudeNotEquals:
          return !this.AreVectorMagnitudeEqual(a.magnitude, b.magnitude, this.floatingPointError);
        default:
          throw new Exception();
      }
    }

    public enum CompareType
    {
      MagnitudeEquals,
      MagnitudeNotEquals,
    }
  }
}
