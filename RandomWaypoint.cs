// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.RandomWaypoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.TerrainAPI;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class RandomWaypoint : AIBehaviour
  {
    public float refreshInterval = 5f;
    public float waypointDistance = 10f;
    private bool _started;

    private Vector3 GetRandomPosition()
    {
      return (this.Entity.Position + Random.onUnitSphere * this.waypointDistance).AdjustToTerrainHeight();
    }

    public void Start()
    {
      this._started = true;
      this.StartCoroutine(this.WaypointGenerationRoutine());
      this.SetCurrentLocation(this.Entity.Position + this.Entity.MainTransform.forward * 100f);
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
      return (IEnumerator) new RandomWaypoint.\u003CWaypointGenerationRoutine\u003Ec__IteratorC9()
      {
        \u003C\u003Ef__this = this
      };
    }
  }
}
