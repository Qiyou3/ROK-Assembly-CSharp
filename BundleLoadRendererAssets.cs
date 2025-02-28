// Decompiled with JetBrains decompiler
// Type: BundleLoadRendererAssets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

#nullable disable
public class BundleLoadRendererAssets : MonoBehaviour
{
  public System.Action OnComplete;
  public List<BundleLoadRendererAssets.AssetUsage> assetUsages = new List<BundleLoadRendererAssets.AssetUsage>();
  public Bundle bundle;
  public string bundleName;
  [NonSerialized]
  internal bool done;
  [NonSerialized]
  internal bool loading;

  public void Start()
  {
    if (Player.IsLocalDedi)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
      this.enabled = false;
    }
    else
      BundleManager.Instance.LoadRendererAssets(this);
  }

  public void ApplyAssetUsages(BundleRendererAssets bundleRendererAssets)
  {
    foreach (BundleLoadRendererAssets.AssetUsage assetUsage in this.assetUsages)
      assetUsage.Apply(bundleRendererAssets);
    if (this.OnComplete == null)
      return;
    this.OnComplete();
    this.OnComplete = (System.Action) null;
  }

  [Serializable]
  public class AssetUsage
  {
    public string assetName;
    public string assetTypeName;
    public BundleLoadRendererAssets.AssetUsage.ObjectUsage objectUsage;

    public AssetUsage()
    {
    }

    public AssetUsage(
      string assetName,
      string assetTypeName,
      BundleLoadRendererAssets.AssetUsage.ObjectUsage objectUsage)
    {
      this.assetName = assetName;
      this.assetTypeName = assetTypeName;
      this.objectUsage = objectUsage;
    }

    public AssetUsage(string assetName, string assetTypeName, UnityEngine.Object objectInHierarchy, int index)
    {
      this.assetName = assetName;
      this.assetTypeName = assetTypeName;
      this.objectUsage = new BundleLoadRendererAssets.AssetUsage.ObjectUsage(objectInHierarchy, index);
      switch (objectInHierarchy)
      {
        case Behaviour _:
          this.objectUsage.isDisabled = !(objectInHierarchy as Behaviour).enabled;
          break;
        case Renderer _:
          this.objectUsage.isDisabled = !(objectInHierarchy as Renderer).enabled;
          break;
        case MeshFilter _:
          Renderer component = (objectInHierarchy as MeshFilter).GetComponent<Renderer>();
          this.objectUsage.isDisabled = (UnityEngine.Object) component == (UnityEngine.Object) null || !component.enabled;
          break;
      }
    }

    internal System.Type AssetType
    {
      get
      {
        if (string.IsNullOrEmpty(this.assetTypeName))
          return (System.Type) null;
        System.Type type = System.Type.GetType("UnityEngine." + this.assetTypeName + ", UnityEngine");
        if (type == null)
        {
          type = System.Type.GetType(this.assetTypeName);
          if (type == null)
            this.LogError<BundleLoadRendererAssets.AssetUsage>("Type specified \"{0}\" could not be found in the UnityEngine assembly.", (object) this.assetTypeName);
        }
        return type;
      }
    }

    internal void Apply(BundleRendererAssets bundleRendererAssets)
    {
      System.Type assetType = this.AssetType;
      UnityEngine.Object asset1 = (UnityEngine.Object) null;
      foreach (UnityEngine.Object asset2 in bundleRendererAssets.assets)
      {
        if (assetType.IsInstanceOfType((object) asset2) && !(asset2.name != this.assetName))
        {
          asset1 = asset2;
          break;
        }
      }
      if (asset1 == (UnityEngine.Object) null)
        this.LogError<BundleLoadRendererAssets.AssetUsage>("Could not find asset of type \"{0}\" and name \"{1}\".", (object) this.AssetType, (object) this.assetName);
      else
        this.Apply(this.objectUsage, asset1);
    }

    [DebuggerHidden]
    private IEnumerable<Material> NewMaterialList(
      Material[] oldMaterials,
      Material newMaterial,
      int newMaterialIndex)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BundleLoadRendererAssets.AssetUsage.\u003CNewMaterialList\u003Ec__Iterator4F materialListCIterator4F = new BundleLoadRendererAssets.AssetUsage.\u003CNewMaterialList\u003Ec__Iterator4F()
      {
        oldMaterials = oldMaterials,
        newMaterialIndex = newMaterialIndex,
        newMaterial = newMaterial,
        \u003C\u0024\u003EoldMaterials = oldMaterials,
        \u003C\u0024\u003EnewMaterialIndex = newMaterialIndex,
        \u003C\u0024\u003EnewMaterial = newMaterial
      };
      // ISSUE: reference to a compiler-generated field
      materialListCIterator4F.\u0024PC = -2;
      return (IEnumerable<Material>) materialListCIterator4F;
    }

    private void Apply(
      BundleLoadRendererAssets.AssetUsage.ObjectUsage currentObjectUsage,
      UnityEngine.Object asset)
    {
      GameObject objectInHierarchy = currentObjectUsage.objectInHierarchy as GameObject;
      MeshFilter meshFilter = !((UnityEngine.Object) objectInHierarchy != (UnityEngine.Object) null) ? currentObjectUsage.objectInHierarchy as MeshFilter : objectInHierarchy.GetComponent<MeshFilter>();
      Mesh mesh = asset as Mesh;
      if ((UnityEngine.Object) meshFilter != (UnityEngine.Object) null && (UnityEngine.Object) mesh != (UnityEngine.Object) null)
      {
        meshFilter.sharedMesh = mesh;
        MeshRenderer component = meshFilter.GetComponent<MeshRenderer>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && !component.enabled && !currentObjectUsage.isDisabled)
          component.enabled = true;
      }
      Renderer renderer = !((UnityEngine.Object) objectInHierarchy != (UnityEngine.Object) null) ? currentObjectUsage.objectInHierarchy as Renderer : objectInHierarchy.GetComponent<Renderer>();
      Material newMaterial = asset as Material;
      if (!((UnityEngine.Object) newMaterial != (UnityEngine.Object) null) || !((UnityEngine.Object) renderer != (UnityEngine.Object) null))
        return;
      renderer.sharedMaterials = this.NewMaterialList(renderer.sharedMaterials, newMaterial, currentObjectUsage.index).ToArray<Material>();
      if (renderer.enabled || currentObjectUsage.isDisabled)
        return;
      renderer.enabled = true;
    }

    [Serializable]
    public class ObjectUsage
    {
      public int index;
      public bool isDisabled;
      public UnityEngine.Object objectInHierarchy;

      public ObjectUsage()
      {
      }

      public ObjectUsage(UnityEngine.Object objectInHierarchy, int index)
      {
        this.objectInHierarchy = objectInHierarchy;
        this.index = index;
        this.isDisabled = false;
      }
    }
  }
}
