// Decompiled with JetBrains decompiler
// Type: CharacterImmunity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Common.Attributes;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using CodeHatch.Networking.Sync;
using UnityEngine;

#nullable disable
[Syncable]
public class CharacterImmunity : EntityBehaviour
{
  [Syncable(0.03f)]
  [NoEdit]
  private bool _isHidden;
  [Syncable(0.03f)]
  [NoEdit]
  private bool _isImmune;
  private bool _hiddenLastFrame;
  private bool _immuneLastFrame;

  public bool IsHidden
  {
    get => this._isHidden;
    set => this._isHidden = value;
  }

  public bool IsImmune
  {
    get => this._isImmune;
    set => this._isImmune = value;
  }

  public bool IsProtected
  {
    get => this.IsHidden && this.IsImmune;
    set
    {
      bool flag = value;
      this.IsImmune = flag;
      this.IsHidden = flag;
    }
  }

  private Renderer[] _renderers => this.Entity.TryGetArray<Renderer>();

  public void Start() => SyncManager.Register<CharacterImmunity>(this);

  public void OnDestroy() => SyncManager.Unregister((object) this);

  public void Update()
  {
    if (this._hiddenLastFrame != this.IsHidden && !this.Entity.IsLocalPlayer)
    {
      for (int index = 0; index < this._renderers.Length; ++index)
      {
        Renderer renderer = this._renderers[index];
        if (renderer.enabled == this.IsHidden)
          renderer.enabled = !this.IsHidden;
      }
    }
    if (this._immuneLastFrame != this.IsImmune && Player.IsLocalServer)
    {
      if ((Object) this.Entity == (Object) null)
        return;
      this.Entity.Owner.SetImmune(this.IsImmune);
    }
    this._hiddenLastFrame = this.IsHidden;
    this._immuneLastFrame = this.IsImmune;
  }
}
