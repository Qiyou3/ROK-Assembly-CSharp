// Decompiled with JetBrains decompiler
// Type: UnityTest.IsRenderedByCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace UnityTest
{
  public class IsRenderedByCamera : ComparerBaseGeneric<Renderer, Camera>
  {
    public IsRenderedByCamera.CompareType compareType;

    protected override bool Compare(Renderer renderer, Camera camera)
    {
      bool flag = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), renderer.bounds);
      switch (this.compareType)
      {
        case IsRenderedByCamera.CompareType.IsVisible:
          return flag;
        case IsRenderedByCamera.CompareType.IsNotVisible:
          return !flag;
        default:
          throw new Exception();
      }
    }

    public enum CompareType
    {
      IsVisible,
      IsNotVisible,
    }
  }
}
