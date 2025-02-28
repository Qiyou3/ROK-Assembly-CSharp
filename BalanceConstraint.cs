// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.BalanceConstraint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using System;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  [Serializable]
  public class BalanceConstraint
  {
    public AngularConstraint Constraint;
    public BalanceConstraint.BalanceMode PitchBalanceMode;
    public BalanceConstraint.BalanceMode RollBalanceMode = BalanceConstraint.BalanceMode.Gravity;

    public Vector3 GetTorque(Quaternion rotation, Vector3 angularVelocity, Vector3 groundNormal)
    {
      Vector3 vector3_1 = rotation * Vector3.right;
      Vector3 vector3_2 = rotation * Vector3.up;
      Vector3 vector3_3 = rotation * Vector3.forward;
      if (this.PitchBalanceMode == this.RollBalanceMode)
      {
        switch (this.PitchBalanceMode)
        {
          case BalanceConstraint.BalanceMode.Gravity:
            return this.Constraint.GetTorque(vector3_2, angularVelocity, -Physics.gravity, Vector3.zero).Perpendicular(groundNormal);
          case BalanceConstraint.BalanceMode.Normal:
            return this.Constraint.GetTorque(vector3_2, angularVelocity, groundNormal, Vector3.zero).Perpendicular(groundNormal);
          default:
            return Vector3.zero;
        }
      }
      else
      {
        Vector3 torque = Vector3.zero;
        if (this.PitchBalanceMode != BalanceConstraint.BalanceMode.None)
        {
          Vector3 vector3_4;
          switch (this.PitchBalanceMode)
          {
            case BalanceConstraint.BalanceMode.Normal:
              vector3_4 = groundNormal;
              break;
            default:
              vector3_4 = -Physics.gravity;
              break;
          }
          Vector3 normalized = vector3_3.Perpendicular(vector3_4).normalized;
          torque = torque + this.Constraint.GetTorque(vector3_3, angularVelocity, normalized, Vector3.zero).Parallel(vector3_1) * Mathf.Max(0.0f, Vector3.Dot(vector3_3, normalized)) + this.Constraint.GetTorque(vector3_2, angularVelocity, vector3_4, Vector3.zero).Parallel(vector3_1) * Mathf.Max(0.0f, -Vector3.Dot(vector3_2, vector3_4));
        }
        if (this.RollBalanceMode != BalanceConstraint.BalanceMode.None)
        {
          Vector3 vector3_5;
          switch (this.RollBalanceMode)
          {
            case BalanceConstraint.BalanceMode.Normal:
              vector3_5 = groundNormal;
              break;
            default:
              vector3_5 = -Physics.gravity;
              break;
          }
          Vector3 normalized = vector3_1.Perpendicular(vector3_5).normalized;
          torque = torque + this.Constraint.GetTorque(vector3_1, angularVelocity, normalized, Vector3.zero).Parallel(vector3_3) * Mathf.Max(0.0f, Vector3.Dot(vector3_1, normalized)) + this.Constraint.GetTorque(vector3_2, angularVelocity, vector3_5, Vector3.zero).Parallel(vector3_3) * Mathf.Max(0.0f, -Vector3.Dot(vector3_2, vector3_5));
        }
        return torque;
      }
    }

    public enum BalanceMode
    {
      None,
      Gravity,
      Normal,
    }
  }
}
