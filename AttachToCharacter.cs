// Decompiled with JetBrains decompiler
// Type: AttachToCharacter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class AttachToCharacter : EntityBehaviour
{
  public CharacterDefinition.Part part;
  private Transform _transformToFollow;

  public void Start()
  {
    this._transformToFollow = this.Entity.GetOrCreate<CharacterDefinition>().GetTransform(this.part);
    if (!(bool) (Object) this._transformToFollow)
    {
      this.LogError<AttachToCharacter>("No {0} transform found to follow.", (object) this.part);
      this.enabled = false;
    }
    else
    {
      if ((Object) this.GetComponent<Collider>() != (Object) null)
        this.LogError<AttachToCharacter>("A collider was found on a gameobject using {0}. This may not be performant.", (object) this.GetType().Name);
      this.transform.localPosition = Vector3.zero;
      this.transform.localRotation = Quaternion.identity;
      this.transform.localScale = Vector3.one;
    }
  }

  public void FixedUpdate() => this.transform.position = this._transformToFollow.position;
}
