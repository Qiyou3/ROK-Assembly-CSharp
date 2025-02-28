// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.AIBridge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class AIBridge : EntityBehaviour
  {
    public Targetable CurrentTarget;
    private bool _hasLocation;
    private readonly Location _location = new Location();
    private readonly WaypointParams _defaultWaypointParams = new WaypointParams();
    private WaypointParams _waypointParams;

    public Location CurrentLocation => !this._hasLocation ? (Location) null : this._location;

    public void SetCurrentLocation(Vector3 localPosition, Transform parent = null)
    {
      this._hasLocation = true;
      this._location.localPosition = localPosition;
      this._location.parent = parent;
    }

    public void RemoveCurrentLocation()
    {
      this._hasLocation = false;
      this._location.localPosition = Vector3.zero;
      this._location.parent = (Transform) null;
    }

    public WaypointParams CurrentWaypointParams
    {
      get => this._waypointParams == null ? this._defaultWaypointParams : this._waypointParams;
      set => this._waypointParams = value;
    }
  }
}
