// Decompiled with JetBrains decompiler
// Type: CodeHatch.Armor.ArmorDNAAdjuster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Inventory.Blueprints.Components;
using CodeHatch.Networking.Events;
using System;
using UMA;

#nullable disable
namespace CodeHatch.Armor
{
  public class ArmorDNAAdjuster : EntityBehaviour
  {
    private UMADynamicAvatar dynamicAvatar;
    private UMADnaHumanoid baseDNA;
    private ArmorManager manager;

    public void Start()
    {
      this.dynamicAvatar = this.Entity.Get<UMADynamicAvatar>();
      this.baseDNA = new UMADnaHumanoid();
      this.baseDNA = DNACopier.CopyDNAOver(this.baseDNA, this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>());
      this.manager = this.GetComponent<ArmorManager>();
      this.manager.OnArmorApplied += new Action<ArmorBlueprint.Slot, ArmorBlueprint>(this.OnArmorApplied);
      EventManager.Subscribe<UMADNASendEvent>(new EventSubscriber<UMADNASendEvent>(this.OnDNAUpdate));
    }

    public void OnDestroy()
    {
      EventManager.Unsubscribe<UMADNASendEvent>(new EventSubscriber<UMADNASendEvent>(this.OnDNAUpdate));
    }

    private void OnDNAUpdate(UMADNASendEvent e)
    {
      if (!((UnityEngine.Object) e.Entity == (UnityEngine.Object) this.Entity) || !e.HasDNA)
        return;
      this.baseDNA = DNACopier.CopyDNAOver(this.baseDNA, e.DNA);
    }

    private void OnArmorApplied(ArmorBlueprint.Slot slot, ArmorBlueprint blueprint)
    {
      bool flag = (UnityEngine.Object) blueprint == (UnityEngine.Object) null;
      if (slot != ArmorBlueprint.Slot.Head && slot != ArmorBlueprint.Slot.Helmet)
        return;
      if (flag)
      {
        this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().earsPosition = this.baseDNA.earsPosition;
        this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().earsRotation = this.baseDNA.earsRotation;
        this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().earsSize = this.baseDNA.earsSize;
        this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().noseCurve = this.baseDNA.noseCurve;
        this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().noseFlatten = this.baseDNA.noseFlatten;
        this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().noseInclination = this.baseDNA.noseInclination;
        this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().nosePronounced = this.baseDNA.nosePronounced;
        this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().noseSize = this.baseDNA.noseSize;
        this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().noseWidth = this.baseDNA.noseWidth;
        this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().jawsSize = this.baseDNA.jawsSize;
        this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().jawsPosition = this.baseDNA.jawsPosition;
        this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().mandibleSize = this.baseDNA.mandibleSize;
        this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().cheekSize = this.baseDNA.cheekSize;
        this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().lowCheekPronounced = this.baseDNA.lowCheekPronounced;
      }
      else
      {
        if (blueprint.AdjustEars)
        {
          this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().earsPosition = 0.0f;
          this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().earsRotation = 0.0f;
          this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().earsSize = 0.0f;
        }
        if (blueprint.AdjustNose)
        {
          this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().noseCurve = 0.0f;
          this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().noseFlatten = 0.0f;
          this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().noseInclination = 0.0f;
          this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().nosePronounced = 0.0f;
          this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().noseSize = 0.0f;
          this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().noseWidth = 0.0f;
        }
        if (blueprint.AdjustJaw)
        {
          this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().jawsSize = 0.0f;
          this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().jawsPosition = 1f;
        }
        if (blueprint.AdjustMandible)
          this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().mandibleSize = 0.0f;
        if (blueprint.AdjustCheek)
          this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().cheekSize = 0.0f;
        if (!blueprint.AdjustLowCheek)
          return;
        this.dynamicAvatar.umaData.GetDna<UMADnaHumanoid>().lowCheekPronounced = 0.5f;
      }
    }
  }
}
