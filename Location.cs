// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.Location
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  [Serializable]
  public class Location
  {
    public Transform parent;
    public Vector3 localPosition;

    public Vector3 WorldPosition
    {
      get
      {
        return (UnityEngine.Object) this.parent != (UnityEngine.Object) null ? this.parent.TransformPoint(this.localPosition) : this.localPosition;
      }
    }
  }
}
