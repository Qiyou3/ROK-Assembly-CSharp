// Decompiled with JetBrains decompiler
// Type: CreationExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using PathologicalGames;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public class CreationExample : MonoBehaviour
{
  public Transform testPrefab;
  public string poolName = "Creator";
  public int spawnAmount = 50;
  public float spawnInterval = 0.25f;
  private SpawnPool pool;

  public void Start()
  {
    this.pool = PoolManager.Pools.Create(this.poolName);
    this.pool.group.parent = this.transform;
    this.pool.group.localPosition = new Vector3(1.5f, 0.0f, 0.0f);
    this.pool.group.localRotation = Quaternion.identity;
    this.pool.CreatePrefabPool(new PrefabPool(this.testPrefab)
    {
      preloadAmount = 5,
      cullDespawned = true,
      cullAbove = 10,
      cullDelay = 1,
      limitInstances = true,
      limitAmount = 5,
      limitFIFO = true
    });
    this.StartCoroutine(this.Spawner());
    Transform prefab = PoolManager.Pools["Shapes"].prefabs["Cube"];
    PoolManager.Pools["Shapes"].Spawn(prefab).name = "Cube (Spawned By CreationExample.cs)";
  }

  [DebuggerHidden]
  private IEnumerator Spawner()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new CreationExample.\u003CSpawner\u003Ec__Iterator129()
    {
      \u003C\u003Ef__this = this
    };
  }

  [DebuggerHidden]
  private IEnumerator Despawner()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new CreationExample.\u003CDespawner\u003Ec__Iterator12A()
    {
      \u003C\u003Ef__this = this
    };
  }
}
