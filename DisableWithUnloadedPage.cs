// Decompiled with JetBrains decompiler
// Type: DisableWithUnloadedPage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using UnityEngine;

#nullable disable
public class DisableWithUnloadedPage : EntityBehaviour
{
  public Object[] ObjectsToDisable;
  private bool _objectsEnabled = true;

  private bool ObjectsEnabled
  {
    set
    {
      if (this._objectsEnabled == value)
        return;
      this._objectsEnabled = value;
      for (int index = 0; index < this.ObjectsToDisable.Length; ++index)
        this.ObjectsToDisable[index].SetEnable(value);
    }
  }

  public void Start()
  {
    if (Player.IsLocalDedi)
    {
      this.LogInfo<DisableWithUnloadedPage>("Destroying DisableWithDistance since we are the dedicated server.");
      Object.Destroy((Object) this);
      this.enabled = false;
    }
    else if (this.Entity.IsLocallyOwned)
    {
      this.LogInfo<DisableWithUnloadedPage>("Destroying DisableWithDistance since we own this object.");
      Object.Destroy((Object) this);
      this.enabled = false;
    }
    else
      this.Entity.GetOrCreate<DisableWithUnloadedPageManager>();
  }

  public void OnPageStatusRefresh(bool isInLoadedPage)
  {
    if (!this.enabled)
      return;
    this.ObjectsEnabled = isInLoadedPage;
  }
}
