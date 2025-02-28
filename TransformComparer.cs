// Decompiled with JetBrains decompiler
// Type: UnityTest.TransformComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace UnityTest
{
  public class TransformComparer : ComparerBaseGeneric<Transform>
  {
    public TransformComparer.CompareType compareType;

    protected override bool Compare(Transform a, Transform b)
    {
      if (this.compareType == TransformComparer.CompareType.Equals)
        return a.position == b.position;
      if (this.compareType == TransformComparer.CompareType.NotEquals)
        return a.position != b.position;
      throw new Exception();
    }

    public enum CompareType
    {
      Equals,
      NotEquals,
    }
  }
}
