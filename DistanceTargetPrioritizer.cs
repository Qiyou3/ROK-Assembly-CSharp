// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.DistanceTargetPrioritizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class DistanceTargetPrioritizer : TargetPrioritizerBase
  {
    public float innerDistance;
    public Vector3 distancePenaltyMultiplier;

    protected override float GetPriorityForInternal(Targetable targetable)
    {
      Vector3 vector3 = targetable.Entity.Position - this.Entity.Position;
      vector3.x *= this.distancePenaltyMultiplier.x;
      vector3.y *= this.distancePenaltyMultiplier.y;
      vector3.z *= this.distancePenaltyMultiplier.z;
      float magnitude = vector3.magnitude;
      return (double) magnitude < (double) this.innerDistance ? 10000f / magnitude : targetable.priority / magnitude;
    }
  }
}
