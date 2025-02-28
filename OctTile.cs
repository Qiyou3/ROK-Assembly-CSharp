// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Geometry.OctTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using SmartAssembly.Attributes;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Geometry
{
  [DoNotObfuscate]
  public abstract class OctTile : MonoBehaviour
  {
    [HideInInspector]
    public bool allowMirror = true;
    [HideInInspector]
    public bool allowXRotation = true;
    [HideInInspector]
    public bool allowYRotation = true;
    [HideInInspector]
    public bool allowZRotation = true;
    [HideInInspector]
    public float probability = 1f;
    [HideInInspector]
    public int tileIndex;

    public virtual float GetTransformProbability(int x, int y, int z, bool mirror)
    {
      return x != 0 && !this.allowXRotation || y != 0 && !this.allowYRotation || z != 0 && !this.allowZRotation || mirror && !this.allowMirror ? 0.0f : 1f;
    }

    public abstract IEnumerable<OctTileData> GetData();
  }
}
