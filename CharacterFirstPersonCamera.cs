// Decompiled with JetBrains decompiler
// Type: CharacterFirstPersonCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Cache;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class CharacterFirstPersonCamera : RotationShakeRunInEditor, IEntityAware
{
  private GetEntity _entity;
  public Transform cameraAnchor;
  public LookBridge lookBridge;

  public Entity Entity
  {
    get
    {
      if (this._entity == null)
        this._entity = new GetEntity((MonoBehaviour) this);
      return this._entity.Get();
    }
  }

  public void Start()
  {
    CharacterDefinition characterDefinition = this.Entity.GetOrCreate<CharacterDefinition>();
    if ((Object) characterDefinition == (Object) null)
    {
      this.enabled = false;
    }
    else
    {
      this.cameraAnchor = characterDefinition.GetTransform(CharacterDefinition.Part.EyesCenter);
      this.shake = this.Entity.Get<MainRigidbody>().transform;
      this.lookBridge = this.Entity.GetOrCreate<LookBridge>();
    }
  }

  public new void LateUpdate()
  {
    this.transform.position = this.cameraAnchor.position;
    this.target = (Transform) null;
    this.targetRotation = this.lookBridge.Rotation;
    if (this.Entity.Has<GunShake>())
      this.targetRotation = this.Entity.Get<GunShake>().ProcessRotation(this.targetRotation);
    base.LateUpdate();
  }
}
