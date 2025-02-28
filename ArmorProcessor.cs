// Decompiled with JetBrains decompiler
// Type: ArmorProcessor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Inventory.Blueprints.Components;
using CodeHatch.Thrones.Hair;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UMA;
using UnityEngine;

#nullable disable
public class ArmorProcessor : MonoBehaviour
{
  public static ArmorProcessor Instance;
  private Texture2D _BlankTexture;
  private static UMAGeneratorBase _generator;
  private static List<ArmorProcessor.ApplyParameters> _parameters = new List<ArmorProcessor.ApplyParameters>();
  private static bool _isWaiting;

  private Texture2D BlankTexture
  {
    get
    {
      if ((UnityEngine.Object) this._BlankTexture == (UnityEngine.Object) null)
        this._BlankTexture = new Texture2D(1, 1);
      return this._BlankTexture;
    }
  }

  public static UMAGeneratorBase generator
  {
    get
    {
      if ((UnityEngine.Object) ArmorProcessor._generator == (UnityEngine.Object) null)
        ArmorProcessor._generator = (UMAGeneratorBase) GameObject.Find("UMAGenerator").GetComponent<UMAGenerator>();
      return ArmorProcessor._generator;
    }
  }

  public void ApplyArmor(
    ArmorManager manager,
    ArmorBlueprint.Slot armorSlot,
    ArmorBlueprint blueprint)
  {
    if ((UnityEngine.Object) manager == (UnityEngine.Object) null)
      this.LogError<ArmorProcessor>("manager == null");
    else if ((UnityEngine.Object) manager.DynamicAvatar == (UnityEngine.Object) null || this.ManagerIsWaiting(manager))
      this.StartCoroutine(this.WaitForAvatar(manager, armorSlot, blueprint));
    else
      this.Apply(manager, armorSlot, blueprint);
  }

  private void RecurseTransformsInPrefab(Transform root, List<Transform> transforms)
  {
    for (int index = 0; index < root.childCount; ++index)
    {
      Transform child = root.GetChild(index);
      transforms.Add(child);
      this.RecurseTransformsInPrefab(child, transforms);
    }
  }

  protected Transform[] GetTransformsInPrefab(Transform prefab)
  {
    List<Transform> transforms = new List<Transform>();
    this.RecurseTransformsInPrefab(prefab, transforms);
    return transforms.ToArray();
  }

  public SlotData UMASlotFromRenderer(SkinnedMeshRenderer armorRenderer)
  {
    if ((UnityEngine.Object) armorRenderer == (UnityEngine.Object) null)
      return (SlotData) null;
    SlotData slotData = new SlotData()
    {
      meshRenderer = armorRenderer
    };
    slotData.umaBoneData = (Transform[]) null;
    slotData.animatedBones = new Transform[0];
    slotData.materialSample = new Material(armorRenderer.sharedMaterial);
    slotData.materialSample.name = armorRenderer.name + " (Instance)";
    slotData.slotName = armorRenderer.gameObject.name + "_Slot";
    if ((UnityEngine.Object) slotData.meshRenderer != (UnityEngine.Object) null)
      slotData.umaBoneData = this.GetTransformsInPrefab(slotData.meshRenderer.rootBone);
    return slotData;
  }

  public OverlayData UMAOverlayFromRenderer(SkinnedMeshRenderer armorRenderer)
  {
    if ((UnityEngine.Object) armorRenderer == (UnityEngine.Object) null)
      return (OverlayData) null;
    List<Texture2D> texture2DList = new List<Texture2D>();
    foreach (string textureName in ArmorProcessor.generator.textureNameList)
    {
      Texture2D texture2D = (Texture2D) armorRenderer.sharedMaterial.GetTexture(textureName);
      if ((UnityEngine.Object) texture2D == (UnityEngine.Object) null)
        texture2D = this.BlankTexture;
      texture2DList.Add(texture2D);
    }
    return new OverlayData()
    {
      textureList = texture2DList.ToArray()
    };
  }

