// Decompiled with JetBrains decompiler
// Type: AttachToAvatarBone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class AttachToAvatarBone : EntityBehaviour
{
  public HumanBodyBones boneID;
  private Transform _transform;

  private Transform _transformToFollow
  {
    get
    {
      if ((Object) this._transform == (Object) null)
        this._transform = this.Entity.GetOrCreate<CharacterDefinition>().GetTransform(this.boneID);
      return this._transform;
    }
    set => this._transform = value;
  }

  public void Start()
  {
    this._transformToFollow = this.Entity.GetOrCreate<CharacterDefinition>().GetTransform(this.boneID);
    if ((Object) this._transformToFollow == (Object) null)
    {
      this.LogDebug<AttachToAvatarBone>("Could not attach to bone of type {0}.", (object) this.boneID);
      this.enabled = false;
    }
    else
    {
      if ((Object) this.GetComponent<Collider>() != (Object) null)
        this.LogError<AttachToAvatarBone>("A collider was found on a gameobject using {0}. This may not be performant.", (object) this.GetType().Name);
      this.transform.localPosition = Vector3.zero;
      this.transform.localRotation = Quaternion.identity;
      this.transform.localScale = Vector3.one;
    }
  }

  public void FixedUpdate() => this.transform.position = this._transformToFollow.position;
}
