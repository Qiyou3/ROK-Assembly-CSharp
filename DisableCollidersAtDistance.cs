// Decompiled with JetBrains decompiler
// Type: DisableCollidersAtDistance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using UnityEngine;

#nullable disable
public class DisableCollidersAtDistance : MonoBehaviour
{
  private Collider[] _colliders;
  public float DisableDistance = 40f;
  private Bounds _bounds;
  private bool _lastEnableColliders = true;

  public Collider[] Colliders
  {
    get
    {
      if (this._colliders == null)
        this._colliders = this.GetComponentsInChildren<Collider>();
      return this._colliders;
    }
  }

  public Bounds Bounds => this._bounds;

  public void Start() => this._bounds = this.gameObject.GetBoundsFromColliders();

  public void Update()
  {
    if (Player.IsLocalDedi)
      return;
    bool flag = false;
    if ((Object) Entity.LocalPlayer != (Object) null)
      flag = (double) Mathf.Sqrt(this.Bounds.SqrDistance(Entity.LocalPlayer.Position)) < (double) this.DisableDistance;
    if (flag != this._lastEnableColliders)
    {
      foreach (Collider collider in this.Colliders)
        collider.enabled = flag;
    }
    this._lastEnableColliders = flag;
  }
}
