// Decompiled with JetBrains decompiler
// Type: ChangeWaypointSettingsLowHealth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.AI;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class ChangeWaypointSettingsLowHealth : EntityBehaviour
{
  public ClockDrivenWaypoint waypointsToReplace;
  public WaypointParams newWapoints;
  [Range(0.0f, 1f)]
  public float lowHealthPercentage = 0.2f;
  private Health health;

  public void Update()
  {
    if (!(bool) (Object) this.health)
      this.health = this.Entity.TryGet<Health>();
    if (!(bool) (Object) this.health || (double) this.health.CurrentHealthPercent > (double) this.lowHealthPercentage)
      return;
    this.waypointsToReplace.dayParams = this.newWapoints;
    this.waypointsToReplace.nightParams = this.newWapoints;
  }
}
