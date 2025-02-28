// Decompiled with JetBrains decompiler
// Type: CharacterChaseCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Cache;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class CharacterChaseCamera : CameraChaseTrace, IEntityAware
{
  private GetEntity _entity;

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
    this.mount = this.Entity.GetOrCreate<CharacterDefinition>().GetTransform(CharacterDefinition.Part.EyesCenter);
    if (!((Object) this.lookAt == (Object) null))
      return;
    this.lookAt = this.mount;
  }
}
