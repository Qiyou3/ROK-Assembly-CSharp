// Decompiled with JetBrains decompiler
// Type: ForceHub
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Utility;
using System;
using UnityEngine;

#nullable disable
[Serializable]
public class ForceHub
{
  private readonly RigidbodyGroup _rigidbodyGroup;
  private readonly Refresher _refresher;

  public ForceHub(RigidbodyGroup rigidbodyGroup)
  {
    this._rigidbodyGroup = rigidbodyGroup;
    this._refresher = new Refresher(RefresherType.FixedUpdate, new System.Action(this.ClearForceCounters));
  }

  public Vector3 NetForce { get; private set; }

  public Vector3 NetTorque { get; private set; }

  private void ClearForceCounters()
  {
    this.NetForce = Vector3.zero;
    this.NetTorque = Vector3.zero;
  }

  private void AddForceToCounters(Vector3 force, Vector3 position)
  {
    this._refresher.RefreshIfNotRefreshed();
    this.NetForce += force;
    this.NetTorque += RigidbodyUtil.GetTorque(force, position, this._rigidbodyGroup.CenterOfMass);
  }

  private void AddTorqueToCounters(Vector3 torque)
  {
    this._refresher.RefreshIfNotRefreshed();
    this.NetTorque += torque;
  }

  public void AddForce(Rigidbody rigidbodyToForce, Vector3 force)
  {
    this.AddForceToCounters(force, rigidbodyToForce.worldCenterOfMass);
    rigidbodyToForce.AddForce(force);
  }

  public void AddUniformForce(RigidbodyGroup subgroupToForce, Vector3 force)
  {
    this.AddForceToCounters(force, subgroupToForce.CenterOfMass);
    subgroupToForce.AddUniformForce(force);
  }

  public void AddUniformTorque(RigidbodyGroup subgroupToForce, Vector3 torque)
  {
    this.AddTorqueToCounters(torque);
    subgroupToForce.AddUniformTorque(torque);
  }

  public void AddForceAtPosition(Rigidbody rigidbodyToForce, Vector3 force, Vector3 position)
  {
    this.AddForceToCounters(force, position);
    rigidbodyToForce.AddForceAtPosition(force, position);
  }

  public void AddUniformForce(Vector3 force)
  {
    this._refresher.RefreshIfNotRefreshed();
    this.NetForce += force;
    this._rigidbodyGroup.AddUniformForce(force);
  }

  public void AddUniformTorque(Vector3 torque)
  {
    this._refresher.RefreshIfNotRefreshed();
    this.NetTorque += torque;
    this._rigidbodyGroup.AddUniformTorque(torque);
  }
}
