// Decompiled with JetBrains decompiler
// Type: EnableWhenEquipped
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Inventory.Blueprints;
using CodeHatch.UserInterface;
using CodeHatch.UserInterface.HUD;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnableWhenEquipped : MonoBehaviour
{
  public InvItemBlueprint[] Blueprints;
  public GameObject[] Targets;
  public MaintainHUD[] HUDs = new MaintainHUD[0];
  private bool _prevShouldShow;

  public void Update()
  {
    bool flag = this.ShouldShowHuds();
    if (flag != this._prevShouldShow)
    {
      if (this.ShouldShowHuds())
      {
        for (int index = 0; index < this.HUDs.Length; ++index)
          GUIManager.OpenHUD(this.HUDs[index]);
      }
      else
      {
        for (int index = 0; index < this.HUDs.Length; ++index)
          GUIManager.CloseHUD(this.HUDs[index]);
      }
    }
    this._prevShouldShow = flag;
  }

  private bool ShouldShowHuds()
  {
    if ((Object) SingletonMonoBehaviour<InvEquipment>.Instance == (Object) null || (bool) (Object) SingletonMonoBehaviour<InvEquipment>.Instance.Equipment)
      return false;
    bool flag = false;
    List<InvGameItemStack> equippedItems = SingletonMonoBehaviour<InvEquipment>.Instance.Equipment.EquippedItems;
    for (int index = 0; index < equippedItems.Count; ++index)
    {
      if (((IEnumerable<InvItemBlueprint>) this.Blueprints).ContainsItem<InvItemBlueprint>(equippedItems[index].Blueprint))
      {
        flag = true;
        break;
      }
    }
    return flag && Entity.LocalPlayerExists && Entity.LocalPlayer.gameObject.activeSelf;
  }

  public void DoEnable(bool enabled)
  {
    foreach (GameObject target in this.Targets)
      target.SetActive(enabled);
    if (this.HUDs == null)
      return;
    for (int index = 0; index < this.HUDs.Length; ++index)
    {
      if (enabled)
        GUIManager.OpenHUD(this.HUDs[index]);
      else
        GUIManager.CloseHUD(this.HUDs[index]);
    }
  }
}