  private void Apply(ArmorManager manager, ArmorBlueprint.Slot armorSlot, ArmorBlueprint blueprint)
  {
    SlotData slotData = (SlotData) null;
    OverlayData overlayData = (OverlayData) null;
    if ((UnityEngine.Object) blueprint == (UnityEngine.Object) null)
    {
      foreach (ArmorManager.ArmorAssociation armorAssociation in manager.ArmorAssociations)
      {
        if (armorAssociation.ArmorSlot == armorSlot)
          blueprint = armorAssociation.DefaultArmorBlueprint;
      }
    }
    if ((UnityEngine.Object) blueprint != (UnityEngine.Object) null)
    {
      slotData = this.UMASlotFromRenderer(blueprint.ArmorRenderer);
      overlayData = this.UMAOverlayFromRenderer(blueprint.ArmorRenderer);
      if (blueprint.UMASlotName != string.Empty)
        slotData = UMAContext.Instance.InstantiateSlot(blueprint.UMASlotName);
      if (blueprint.UMAOverlayName != string.Empty)
        overlayData = UMAContext.Instance.InstantiateOverlay(blueprint.UMAOverlayName);
    }
    int index1 = 0;
    ArmorBlueprint previousBlueprint = (ArmorBlueprint) null;
    ArmorAlternativeMesh armorAlternativeMesh = !((UnityEngine.Object) blueprint == (UnityEngine.Object) null) ? blueprint.ArmorAlternativeMesh : (ArmorAlternativeMesh) null;
    for (int index2 = 0; index2 < manager.ArmorAssociations.Length; ++index2)
    {
      ArmorManager.ArmorAssociation armorAssociation = manager.ArmorAssociations[index2];
      if ((UnityEngine.Object) armorAlternativeMesh != (UnityEngine.Object) null)
      {
        foreach (ArmorAlternativeMesh.MeshForEquippedBlueprints equippedBlueprint in armorAlternativeMesh.MeshesForEquippedBlueprints)
        {
          foreach (UnityEngine.Object blueprint1 in equippedBlueprint.Blueprints)
          {
            if (!(blueprint1 != (UnityEngine.Object) armorAssociation.CurrentBlueprint))
            {
              slotData = this.UMASlotFromRenderer(equippedBlueprint.Mesh);
              overlayData = this.UMAOverlayFromRenderer(equippedBlueprint.Mesh);
            }
          }
        }
        if (armorAlternativeMesh.SlotOverlaysForEquippedBlueprints != null)
        {
          for (int index3 = 0; index3 < armorAlternativeMesh.SlotOverlaysForEquippedBlueprints.Length; ++index3)
          {
            for (int index4 = 0; index4 < armorAlternativeMesh.SlotOverlaysForEquippedBlueprints[index3].Blueprints.Length; ++index4)
            {
              if (!((UnityEngine.Object) armorAlternativeMesh.SlotOverlaysForEquippedBlueprints[index3].Blueprints[index4] != (UnityEngine.Object) armorAssociation.CurrentBlueprint))
              {
                slotData = armorAlternativeMesh.SlotOverlaysForEquippedBlueprints[index3].Slot;
                overlayData = armorAlternativeMesh.SlotOverlaysForEquippedBlueprints[index3].Overlay;
              }
            }
          }
        }
      }
      if ((UnityEngine.Object) armorAssociation.CurrentBlueprint != (UnityEngine.Object) null && armorAssociation.CurrentBlueprint.HiddenArmorSlot == armorSlot)
        slotData = (SlotData) null;
      if (armorAssociation.ArmorSlot == armorSlot)
      {
        index1 = index2;
        previousBlueprint = armorAssociation.CurrentBlueprint;
        armorAssociation.CurrentBlueprint = blueprint;
      }
    }
    this.ReapplyAlternativeMeshBlueprint(manager, blueprint, previousBlueprint);
    CharacterHair characterHair = manager.Entity.Get<CharacterHair>();
    if ((UnityEngine.Object) characterHair != (UnityEngine.Object) null)
    {
      ArmorHidesHair armorHidesHair1 = !((UnityEngine.Object) blueprint == (UnityEngine.Object) null) ? blueprint.ArmorHidesHair : (ArmorHidesHair) null;
      ArmorHidesHair armorHidesHair2 = !((UnityEngine.Object) previousBlueprint == (UnityEngine.Object) null) ? previousBlueprint.ArmorHidesHair : (ArmorHidesHair) null;
      if ((UnityEngine.Object) armorHidesHair1 != (UnityEngine.Object) null && armorHidesHair1.HideHair)
        characterHair.HideHair();
      else if ((UnityEngine.Object) armorHidesHair2 != (UnityEngine.Object) null && armorHidesHair2.HideHair)
        characterHair.ShowHair();
      if ((UnityEngine.Object) armorHidesHair1 != (UnityEngine.Object) null && armorHidesHair1.HideBeard)
        characterHair.HideBeard();
      else if ((UnityEngine.Object) armorHidesHair2 != (UnityEngine.Object) null && armorHidesHair2.HideBeard)
        characterHair.ShowBeard();
      bool flag = false;
      if ((UnityEngine.Object) armorHidesHair1 != (UnityEngine.Object) null && !armorHidesHair1.HideHair && armorHidesHair1.ReplaceableHairStyles != null)
      {
        for (int index5 = 0; index5 < armorHidesHair1.ReplaceableHairStyles.Length; ++index5)
        {
          if (characterHair.StyleIndex == armorHidesHair1.ReplaceableHairStyles[index5])
          {
            flag = true;
            characterHair.SetTempHair(armorHidesHair1.HairToReplaceWith);
            break;
          }
        }
      }
      if (!flag && (UnityEngine.Object) armorHidesHair2 != (UnityEngine.Object) null && armorHidesHair2.ReplaceableHairStyles != null && armorHidesHair2.ReplaceableHairStyles.Length > 0)
      {
        for (int index6 = 0; index6 < armorHidesHair2.ReplaceableHairStyles.Length; ++index6)
        {
          if (characterHair.StyleIndex == armorHidesHair2.ReplaceableHairStyles[index6])
          {
            characterHair.SetTempHair(characterHair.StyleIndex);
            break;
          }
        }
      }
    }
    if (manager.DynamicAvatar.umaData.umaRecipe.slotDataList == null)
      manager.DynamicAvatar.umaData.umaRecipe.slotDataList = new SlotData[manager.ArmorAssociations.Length];
    if (manager.DynamicAvatar.umaData.umaRecipe.slotDataList.Length < manager.ArmorAssociations.Length)
      Array.Resize<SlotData>(ref manager.DynamicAvatar.umaData.umaRecipe.slotDataList, manager.ArmorAssociations.Length);
    manager.DynamicAvatar.umaData.umaRecipe.slotDataList[index1] = slotData;
    if ((UnityEngine.Object) overlayData != (UnityEngine.Object) null && (UnityEngine.Object) slotData != (UnityEngine.Object) null)
      manager.DynamicAvatar.umaData.umaRecipe.slotDataList[index1].AddOverlay(overlayData);
    manager.DynamicAvatar.umaGenerator = ArmorProcessor.generator;
    manager.DynamicAvatar.umaData.umaGenerator = ArmorProcessor.generator;
    manager.RecalculatePerformance();
    if (manager.Entity.IsLocalPlayer)
      ArmorMenu.UpdateDefense();
    manager.RequiresUpdate = true;
  }

