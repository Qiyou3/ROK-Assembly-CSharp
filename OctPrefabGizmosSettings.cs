// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Geometry.OctPrefabGizmosSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Geometry
{
  [Serializable]
  public class OctPrefabGizmosSettings
  {
    public OctPrefabGizmosSettings.GizmoType gizmoType = OctPrefabGizmosSettings.GizmoType.Solid;
    [Range(0.0f, 1f)]
    public float gizmoTransparency = 0.5f;

    public enum GizmoType
    {
      WireFrame,
      Solid,
    }
  }
}
