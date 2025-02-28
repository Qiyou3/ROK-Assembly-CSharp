// Decompiled with JetBrains decompiler
// Type: AnimatorBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using JetBrains.Annotations;
using UnityEngine;

#nullable disable
public abstract class AnimatorBehaviour : EntityBehaviour
{
  private AnimatorBehaviourManager _manager;
  private Animator _animator;
  private Transform _avatarTransform;
  private CodeHatch.Engine.Modules.ScriptLOD.ScriptLOD _scriptLOD;
  public AnimatorWeighting Weighting;

  [CanBeNull]
  public AnimatorBehaviourManager Manager
  {
    get
    {
      if ((Object) this._manager == (Object) null)
      {
        Entity entity = this.TryGetEntity();
        if ((Object) entity != (Object) null)
          this._manager = entity.TryGet<AnimatorBehaviourManager>();
      }
      return this._manager;
    }
  }

  [CanBeNull]
  protected Animator MyAnimator
  {
    get
    {
      if ((Object) this._animator == (Object) null)
      {
        Entity entity = this.TryGetEntity();
        if ((Object) entity != (Object) null)
          this._animator = entity.TryGet<Animator>();
      }
      return this._animator;
    }
  }

  [CanBeNull]
  protected Transform AvatarTransform
  {
    get
    {
      if ((Object) this._avatarTransform == (Object) null && (Object) this.MyAnimator != (Object) null)
        this._avatarTransform = this.MyAnimator.transform;
      return this._avatarTransform;
    }
  }

  protected CodeHatch.Engine.Modules.ScriptLOD.ScriptLOD ScriptLOD
  {
    get
    {
      if ((Object) this._scriptLOD == (Object) null)
        this._scriptLOD = this.Entity.GetOrCreate<CodeHatch.Engine.Modules.ScriptLOD.ScriptLOD>();
      return this._scriptLOD;
    }
  }

  public virtual float Weight => this.Weighting.weight;

  public virtual float CalculateWeight() => this.Weighting.CalculateWeight(this.MyAnimator);

  public virtual void Start()
  {
  }

  public abstract void Initialize(AnimatorBehaviourManager manager);
}
