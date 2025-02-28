// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.ClockDrivenWaypoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class ClockDrivenWaypoint : EntityBehaviour
  {
    private AIBridge _aiBridge;
    public WaypointParams dayParams;
    public WaypointParams nightParams;

    private AIBridge MyAIBridge
    {
      get
      {
        if ((Object) this._aiBridge == (Object) null)
          this._aiBridge = this.Entity.GetOrCreate<AIBridge>();
        return this._aiBridge;
      }
    }

    public void OnValidate()
    {
      this.dayParams.OnValidate();
      this.nightParams.OnValidate();
    }

    public void Update()
    {
      this.MyAIBridge.CurrentWaypointParams = GameClock.Instance.CurrentTimeBlock != GameClock.TimeBlock.Night ? this.dayParams : this.nightParams;
    }
  }
}
