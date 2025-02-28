// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.SimpleForceAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class SimpleForceAttack : AttackBase
  {
    [Designer]
    public float VelocityChange;
    public Vector3 distancePenaltyMultiplier = Vector3.one;
    public Rigidbody CounterBody;
    public Rigidbody AttackBody;
    public Transform attackForcePointOnRigidbody;

    private Transform AttackForcePoint
    {
      get
      {
        if ((Object) this.attackForcePointOnRigidbody == (Object) null)
        {
          this.attackForcePointOnRigidbody = new GameObject("Attack Force Point").transform;
          this.attackForcePointOnRigidbody.parent = this.AttackBody.transform;
          this.attackForcePointOnRigidbody.position = this.AttackBody.worldCenterOfMass;
        }
        return this.attackForcePointOnRigidbody;
      }
    }

    public override void TriggerAttack(Location locationToAttack)
    {
      float velocityChange = this.VelocityChange;
      Vector3 vector3 = (locationToAttack.WorldPosition - this.AttackForcePoint.position).normalized * velocityChange;
      if (!vector3.IsValid())
        return;
      this.AttackBody.AddForceAtPosition(vector3, this.AttackForcePoint.position, ForceMode.VelocityChange);
      if (!((Object) this.CounterBody != (Object) null))
        return;
      this.CounterBody.AddForce(-vector3 * (this.AttackBody.mass / this.CounterBody.mass), ForceMode.VelocityChange);
    }
  }
}
