// Decompiled with JetBrains decompiler
// Type: FishSpawnerPlacer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public class FishSpawnerPlacer : MonoBehaviour
{
  public int rows;
  public int collumns;
  public float spawnerHeight;
  public GameObject topSpawner;
  public GameObject midSpawner;
  public GameObject botSpawner;
  private float width;
  private float depth;

  public float spawnerWidth
  {
    get
    {
      if ((Object) this.topSpawner != (Object) null && (double) this.width == 0.0)
      {
        SchoolController component = this.topSpawner.GetComponent<SchoolController>();
        if ((Object) component != (Object) null)
        {
          this.width = component._positionSphere + component._spawnSphere;
          this.width *= 2f;
        }
      }
      return this.width;
    }
  }

  public float spawnerDepth
  {
    get
    {
      if ((Object) this.topSpawner != (Object) null && (double) this.depth == 0.0)
      {
        SchoolController component = this.topSpawner.GetComponent<SchoolController>();
        if ((Object) component != (Object) null)
        {
          this.depth = component._positionSphereDepth + component._spawnSphereDepth;
          this.depth *= 2f;
        }
      }
      return this.depth;
    }
  }

  [ContextMenu("SPAWN")]
  public void Spawn() => this.StartCoroutine(this.InstantiateAndSetHeights());

  [DebuggerHidden]
  private IEnumerator InstantiateAndSetHeights()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new FishSpawnerPlacer.\u003CInstantiateAndSetHeights\u003Ec__Iterator148()
    {
      \u003C\u003Ef__this = this
    };
  }
}
