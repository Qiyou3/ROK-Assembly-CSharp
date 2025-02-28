// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.NpcConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class NpcConfig : uLinkEntityBehaviour
  {
    public static List<Transform> NpcTransforms = new List<Transform>();
    public float AttackForce = 20f;
    public float AttackReach = 5f;
    public int OpposingTeamNumber = 1;
    public float MoveSpeed = 5f;
    public float AvoidVelocityDivder = 2f;
    public float SlowdownDistance = 3f;
    public float StopDistance = 1f;
    public float ViewAngle = 60f;
    public float ViewDistance = 30f;
    public float NightMoveSpeed = 15f;
    public float NightViewAngle = 100f;
    public float NightViewDistance = 60f;
    public float InstantTargetDistance = 3f;
    public Color Color;
    public Color DeadColor;
    public string SpecialityShaderVariableName;

    public void Start()
    {
      NpcConfigInitData initData = this.Entity.TryGetInitData<NpcConfigInitData>();
      if (initData == null)
        return;
      this.TrySetMaxHealth(initData.maxHealth);
      this.transform.position += this.transform.localScale.y * Vector3.up * 4f;
      this.MoveSpeed = initData.moveSpeed;
      this.AttackForce = initData.attackForce;
      this.Color = initData.color;
      if (initData.autoTargetOnSpawn)
        this.TryAutoTarget();
      this.ChangeColor(this.Color);
    }

    private void TrySetMaxHealth(float maxHealth)
    {
      this.Entity.Get<IHealth>()?.UpdateMaxHealth(maxHealth, true);
    }

    private void TryAutoTarget()
    {
      PrimalBrain primalBrain = this.Entity.Get<PrimalBrain>();
      if (!((Object) primalBrain != (Object) null))
        return;
      primalBrain.AutoTarget();
    }

    public void Update()
    {
      IHealth health = this.Entity.TryGet<IHealth>();
      if (health == null || !health.IsDead)
        return;
      this.ChangeColor(this.DeadColor);
    }

    public void ChangeColor(Color color)
    {
      SkinnedMeshRenderer skinnedMeshRenderer = this.Entity.TryGet<SkinnedMeshRenderer>();
      if (!((Object) skinnedMeshRenderer != (Object) null))
        return;
      skinnedMeshRenderer.material.SetColor(this.SpecialityShaderVariableName, this.DeadColor);
    }
  }
}
