// Decompiled with JetBrains decompiler
// Type: InputWeaponController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Cache;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Input;
using UnityEngine;

#nullable disable
public class InputWeaponController : MonoBehaviour, IEntityAware
{
  private GetEntity _entity;
  public KeyButton fireButton = new KeyButton("Attack", KeyCode.Mouse0);
  public KeyButton altFireButton = new KeyButton("Defend", KeyCode.Mouse1);

  public Entity Entity
  {
    get
    {
      if (this._entity == null)
        this._entity = new GetEntity((MonoBehaviour) this);
      return this._entity.Get();
    }
  }

  private IHealth Health => this.Entity.Get<IHealth>();

  private WeaponBridge MyWeaponBridge => this.Entity.GetOrCreate<WeaponBridge>();

  public bool IsFiring
  {
    set => this.MyWeaponBridge.IsFiring = value;
  }

  public bool IsAltFiring
  {
    set => this.MyWeaponBridge.IsAltFiring = value;
  }

  public virtual void Update()
  {
    if (!this.Entity.IsLocallyOwned)
      return;
    if (this.Health != null && this.Health.IsDead)
    {
      this.IsFiring = false;
      this.IsAltFiring = false;
    }
    else
    {
      this.IsFiring = this.fireButton.Get();
      this.IsAltFiring = this.altFireButton.Get();
    }
  }
}
