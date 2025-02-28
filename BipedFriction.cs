// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.BipedFriction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Common.Attributes;
using CodeHatch.Engine.Core.Cache;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class BipedFriction : EntityBehaviour
  {
    [ShowProperties]
    public PhysicMaterial standingMaterial;
    [ShowProperties]
    public PhysicMaterial tumbleMaterial;
    [ShowProperties]
    public PhysicMaterial movingMateiral;
    public float velocityForStandingMaterial = 0.1f;
    public float lerpAtHalfStability = 0.9f;
    private MotorBridge _myMotorBridge;
    private BipedBody _myBipedBody;
    private readonly List<Collider> _colliders = new List<Collider>();
    [ShowProperties]
    public PhysicMaterial materialUsed;
    private float t;
    private float t2;

    public MotorBridge MyMotorBridge
    {
      get
      {
        if ((Object) this._myMotorBridge == (Object) null)
          this._myMotorBridge = this.Entity.GetOrCreate<MotorBridge>();
        return this._myMotorBridge;
      }
    }

    public BipedBody MyBipedBody
    {
      get
      {
        if ((Object) this._myBipedBody == (Object) null)
          this._myBipedBody = this.Entity.Get<BipedBody>();
        return this._myBipedBody;
      }
    }

    public void Awake()
    {
      this.materialUsed = new PhysicMaterial();
      this.materialUsed.hideFlags = HideFlags.HideAndDontSave;
    }

    public void Start()
    {
      if (!this.Entity.IsLocallyOwned)
      {
        Object.Destroy((Object) this);
        this.enabled = false;
      }
      else
      {
        foreach (Collider collider in this.Entity.GetArray<Collider>())
        {
          if (!((Object) collider == (Object) null))
          {
            Rigidbody attachedRigidbody = collider.attachedRigidbody;
            if ((Object) this.MyBipedBody.Torso == (Object) attachedRigidbody || (Object) this.MyBipedBody.Legs == (Object) attachedRigidbody)
              this._colliders.Add(collider);
          }
        }
      }
    }

    public void FixedUpdate()
    {
      this.t = (float) (1.0 / (1.0 + (double) Mathf.Max(0.0f, this.MyMotorBridge.VelocityGround.magnitude / this.velocityForStandingMaterial)));
      if (!this.t.IsReal())
        this.t = 0.0f;
      this.materialUsed.Lerp(this.movingMateiral, this.standingMaterial, Mathf.Clamp01(this.t));
      this.t2 = Mathf.Pow(this.MyMotorBridge.Stability, -Mathf.Log(this.lerpAtHalfStability) / Mathf.Log(2f));
      if (!this.t2.IsReal())
        this.t2 = 0.0f;
      this.materialUsed.Lerp(this.tumbleMaterial, this.materialUsed, Mathf.Clamp01(this.t2));
      for (int index = 0; index < this._colliders.Count; ++index)
        this._colliders[index].sharedMaterial = this.materialUsed;
    }
  }
}
