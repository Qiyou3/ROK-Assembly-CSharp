// Decompiled with JetBrains decompiler
// Type: EntityRigidbodyManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class EntityRigidbodyManager : BaseEntityConnectionManager<EntityRigidbodyManager>
{
  private RigidbodyGroup _allRigidbodies;
  private RigidbodyGroup _myRigidbodies;
  private ForceHub _forceHub;

  private static bool ShouldManageRigidbody(Rigidbody r)
  {
    if ((UnityEngine.Object) r == (UnityEngine.Object) null)
    {
      Logger.Error("Found a null rigidbody in ShouldManageRigidbody");
      return false;
    }
    return !r.isKinematic;
  }

  public RigidbodyGroup AllRigidbodies
  {
    get
    {
      if (this._allRigidbodies == null)
        this._allRigidbodies = new RigidbodyGroup(((IEnumerable<Rigidbody>) this.Entity.TryGetArray<Rigidbody>()).Where<Rigidbody>(new Func<Rigidbody, bool>(EntityRigidbodyManager.ShouldManageRigidbody)).ToArray<Rigidbody>());
      return this._allRigidbodies;
    }
  }

  public RigidbodyGroup MyRigidbodies
  {
    get
    {
      if (this._myRigidbodies == null)
        this._myRigidbodies = new RigidbodyGroup(((IEnumerable<Rigidbody>) this.Entity.TryGetArray<Rigidbody>()).Where<Rigidbody>(new Func<Rigidbody, bool>(EntityRigidbodyManager.ShouldManageRigidbody)).ToArray<Rigidbody>());
      return this._myRigidbodies;
    }
  }

  public ForceHub ForceHub
  {
    get
    {
      if (this._forceHub == null)
        this._forceHub = new ForceHub(this.MyRigidbodies);
      return this._forceHub;
    }
  }

  public bool CollisionEnabled
  {
    get => this.MyRigidbodies.CollisionEnabled;
    set => this.MyRigidbodies.CollisionEnabled = value;
  }

  public bool UseGravity
  {
    get => this.MyRigidbodies.UseGravity;
    set => this.MyRigidbodies.UseGravity = value;
  }

  protected override object GetCustomObject(List<BaseEntityConnectionManager> list)
  {
    List<Rigidbody> rigidbodyList = new List<Rigidbody>();
    for (int index1 = 0; index1 < list.Count; ++index1)
    {
      if (list[index1].HasEntity)
      {
        Rigidbody[] array = list[index1].Entity.TryGetArray<Rigidbody>();
        for (int index2 = 0; index2 < array.Length; ++index2)
        {
          if (EntityRigidbodyManager.ShouldManageRigidbody(array[index2]))
            rigidbodyList.Add(array[index2]);
        }
      }
    }
    return (object) new RigidbodyGroup(rigidbodyList.ToArray());
  }

  protected override void ApplyCustomObject(object customObject)
  {
    this._allRigidbodies = (RigidbodyGroup) customObject;
  }

  public void AddForce(Rigidbody childRigidbody, Vector3 force)
  {
    this.ForceHub.AddForce(childRigidbody, force);
  }

  public void AddForceAtPosition(Rigidbody childRigidbody, Vector3 force, Vector3 position)
  {
    this.ForceHub.AddForce(childRigidbody, force);
  }

  public void AddUniformForce(Vector3 force) => this.ForceHub.AddUniformForce(force);

  public void AddUniformTorque(Vector3 torque) => this.ForceHub.AddUniformTorque(torque);

  public void AddUniformForce(RigidbodyGroup subgroup, Vector3 force)
  {
    this.ForceHub.AddUniformForce(subgroup, force);
  }

  public void AddUniformTorque(RigidbodyGroup subgroup, Vector3 torque)
  {
    this.ForceHub.AddUniformTorque(subgroup, torque);
  }

  public void Teleport(Vector3 newPosition)
  {
    Vector3 position = this.Entity.Position;
    this.AllRigidbodies.OffsetPosition(newPosition - position);
    this.AllRigidbodies.SetVelocity(Vector3.zero);
    this.AllRigidbodies.SetAngularVelocity(Vector3.zero);
  }
}
