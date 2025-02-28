// Decompiled with JetBrains decompiler
// Type: ArmorAlternativeMesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Inventory.Blueprints.Components;
using System;
using UMA;
using UnityEngine;

#nullable disable
public class ArmorAlternativeMesh : MonoBehaviour
{
  public ArmorAlternativeMesh.MeshForEquippedBlueprints[] MeshesForEquippedBlueprints;
  public ArmorAlternativeMesh.SlotOverlayForEquippedBlueprints[] SlotOverlaysForEquippedBlueprints;

  [Serializable]
  public class MeshForEquippedBlueprints
  {
    public ArmorBlueprint[] Blueprints;
    public SkinnedMeshRenderer Mesh;
  }

  [Serializable]
  public class SlotOverlayForEquippedBlueprints
  {
    public ArmorBlueprint[] Blueprints;
    public SlotData Slot;
    public OverlayData Overlay;
  }
}
