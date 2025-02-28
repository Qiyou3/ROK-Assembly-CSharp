// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.BipedModdableCollisionDamage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Damaging;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Modding.Abstract;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class BipedModdableCollisionDamage : EntityBehaviour, IModable
  {
    private const string _torsoRelativeVelocityThreshold = "Torso.RelativeVelocityThreshold";
    private const string _torsoImpactThreshold = "Torso.ImpactThreshold";
    private const string _torsoImpactMultiplier = "Torso.ImpactMultiplier";
    private const string _legsRelativeVelocityThreshold = "Legs.RelativeVelocityThreshold";
    private const string _legsImpactThreshold = "Legs.ImpactThreshold";
    private const string _legsImpactMultiplier = "Legs.ImpactMultiplier";
    public CollisionDamage TorsoCollision;
    public CollisionDamage LegsCollision;
    public PhysicsDefendCollisionModifier TorsoModifier;
    public PhysicsDefendCollisionModifier LegsModifier;

    public string ModHandlerName => "PhysicsDamage";

    public void GetModDefaults(IList<ModEntry> defaultModList)
    {
      this.NullCheck<CollisionDamage>(this.TorsoCollision, "TorsoCollision");
      if ((Object) this.TorsoCollision != (Object) null)
        defaultModList.Add(new ModEntry("Torso.RelativeVelocityThreshold", (object) this.TorsoCollision.relativeVelocityMagnitudeThreshold));
      this.NullCheck<PhysicsDefendCollisionModifier>(this.TorsoModifier, "TorsoModifier");
      if ((Object) this.TorsoModifier != (Object) null)
      {
        defaultModList.Add(new ModEntry("Torso.ImpactThreshold", (object) this.TorsoModifier.ImpactDamageThreshold));
        defaultModList.Add(new ModEntry("Torso.ImpactMultiplier", (object) this.TorsoModifier.ImpactDamageMultiplier));
      }
      this.NullCheck<CollisionDamage>(this.LegsCollision, "LegsCollision");
      if ((Object) this.LegsCollision != (Object) null)
        defaultModList.Add(new ModEntry("Legs.RelativeVelocityThreshold", (object) this.LegsCollision.relativeVelocityMagnitudeThreshold));
      this.NullCheck<PhysicsDefendCollisionModifier>(this.LegsModifier, "LegsModifier");
      if (!((Object) this.LegsModifier != (Object) null))
        return;
      defaultModList.Add(new ModEntry("Legs.ImpactThreshold", (object) this.LegsModifier.ImpactDamageThreshold));
      defaultModList.Add(new ModEntry("Legs.ImpactMultiplier", (object) this.LegsModifier.ImpactDamageMultiplier));
    }

    public void ApplyMod(string key, object value)
    {
      if (value == null)
        return;
      string key1 = key;
      if (key1 == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (BipedModdableCollisionDamage.\u003C\u003Ef__switch\u0024map29 == null)
      {
        // ISSUE: reference to a compiler-generated field
        BipedModdableCollisionDamage.\u003C\u003Ef__switch\u0024map29 = new Dictionary<string, int>(6)
        {
          {
            "Torso.RelativeVelocityThreshold",
            0
          },
          {
            "Torso.ImpactThreshold",
            1
          },
          {
            "Torso.ImpactMultiplier",
            2
          },
          {
            "Legs.RelativeVelocityThreshold",
            3
          },
          {
            "Legs.ImpactThreshold",
            4
          },
          {
            "Legs.ImpactMultiplier",
            5
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!BipedModdableCollisionDamage.\u003C\u003Ef__switch\u0024map29.TryGetValue(key1, out num))
        return;
      switch (num)
      {
        case 0:
          this.NullCheck<CollisionDamage>(this.TorsoCollision, "TorsoCollision");
          if (!((Object) this.TorsoCollision != (Object) null))
            break;
          this.TorsoCollision.relativeVelocityMagnitudeThreshold = Mathf.Max((float) value, 0.0f);
          break;
        case 1:
          this.NullCheck<PhysicsDefendCollisionModifier>(this.TorsoModifier, "TorsoModifier");
          if (!((Object) this.TorsoModifier != (Object) null))
            break;
          this.TorsoModifier.ImpactDamageThreshold = Mathf.Max((float) value, 0.0f);
          break;
        case 2:
          this.NullCheck<PhysicsDefendCollisionModifier>(this.TorsoModifier, "TorsoModifier");
          if (!((Object) this.TorsoModifier != (Object) null))
            break;
          this.TorsoModifier.ImpactDamageMultiplier = Mathf.Max((float) value, 0.0f);
          break;
        case 3:
          this.NullCheck<CollisionDamage>(this.LegsCollision, "LegsCollision");
          if (!((Object) this.LegsCollision != (Object) null))
            break;
          this.LegsCollision.relativeVelocityMagnitudeThreshold = Mathf.Max((float) value, 0.0f);
          break;
        case 4:
          this.NullCheck<PhysicsDefendCollisionModifier>(this.LegsModifier, "LegsModifier");
          if (!((Object) this.LegsModifier != (Object) null))
            break;
          this.LegsModifier.ImpactDamageThreshold = Mathf.Max((float) value, 0.0f);
          break;
        case 5:
          this.NullCheck<PhysicsDefendCollisionModifier>(this.LegsModifier, "LegsModifier");
          if (!((Object) this.LegsModifier != (Object) null))
            break;
          this.LegsModifier.ImpactDamageMultiplier = Mathf.Max((float) value, 0.0f);
          break;
      }
    }
  }
}
