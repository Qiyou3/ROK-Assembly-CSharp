// Decompiled with JetBrains decompiler
// Type: ArmorFullyEquippedBonus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class ArmorFullyEquippedBonus : EntityBehaviour, IArmorBonus
{
  [Range(0.0f, 100f)]
  public float FullEquippedStatusBoostPercent;
  public int OnlyCheckUpToSlotNumber = 5;
  private ArmorManager _ArmorManager;

  public ArmorManager ArmorManager
  {
    get
    {
      if ((Object) this._ArmorManager == (Object) null)
        this._ArmorManager = this.Entity.TryGet<ArmorManager>();
      return this._ArmorManager;
    }
  }

  public bool BonusActive { get; protected set; }

  public void RecalculateBonus()
  {
    for (int index1 = 0; index1 < Mathf.Min(this.ArmorManager.EquipSlotToArmorSlot.Length, this.OnlyCheckUpToSlotNumber); ++index1)
    {
      bool flag = false;
      for (int index2 = 0; index2 < this.ArmorManager.ArmorAssociations.Length; ++index2)
      {
        ArmorManager.ArmorAssociation armorAssociation = this.ArmorManager.ArmorAssociations[index2];
        if (!((Object) armorAssociation.CurrentBlueprint == (Object) armorAssociation.DefaultArmorBlueprint))
        {
          for (int index3 = 0; index3 < this.ArmorManager.EquipSlotToArmorSlot[index1].Length; ++index3)
          {
            if (this.ArmorManager.EquipSlotToArmorSlot[index1][index3] == armorAssociation.ArmorSlot)
            {
              flag = true;
              break;
            }
          }
          if (flag)
            break;
        }
      }
      if (!flag)
      {
        this.BonusActive = false;
        return;
      }
    }
    this.BonusActive = true;
  }

  public float ModifyDefense(float defense)
  {
    return !this.BonusActive ? defense : defense * (float) (1.0 + (double) this.FullEquippedStatusBoostPercent / 100.0);
  }

  public string BonusMessage() => !this.BonusActive ? string.Empty : "Fully Equipped +10% Defense";
}
