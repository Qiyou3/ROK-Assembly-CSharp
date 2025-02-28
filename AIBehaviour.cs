// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.AIBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class AIBehaviour : EntityBehaviour
  {
    private AIBridge _aiBridge;

    private AIBridge MyAIBridge
    {
      get
      {
        if ((Object) this._aiBridge == (Object) null)
          this._aiBridge = this.Entity.GetOrCreate<AIBridge>();
        return this._aiBridge;
      }
    }

    public Targetable CurrentTarget
    {
      get => this.MyAIBridge.CurrentTarget;
      set => this.MyAIBridge.CurrentTarget = value;
    }

    public Location CurrentLocation => this.MyAIBridge.CurrentLocation;

    public void SetCurrentLocation(Vector3 localPosition, Transform parent = null)
    {
      this.MyAIBridge.SetCurrentLocation(localPosition, parent);
    }

    public void RemoveCurrentLocation() => this.MyAIBridge.RemoveCurrentLocation();

    public WaypointParams CurrentWaypointParams
    {
      get => this.MyAIBridge.CurrentWaypointParams;
      set => this.MyAIBridge.CurrentWaypointParams = value;
    }
  }
}
