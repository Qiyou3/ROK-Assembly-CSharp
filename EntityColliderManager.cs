// Decompiled with JetBrains decompiler
// Type: EntityColliderManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Tracing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public sealed class EntityColliderManager : BaseEntityConnectionManager<EntityColliderManager>
{
  private List<Collider> _myColliders;
  private List<Collider> _allColliders;
  private TracerIgnoreParams _visibilityParams;

  public List<Collider> MyColliders
  {
    get
    {
      if (this._myColliders == null)
        this._myColliders = ((IEnumerable<Collider>) this.Entity.TryGetArray<Collider>()).ToList<Collider>();
      return this._myColliders;
    }
  }

  public List<Collider> AllColliders
  {
    get
    {
      if (this._allColliders == null)
        this._allColliders = ((IEnumerable<Collider>) this.Entity.TryGetArray<Collider>()).ToList<Collider>();
      return this._allColliders;
    }
  }

  protected override void SetConnected(Entity other, bool ignore)
  {
    if ((Object) other == (Object) null)
      return;
    Collider[] array1 = this.Entity.TryGetArray<Collider>();
    Collider[] array2 = other.TryGetArray<Collider>();
    for (int index1 = 0; index1 < array1.Length; ++index1)
    {
      Collider collider1 = array1[index1];
      if (!((Object) collider1 == (Object) null) && collider1.enabled && collider1.gameObject.activeInHierarchy)
      {
        for (int index2 = 0; index2 < array2.Length; ++index2)
        {
          Collider collider2 = array2[index2];
          if (!((Object) collider2 == (Object) null) && collider2.enabled && collider2.gameObject.activeInHierarchy)
            Physics.IgnoreCollision(collider1, collider2, ignore);
        }
      }
    }
  }

  protected override object GetCustomObject(List<BaseEntityConnectionManager> list)
  {
    List<Collider> customObject = new List<Collider>();
    for (int index = 0; index < list.Count; ++index)
    {
      if (list[index].HasEntity)
      {
        foreach (Collider collider in list[index].Entity.TryGetArray<Collider>())
          customObject.Add(collider);
      }
    }
    return (object) customObject;
  }

  protected override void ApplyCustomObject(object customObject)
  {
    this._allColliders = (List<Collider>) customObject;
  }

  private TracerIgnoreParams VisibilityParams
  {
    get
    {
      if (this._visibilityParams == null)
        this._visibilityParams = new TracerIgnoreParams(this.Entity, TracerEntityIgnoreFlags.IgnoreSelfOnly);
      return this._visibilityParams;
    }
  }

  public bool GetEntityVisibility(Entity otherEntity)
  {
    return this.GetEntityVisibility(otherEntity, this.Entity.Position);
  }

  public bool GetEntityVisibility(Entity otherEntity, Vector3 visibleFrom)
  {
    Vector3 direction = otherEntity.Position - visibleFrom;
    float magnitude = direction.magnitude;
    RaycastHit raycastHit = new Ray(visibleFrom, direction).Raycast(0.0f, magnitude, this.VisibilityParams);
    return (Object) raycastHit.collider == (Object) null || raycastHit.collider.BelongsTo(otherEntity);
  }
}
