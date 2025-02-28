// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.SpawnerBasedWaypoints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.TerrainAPI;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class SpawnerBasedWaypoints : AIBehaviour
  {
    public float refreshInterval = 5f;
    public float distanceFromSpawner = 10f;
    public float distanceFromLastDistance = 20f;
    public float yOffset;
    private Vector3 initialPosition;
    private bool _started;

    private Vector3 GetNewPoint()
    {
      Vector3 terrainHeight = (this.Entity.Get<MainTransform>().transform.position + Random.onUnitSphere * this.distanceFromLastDistance).AdjustToTerrainHeight();
      terrainHeight.y += this.yOffset;
      Vector3 vector3_1 = Vector3.Scale(this.initialPosition - terrainHeight, Vector3.one - Vector3.up);
      float magnitude = vector3_1.magnitude;
      if ((double) magnitude > (double) this.distanceFromSpawner)
      {
        Vector3 vector3_2 = vector3_1 / magnitude;
        terrainHeight.x = this.initialPosition.x + vector3_2.x * this.distanceFromSpawner;
        terrainHeight.z = this.initialPosition.z + vector3_2.z * this.distanceFromSpawner;
      }
      return terrainHeight;
    }

    public void Start()
    {
      this.initialPosition = this.Entity.Get<MainTransform>().transform.position;
      this._started = true;
      this.StartCoroutine(this.WaypointGenerationRoutine());
    }

    public void OnEnable()
    {
      if (!this._started)
        return;
      this.Start();
    }

    [DebuggerHidden]
    public IEnumerator WaypointGenerationRoutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new SpawnerBasedWaypoints.\u003CWaypointGenerationRoutine\u003Ec__Iterator146()
      {
        \u003C\u003Ef__this = this
      };
    }
  }
}
