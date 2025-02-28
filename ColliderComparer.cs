// Decompiled with JetBrains decompiler
// Type: UnityTest.ColliderComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace UnityTest
{
  public class ColliderComparer : ComparerBaseGeneric<Bounds>
  {
    public ColliderComparer.CompareType compareType;

    protected override bool Compare(Bounds a, Bounds b)
    {
      switch (this.compareType)
      {
        case ColliderComparer.CompareType.Intersects:
          return a.Intersects(b);
        case ColliderComparer.CompareType.DoesNotIntersect:
          return !a.Intersects(b);
        default:
          throw new Exception();
      }
    }

    public enum CompareType
    {
      Intersects,
      DoesNotIntersect,
    }
  }
}
