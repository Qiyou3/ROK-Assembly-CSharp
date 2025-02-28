// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.BipedStability
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Modding.Abstract;
using CodeHatch.Networking.Events;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class BipedStability : EntityBehaviour, IModable
  {
    private const string _minStabilityName = "Stability.Min";
    private const string _dropOffBeginAngleName = "Stability.SlopeAngle.DropOffBegin";
    private const string _dropOffEndAngleName = "Stability.SlopeAngle.DropOffEnd";
    private const string _dropOffBeginVelocityName = "Stability.RelativeContactVelocity.DropOffBegin";
    private const string _dropOffEndVelocityName = "Stability.RelativeContactVelocity.DropOffEnd";
    [Range(0.0f, 10f)]
    public float StabilityHalfLife = 0.25f;
    [Range(0.0f, 1f)]
    public float MinStability = 0.1f;
    public BipedStability.SlopeStabilityModule SlopeStability;
    public BipedStability.VelocityStabilityModule VelocityStability;
    private float _lastGroundedTime;
    private float _lastJumpTime;
    [CanBeNull]
    private MotorBridge _myMotorBridge;
    [CanBeNull]
    private BipedBody _myBipedBody;

    [CanBeNull]
    public MotorBridge MyMotorBridge
    {
      get
      {
        if ((UnityEngine.Object) this._myMotorBridge == (UnityEngine.Object) null)
        {
          Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
          if ((UnityEngine.Object) entity == (UnityEngine.Object) null)
            return (MotorBridge) null;
          this._myMotorBridge = entity.GetOrCreate<MotorBridge>();
        }
        return this._myMotorBridge;
      }
    }

    [CanBeNull]
    public BipedBody MyBipedBody
    {
      get
      {
        if ((UnityEngine.Object) this._myBipedBody == (UnityEngine.Object) null)
        {
          Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
          if ((UnityEngine.Object) entity == (UnityEngine.Object) null)
            return (BipedBody) null;
          this._myBipedBody = entity.Get<BipedBody>();
        }
        return this._myBipedBody;
      }
    }

    public void Start()
    {
      Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
      if ((UnityEngine.Object) entity == (UnityEngine.Object) null)
        return;
      if (!entity.IsLocallyOwned)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this);
        this.enabled = false;
      }
      else
      {
        EventManager.Subscribe<EntityJumpEvent>(new EventSubscriber<EntityJumpEvent>(this.OnEntityJump));
        BipedBody bipedBody = this.NullCheck<BipedBody>(this.MyBipedBody, "MyBipedBody");
        if ((UnityEngine.Object) bipedBody == (UnityEngine.Object) null)
          return;
        bipedBody.Torso.gameObject.AddComponent<NotifyOnCollision>().collisionEnter += new Action<Collision>(this.OnCollisionEnter);
        bipedBody.Legs.gameObject.AddComponent<NotifyOnCollision>().collisionStay += new Action<Collision>(this.OnCollisionStay);
      }
    }

    public void OnDestroy()
    {
      EventManager.Unsubscribe<EntityJumpEvent>(new EventSubscriber<EntityJumpEvent>(this.OnEntityJump));
    }

    public void OnEntityJump(EntityJumpEvent theEvent)
    {
      Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
      if ((UnityEngine.Object) entity == (UnityEngine.Object) null || (UnityEngine.Object) theEvent.Entity != (UnityEngine.Object) entity || !theEvent.SendLocally && theEvent.IsSender)
        return;
      this._lastJumpTime = Time.time;
    }

    public void OnCollisionEnter(Collision collision) => this.OnCollisionStay(collision);

    public void OnCollisionStay(Collision collision)
    {
      if (!this.enabled)
        return;
      this.SlopeStability.OnCollisionStay(collision);
      this.VelocityStability.OnCollisionStay(collision);
    }

    public void FixedUpdate()
    {
      MotorBridge motorBridge = this.NullCheck<MotorBridge>(this.MyMotorBridge, "MyMotorBridge");
      if ((UnityEngine.Object) motorBridge == (UnityEngine.Object) null)
      {
        this.SlopeStability.ClearContacts();
      }
      else
      {
        motorBridge.Stability = HalfLife.GainLoss(motorBridge.Stability, this.MinStability, 0.0f, this.StabilityHalfLife);
        this.SlopeStability.FixedUpdate();
        this.VelocityStability.FixedUpdate();
        motorBridge.Stability = Mathf.Max(motorBridge.Stability, this.SlopeStability.Stability);
        motorBridge.Stability = Mathf.Min(motorBridge.Stability, this.VelocityStability.Stability);
        if ((double) motorBridge.Stability < (double) this.MinStability)
          motorBridge.Stability = this.MinStability;
        if ((double) this.SlopeStability.Stability > 0.0)
        {
          motorBridge.LastGroundNormalTouched = this.SlopeStability.Normal;
          this._lastGroundedTime = Time.time;
        }
        motorBridge.Grounded = (double) Time.time - (double) this._lastJumpTime > 0.25 && (double) Time.time - (double) this._lastGroundedTime < 0.25;
      }
    }

    public string ModHandlerName => "Movement";

    public void GetModDefaults(IList<ModEntry> defaultModList)
    {
      defaultModList.Add(new ModEntry("Stability.Min", (object) this.MinStability));
      defaultModList.Add(new ModEntry("Stability.SlopeAngle.DropOffBegin", (object) this.SlopeStability.StabilityDropOffBeginAngle));
      defaultModList.Add(new ModEntry("Stability.SlopeAngle.DropOffEnd", (object) this.SlopeStability.StabilityDropOffEndAngle));
      defaultModList.Add(new ModEntry("Stability.RelativeContactVelocity.DropOffBegin", (object) this.VelocityStability.StabilityDropOffBeginVelocity));
      defaultModList.Add(new ModEntry("Stability.RelativeContactVelocity.DropOffEnd", (object) this.VelocityStability.StabilityDropOffEndVelocity));
    }

    public void ApplyMod(string key, object value)
    {
      if (value == null)
        return;
      string key1 = key;
      if (key1 == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (BipedStability.\u003C\u003Ef__switch\u0024map2B == null)
      {
        // ISSUE: reference to a compiler-generated field
        BipedStability.\u003C\u003Ef__switch\u0024map2B = new Dictionary<string, int>(5)
        {
          {
            "Stability.Min",
            0
          },
          {
            "Stability.SlopeAngle.DropOffBegin",
            1
          },
          {
            "Stability.SlopeAngle.DropOffEnd",
            2
          },
          {
            "Stability.RelativeContactVelocity.DropOffBegin",
            3
          },
          {
            "Stability.RelativeContactVelocity.DropOffEnd",
            4
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!BipedStability.\u003C\u003Ef__switch\u0024map2B.TryGetValue(key1, out num))
        return;
      switch (num)
      {
        case 0:
          this.MinStability = Mathf.Clamp01((float) value);
          break;
        case 1:
          this.SlopeStability.StabilityDropOffBeginAngle = Mathf.Clamp((float) value, 0.0f, 90f);
          break;
        case 2:
          this.SlopeStability.StabilityDropOffEndAngle = Mathf.Clamp((float) value, 0.0f, 90f);
          break;
        case 3:
          this.VelocityStability.StabilityDropOffBeginVelocity = Mathf.Max((float) value, 0.0f);
          break;
        case 4:
          this.VelocityStability.StabilityDropOffEndVelocity = Mathf.Max((float) value, 0.0f);
          break;
      }
    }

    public abstract class StabilityModule
    {
      public float Stability { get; protected set; }

      public abstract void OnCollisionStay(Collision collision);

      public abstract void FixedUpdate();
    }

    [Serializable]
    public class SlopeStabilityModule : BipedStability.StabilityModule
    {
      [Range(0.0f, 90f)]
      public float StabilityDropOffBeginAngle;
      [Range(0.0f, 90f)]
      public float StabilityDropOffEndAngle;
      [CanBeNull]
      private float[] _contactMultipliers;
      private readonly List<ContactPoint> _contacts = new List<ContactPoint>();

      private float GetStability(float angle)
      {
        if ((double) angle <= (double) this.StabilityDropOffBeginAngle)
          return 1f;
        if ((double) angle >= (double) this.StabilityDropOffEndAngle)
          return 0.0f;
        float num = this.StabilityDropOffEndAngle - this.StabilityDropOffBeginAngle;
        return (double) num <= 0.0 ? 0.0f : Mathf.Clamp01((angle - this.StabilityDropOffBeginAngle) / num);
      }

      public Vector3 Normal { get; private set; }

      [NotNull]
      private float[] GetContactMultipliers(int contactCount)
      {
        if (this._contactMultipliers == null || this._contactMultipliers.Length < contactCount)
          this._contactMultipliers = new float[contactCount];
        for (int index = 0; index < contactCount; ++index)
          this._contactMultipliers[index] = 0.0f;
        return this._contactMultipliers;
      }

      public override void OnCollisionStay(Collision collision)
      {
        foreach (ContactPoint contact in collision.contacts)
          this._contacts.Add(contact);
      }

      public override void FixedUpdate()
      {
        if (this._contacts.Count <= 0)
        {
          this.Normal = Vector3.zero;
          this.Stability = 0.0f;
        }
        else
        {
          for (int index = 0; index < this._contacts.Count; ++index)
            Debug.DrawRay(this._contacts[index].point, this._contacts[index].normal, Color.white, 0.0f);
          this.CalculateNormal();
          this.CalculateStability();
          this.ClearContacts();
        }
      }

      private void CalculateNormal()
      {
        if (this._contacts.Count == 1)
        {
          this.Normal = this._contacts[0].normal;
        }
        else
        {
          this.Normal = Vector3.zero;
          Vector3 vector3 = -Physics.gravity.normalized;
          float[] contactMultipliers = this.GetContactMultipliers(this._contacts.Count);
          for (int index1 = 0; index1 < 3; ++index1)
          {
            for (int index2 = 0; index2 < this._contacts.Count; ++index2)
            {
              float num1 = contactMultipliers[index2];
              float num2 = Vector3.Dot(this._contacts[index2].normal, vector3 - this.Normal);
              contactMultipliers[index2] = Mathf.Clamp01(num1 + num2);
              this.Normal += (contactMultipliers[index2] - num1) * this._contacts[index2].normal;
            }
          }
          this.Normal.Normalize();
          float num = 0.0f;
          Vector3 zero = Vector3.zero;
          for (int index = 0; index < this._contacts.Count; ++index)
          {
            num += contactMultipliers[index];
            zero += this._contacts[index].point * contactMultipliers[index];
          }
          if ((double) num <= 0.0)
            return;
          Debug.DrawRay(zero / num, this.Normal, Color.red, 1f);
        }
      }

      private void CalculateStability()
      {
        this.Stability = this.GetStability(Vector3.Angle(this.Normal, -Physics.gravity));
      }

      public void ClearContacts() => this._contacts.Clear();
    }

    [Serializable]
    public class VelocityStabilityModule : BipedStability.StabilityModule
    {
      public float StabilityDropOffBeginVelocity = 11f;
      public float StabilityDropOffEndVelocity = 14f;
      [Range(0.0f, 10f)]
      public float RecoveryHalfLife = 1f;
      private float _maxRelativeVelocityMagnitude;

      private float GetStability(float velocity)
      {
        if ((double) velocity <= (double) this.StabilityDropOffBeginVelocity)
          return 1f;
        if ((double) velocity >= (double) this.StabilityDropOffEndVelocity)
          return 0.0f;
        float num = this.StabilityDropOffEndVelocity - this.StabilityDropOffBeginVelocity;
        return (double) num <= 0.0 ? 0.0f : Mathf.Clamp01((velocity - this.StabilityDropOffBeginVelocity) / num);
      }

      public override void OnCollisionStay(Collision collision)
      {
        for (int index = 0; index < collision.contacts.Length; ++index)
          this._maxRelativeVelocityMagnitude = Mathf.Max(this._maxRelativeVelocityMagnitude, collision.contacts[index].GetRelativeVelocity().magnitude);
      }

      public override void FixedUpdate()
      {
        this.Stability = HalfLife.GainLoss(this.Stability, this.GetStability(this._maxRelativeVelocityMagnitude), this.RecoveryHalfLife, 0.0f);
        this._maxRelativeVelocityMagnitude = 0.0f;
      }
    }
  }
}
