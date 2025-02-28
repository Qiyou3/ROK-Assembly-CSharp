// Decompiled with JetBrains decompiler
// Type: CharacterDefinition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Common.Attributes;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class CharacterDefinition : EntityDefinitionBase
{
  [WarnNull]
  public Animator animator;
  [WarnNull]
  public Transform animationRoot;
  [EnumList(typeof (HumanBodyBones))]
  public Transform[] humanBodyBones;
  [EnumList(typeof (CharacterDefinition.Part))]
  public Transform[] parts;

  public Animator MyAnimator
  {
    get
    {
      this.EnsureValid((bool) (Object) this.animator);
      return this.animator;
    }
  }

  public Transform AnimationRoot
  {
    get
    {
      this.EnsureValid((bool) (Object) this.animationRoot);
      return this.animationRoot;
    }
  }

  protected override void Validate()
  {
    if ((Object) this.animator == (Object) null)
    {
      this.RecordIssue("Specific animator to use for this character definition wasn't provided; attempting to find one.", DebugMessage.MessageType.Warning);
      this.animator = this.Entity.TryGet<Animator>();
    }
    if ((Object) this.animator != (Object) null)
    {
      Transform[] transformArray = this.humanBodyBones;
      if (this.humanBodyBones == null || this.humanBodyBones.Length < 54)
        transformArray = new Transform[54];
      if (this.animator.isHuman)
      {
        if (this.humanBodyBones != null && this.humanBodyBones.Length != 0)
          this.LogWarning<CharacterDefinition>("HumanBodyBone overrides were provided for this character definition, but this is only valid for non-humanoid characters. The default values provided by animator.GetBoneTransform() will be used.");
        for (int humanBoneId = 0; humanBoneId < 54; ++humanBoneId)
          transformArray[humanBoneId] = this.animator.GetBoneTransform((HumanBodyBones) humanBoneId);
      }
      else
      {
        for (int humanBoneId = 0; humanBoneId < 54; ++humanBoneId)
          transformArray[humanBoneId] = this.humanBodyBones == null || humanBoneId >= this.humanBodyBones.Length || !((Object) this.humanBodyBones[humanBoneId] != (Object) null) ? this.animator.GetBoneTransform((HumanBodyBones) humanBoneId) : this.humanBodyBones[humanBoneId];
      }
      this.humanBodyBones = transformArray;
    }
    if (!((Object) this.animationRoot == (Object) null))
      return;
    this.RecordIssue("Specific animationRoot to use for this character definition wasn't provided; attempting to find one.", DebugMessage.MessageType.Warning);
    if ((Object) this.animator != (Object) null)
    {
      this.animationRoot = this.animator.transform;
    }
    else
    {
      Animation animation = this.Entity.TryGet<Animation>();
      if ((Object) animation != (Object) null)
        this.animationRoot = animation.transform;
      else
        this.animationRoot = this.Entity.MainTransform;
    }
  }

  public Transform GetTransform(HumanBodyBones bone)
  {
    if (this.humanBodyBones == null)
      return (Transform) null;
    this.EnsureValid((bool) (Object) this.animator);
    int index = (int) bone;
    if (this.humanBodyBones.Length > index)
    {
      Transform humanBodyBone = this.humanBodyBones[index];
      if ((Object) humanBodyBone != (Object) null)
        return humanBodyBone;
    }
    if ((Object) this.MyAnimator != (Object) null)
      return this.MyAnimator.GetBoneTransform(bone);
    this.LogWarning<CharacterDefinition>("Could not find the bone, \"{0}\", using the MainTransform instead.", (object) bone);
    return (Transform) (GameObjectAttribute<Transform>) this.Entity.Get<MainTransform>();
  }

  public Transform GetTransform(CharacterDefinition.Part bone)
  {
    int index = (int) bone;
    if (this.parts != null && this.parts.Length > index)
    {
      Transform part = this.parts[index];
      if (this.parts != null)
        return part;
    }
    return (Transform) (GameObjectAttribute<Transform>) this.Entity.Get<MainTransform>();
  }

  public HumanBodyBones GetHumanBodyBone(Transform myTransform)
  {
    this.EnsureValid((object) this.humanBodyBones);
    Transform parent = myTransform.parent;
    HumanBodyBones humanBodyBone1 = HumanBodyBones.LastBone;
    if (this.humanBodyBones != null)
    {
      for (int humanBodyBone2 = 0; humanBodyBone2 < this.humanBodyBones.Length; ++humanBodyBone2)
      {
        if ((Object) myTransform == (Object) this.humanBodyBones[humanBodyBone2])
          return (HumanBodyBones) humanBodyBone2;
        if ((Object) parent == (Object) this.humanBodyBones[humanBodyBone2])
          humanBodyBone1 = (HumanBodyBones) humanBodyBone2;
      }
    }
    return humanBodyBone1;
  }

  public enum Part
  {
    EyesCenter,
    LeftEar,
    RightEar,
    Mouth,
  }
}