  private void ReapplyAlternativeMeshBlueprint(
    ArmorManager manager,
    ArmorBlueprint blueprint,
    ArmorBlueprint previousBlueprint)
  {
    foreach (ArmorManager.ArmorAssociation armorAssociation in manager.ArmorAssociations)
    {
      if (!((UnityEngine.Object) armorAssociation.CurrentBlueprint == (UnityEngine.Object) null))
      {
        ArmorAlternativeMesh armorAlternativeMesh = armorAssociation.CurrentBlueprint.ArmorAlternativeMesh;
        if (!((UnityEngine.Object) armorAlternativeMesh == (UnityEngine.Object) null))
        {
          foreach (ArmorAlternativeMesh.MeshForEquippedBlueprints equippedBlueprint in armorAlternativeMesh.MeshesForEquippedBlueprints)
          {
            foreach (ArmorBlueprint blueprint1 in equippedBlueprint.Blueprints)
            {
              if (!((UnityEngine.Object) blueprint1 != (UnityEngine.Object) blueprint) || !((UnityEngine.Object) blueprint1 != (UnityEngine.Object) previousBlueprint))
                this.ApplyArmor(manager, armorAssociation.CurrentBlueprint.ArmorSlot, armorAssociation.CurrentBlueprint);
            }
          }
          if (armorAlternativeMesh.SlotOverlaysForEquippedBlueprints != null)
          {
            for (int index1 = 0; index1 < armorAlternativeMesh.SlotOverlaysForEquippedBlueprints.Length; ++index1)
            {
              for (int index2 = 0; index2 < armorAlternativeMesh.SlotOverlaysForEquippedBlueprints[index1].Blueprints.Length; ++index2)
              {
                if (!((UnityEngine.Object) armorAlternativeMesh.SlotOverlaysForEquippedBlueprints[index1].Blueprints[index2] != (UnityEngine.Object) blueprint) || !((UnityEngine.Object) armorAlternativeMesh.SlotOverlaysForEquippedBlueprints[index1].Blueprints[index2] != (UnityEngine.Object) previousBlueprint))
                  this.ApplyArmor(manager, armorAssociation.CurrentBlueprint.ArmorSlot, armorAssociation.CurrentBlueprint);
              }
            }
          }
        }
      }
    }
  }

  private bool ManagerIsWaiting(ArmorManager manager)
  {
    for (int index = 0; index < ArmorProcessor._parameters.Count; ++index)
    {
      if (ArmorProcessor._parameters[index] != null && (UnityEngine.Object) ArmorProcessor._parameters[index].Manager == (UnityEngine.Object) manager)
        return true;
    }
    return false;
  }

  [DebuggerHidden]
  private IEnumerator WaitForAvatar(
    ArmorManager manager,
    ArmorBlueprint.Slot slot,
    ArmorBlueprint blueprint)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ArmorProcessor.\u003CWaitForAvatar\u003Ec__IteratorD4()
    {
      manager = manager,
      slot = slot,
      blueprint = blueprint,
      \u003C\u0024\u003Emanager = manager,
      \u003C\u0024\u003Eslot = slot,
      \u003C\u0024\u003Eblueprint = blueprint,
      \u003C\u003Ef__this = this
    };
  }

  public void Awake() => ArmorProcessor.Instance = this;

  private class ApplyParameters
  {
    public ArmorManager Manager;
    public ArmorBlueprint.Slot Slot;
    public ArmorBlueprint Blueprint;
  }
}
