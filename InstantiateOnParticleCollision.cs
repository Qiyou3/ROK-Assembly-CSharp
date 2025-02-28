// Decompiled with JetBrains decompiler
// Type: InstantiateOnParticleCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.EffectsPooling;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class InstantiateOnParticleCollision : MonoBehaviour
{
  public GameObject Prefab;
  public bool ParentIsCollider = true;
  private ParticleSystem _System;
  private ParticleCollisionEvent[] _CollisionEvents;
  private List<GameObject> Instances = new List<GameObject>();

  public void Start()
  {
    this._System = this.GetComponent<ParticleSystem>();
    this._CollisionEvents = new ParticleCollisionEvent[16];
  }

  public void OnParticleCollision(GameObject other)
  {
    int collisionEventSize = this._System.GetSafeCollisionEventSize();
    if (this._CollisionEvents.Length < collisionEventSize)
      this._CollisionEvents = new ParticleCollisionEvent[collisionEventSize];
    int collisionEvents = this._System.GetCollisionEvents(other, this._CollisionEvents);
    for (int index = 0; index < collisionEvents; ++index)
    {
      GameObject gameObject = EffectsPool.Instantiate(this.Prefab, this._CollisionEvents[index].intersection, Quaternion.FromToRotation(Vector3.up, this._CollisionEvents[index].normal));
      if (this.ParentIsCollider)
        gameObject.transform.parent = other.transform;
      this.Instances.Add(gameObject);
    }
  }

  public void OnDisable() => this.Instances.Clear();
}
