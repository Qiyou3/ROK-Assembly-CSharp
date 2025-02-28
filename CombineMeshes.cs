// Decompiled with JetBrains decompiler
// Type: CombineMeshes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using SmartAssembly.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityThreading;

#nullable disable
[DoNotObfuscateControlFlow]
public class CombineMeshes
{
  public static int maxVertexPerMerge = 60000;

  [DoNotObfuscateControlFlow]
  private static void CombineDualMeshes(Mesh[,] dualMeshes, Mesh[] finalMeshes, int materialIndex)
  {
    int[] triangles = dualMeshes[1, materialIndex].triangles;
    for (int index = 0; index < triangles.Length; index += 3)
    {
      int num = triangles[index];
      triangles[index] = triangles[index + 1];
      triangles[index + 1] = num;
    }
    dualMeshes[1, materialIndex].triangles = triangles;
    CombineInstance[] combine = new CombineInstance[2];
    combine[0].mesh = dualMeshes[0, materialIndex];
    combine[0].subMeshIndex = 0;
    combine[1].mesh = dualMeshes[1, materialIndex];
    combine[1].subMeshIndex = 0;
    finalMeshes[materialIndex] = new Mesh();
    finalMeshes[materialIndex].CombineMeshes(combine, true, false);
  }

  [DoNotObfuscateControlFlow]
  private static Mesh CombineDualMeshes(Mesh meshNonInverted, Mesh meshInverted)
  {
    int[] triangles = meshInverted.triangles;
    for (int index = 0; index < triangles.Length; index += 3)
    {
      int num = triangles[index];
      triangles[index] = triangles[index + 1];
      triangles[index + 1] = num;
    }
    meshInverted.triangles = triangles;
    CombineInstance[] combine = new CombineInstance[2];
    combine[0].mesh = meshNonInverted;
    combine[0].subMeshIndex = 0;
    combine[1].mesh = meshInverted;
    combine[1].subMeshIndex = 0;
    Mesh mesh = new Mesh();
    mesh.CombineMeshes(combine, true, false);
    return mesh;
  }

  [DoNotObfuscateControlFlow]
  private static void CombineSeparate(
    GameObject combinedObjectsParent,
    List<Material> materials,
    List<List<CombineInstance>>[] combineInstanceLists)
  {
    for (int index1 = 0; index1 < materials.Count; ++index1)
    {
      GameObject gameObject = new GameObject(materials[index1].name);
      gameObject.transform.parent = combinedObjectsParent.transform;
      gameObject.transform.localPosition = Vector3.zero;
      gameObject.transform.localRotation = Quaternion.identity;
      gameObject.transform.localScale = Vector3.one;
      MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
      Mesh[] meshArray = new Mesh[2];
      for (int index2 = 0; index2 < 2; ++index2)
      {
        CombineInstance[] array = combineInstanceLists[index2][index1].ToArray();
        meshArray[index2] = new Mesh();
        meshArray[index2].CombineMeshes(array, true, true);
      }
      Mesh mesh1 = CombineMeshes.CombineDualMeshes(meshArray[0], meshArray[1]);
      CombineInstance[] combine = new CombineInstance[1]
      {
        new CombineInstance()
      };
      combine[0].mesh = mesh1;
      combine[0].subMeshIndex = 0;
      meshFilter.sharedMesh = new Mesh();
      meshFilter.sharedMesh.CombineMeshes(combine, false, false);
      mesh1.Clear();
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) mesh1);
      foreach (Mesh mesh2 in meshArray)
      {
        if ((bool) (UnityEngine.Object) mesh2)
        {
          mesh2.Clear();
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) mesh2);
        }
      }
      gameObject.AddComponent<MeshRenderer>().sharedMaterial = materials[index1];
    }
  }

  [DoNotObfuscateControlFlow]
  private static void CombineIntoOne(
    GameObject combinedObjectsParent,
    List<Material> materials,
    List<List<CombineInstance>>[] combineInstanceLists)
  {
    MeshFilter meshFilter = combinedObjectsParent.GetComponent<MeshFilter>();
    if (!(bool) (UnityEngine.Object) meshFilter)
      meshFilter = combinedObjectsParent.AddComponent<MeshFilter>();
    Mesh[,] meshArray1 = new Mesh[2, materials.Count];
    Mesh[] meshArray2 = new Mesh[materials.Count];
    CombineInstance[] combine = new CombineInstance[materials.Count];
    for (int index1 = 0; index1 < materials.Count; ++index1)
    {
      for (int index2 = 0; index2 < 2; ++index2)
      {
        CombineInstance[] array = combineInstanceLists[index2][index1].ToArray();
        meshArray1[index2, index1] = new Mesh();
        meshArray1[index2, index1].CombineMeshes(array, true, true);
      }
      meshArray2[index1] = CombineMeshes.CombineDualMeshes(meshArray1[0, index1], meshArray1[1, index1]);
      combine[index1] = new CombineInstance();
      combine[index1].mesh = meshArray2[index1];
      combine[index1].subMeshIndex = 0;
    }
    meshFilter.sharedMesh = new Mesh();
    meshFilter.sharedMesh.CombineMeshes(combine, false, false);
    foreach (Mesh mesh in meshArray2)
    {
      if ((bool) (UnityEngine.Object) mesh)
      {
        mesh.Clear();
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) mesh);
      }
    }
    Mesh[,] meshArray3 = meshArray1;
    int length1 = meshArray3.GetLength(0);
    int length2 = meshArray3.GetLength(1);
    for (int index3 = 0; index3 < length1; ++index3)
    {
      for (int index4 = 0; index4 < length2; ++index4)
      {
        Mesh mesh = meshArray3[index3, index4];
        if ((bool) (UnityEngine.Object) mesh)
        {
          mesh.Clear();
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) mesh);
        }
      }
    }
    MeshRenderer meshRenderer = combinedObjectsParent.GetComponent<MeshRenderer>();
    if (!(bool) (UnityEngine.Object) meshRenderer)
      meshRenderer = combinedObjectsParent.AddComponent<MeshRenderer>();
    Material[] array1 = materials.ToArray();
    meshRenderer.sharedMaterials = array1;
  }

  [DoNotObfuscateControlFlow]
  private static void ExtractMaterialSubmesh(
    MeshFilter meshFilter,
    MeshRenderer meshRenderer,
    List<Material> materials,
    int subMeshIndex,
    List<List<CombineInstance>>[] combineInstanceLists)
  {
    int index = 0;
    while (index < materials.Count && !((UnityEngine.Object) materials[index] == (UnityEngine.Object) meshRenderer.sharedMaterials[subMeshIndex]))
      ++index;
    if (index == materials.Count)
    {
      materials.Add(meshRenderer.sharedMaterials[subMeshIndex]);
      combineInstanceLists[0].Add(new List<CombineInstance>());
      combineInstanceLists[1].Add(new List<CombineInstance>());
    }
    CombineInstance combineInstance = new CombineInstance();
    combineInstance.transform = meshRenderer.transform.localToWorldMatrix;
    combineInstance.subMeshIndex = subMeshIndex;
    combineInstance.mesh = meshFilter.sharedMesh;
    bool flag = TransformUtilEx.IsInverted(meshRenderer.transform);
    combineInstanceLists[!flag ? 0 : 1][index].Add(combineInstance);
  }

  [DoNotObfuscateControlFlow]
  public static void Combine(
    GameObject[] objectsToCombine,
    GameObject combinedObjectsParent,
    bool separateMaterials)
  {
    List<Material> materials = new List<Material>();
    List<List<CombineInstance>>[] combineInstanceLists = new List<List<CombineInstance>>[2]
    {
      new List<List<CombineInstance>>(),
      new List<List<CombineInstance>>()
    };
    foreach (GameObject gameObject in objectsToCombine)
    {
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        Logger.Warning("Combine() recieved a null object - skipping...");
      else if (gameObject.isStatic)
      {
        foreach (MeshFilter componentsInChild in gameObject.GetComponentsInChildren<MeshFilter>())
        {
          if ((UnityEngine.Object) componentsInChild == (UnityEngine.Object) null)
            Logger.Error("Combine() recieved a null meshFilter - skipping...");
          else if (componentsInChild.gameObject.isStatic)
          {
            MeshRenderer component = componentsInChild.GetComponent<MeshRenderer>();
            if ((UnityEngine.Object) component == (UnityEngine.Object) null)
              Logger.Error("MeshFilter does not have a coresponding MeshRenderer.");
            else if (component.sharedMaterials.Length != componentsInChild.sharedMesh.subMeshCount)
            {
              Logger.Error("Mismatch between material count and submesh count. Is this the correct MeshRenderer?");
            }
            else
            {
              for (int subMeshIndex = 0; subMeshIndex < componentsInChild.sharedMesh.subMeshCount; ++subMeshIndex)
                CombineMeshes.ExtractMaterialSubmesh(componentsInChild, component, materials, subMeshIndex, combineInstanceLists);
            }
          }
        }
      }
    }
    if (separateMaterials)
      CombineMeshes.CombineSeparate(combinedObjectsParent, materials, combineInstanceLists);
    else
      CombineMeshes.CombineIntoOne(combinedObjectsParent, materials, combineInstanceLists);
  }

  [DoNotObfuscateControlFlow]
  private static void GetDistanceOfNode(
    CombineMeshes.LODLayerNodeDistComparer distComparer,
    List<CombineMeshes.LODLayerNode> lodLayersPosSorted,
    List<CombineMeshes.LODLayerNode> lodLayersDistSorted,
    int layerPosIndex)
  {
    CombineMeshes.LODLayerNode lodLayerNode1 = lodLayersPosSorted[layerPosIndex];
    CombineMeshes.LODLayerNode lodLayerNode2 = (CombineMeshes.LODLayerNode) null;
    for (int index = layerPosIndex + 1; index < lodLayersPosSorted.Count; ++index)
    {
      CombineMeshes.LODLayerNode otherNode = lodLayersPosSorted[index];
      if (lodLayerNode1.IsMergableWith(otherNode))
      {
        lodLayerNode2 = otherNode;
        break;
      }
    }
    lodLayerNode1.distanceToNext = lodLayerNode2 != null ? ((double) lodLayerNode2.pixelSize != (double) lodLayerNode1.pixelSize ? lodLayerNode2.pixelSize - lodLayerNode1.pixelSize : 0.0f) : float.PositiveInfinity;
    lodLayersDistSorted.InsertSorted<CombineMeshes.LODLayerNode>(lodLayerNode1, (IComparer<CombineMeshes.LODLayerNode>) distComparer);
  }

  [DoNotObfuscateControlFlow]
  private static List<LODGroupScriptable> GetLODGroups(
    GameObject[] objectsToCombine,
    ref int numNullObjects)
  {
    List<LODGroupScriptable> lodGroups = new List<LODGroupScriptable>();
    for (int index = 0; index < objectsToCombine.Length; ++index)
    {
      if ((UnityEngine.Object) objectsToCombine[index] == (UnityEngine.Object) null)
        ++numNullObjects;
      else
        lodGroups.AddRange((IEnumerable<LODGroupScriptable>) objectsToCombine[index].GetComponentsInChildren<LODGroupScriptable>());
    }
    return lodGroups;
  }

  [DoNotObfuscateControlFlow]
  private static Bounds[] GetMaterialBounds(
    List<LODGroupScriptable> lodGroups,
    List<Material> materials)
  {
    Bounds[] materialBounds = new Bounds[materials.Count];
    for (int index1 = 0; index1 < lodGroups.Count; ++index1)
    {
      for (int index2 = 0; index2 < lodGroups[index1].layers.Count; ++index2)
      {
        LODGroupScriptable.LODLayer layer = lodGroups[index1].layers[index2];
        if (layer.renderers != null)
        {
          for (int index3 = 0; index3 < layer.renderers.Count; ++index3)
          {
            Renderer renderer = layer.renderers[index3];
            if (!((UnityEngine.Object) renderer == (UnityEngine.Object) null))
            {
              Material[] sharedMaterials = renderer.sharedMaterials;
              for (int index4 = 0; index4 < materialBounds.Length; ++index4)
              {
                if (sharedMaterials.Contains<Material>(materials[index4]))
                {
                  if (materialBounds[index4].size == Vector3.zero)
                    materialBounds[index4] = renderer.bounds;
                  else
                    materialBounds[index4].Encapsulate(renderer.bounds);
                }
              }
            }
          }
        }
      }
    }
    return materialBounds;
  }

  [DoNotObfuscateControlFlow]
  private static void GetNodesMaterialsMasks(
    List<LODGroupScriptable> lodGroups,
    List<CombineMeshes.LODLayerNode> lodNodesPosSorted,
    List<Material> materials,
    ref Mask[] lodGroupMaterialMasks,
    ref int numNullLayers,
    ref int numNullRenderers,
    ref string nullRendererMeshfilters,
    ref string mismatchedRenderers)
  {
    for (int index1 = 0; index1 < lodGroups.Count; ++index1)
    {
      LODGroupScriptable lodGroup = lodGroups[index1];
      lodGroupMaterialMasks[index1] = new Mask();
      Mask mask = lodGroupMaterialMasks[index1];
      for (int index2 = 0; index2 < lodGroup.layers.Count; ++index2)
      {
        LODGroupScriptable.LODLayer layer = lodGroup.layers[index2];
        if (layer == null)
          ++numNullLayers;
        else if (layer.enabled)
        {
          CombineMeshes.LODLayerNode lodLayerNode = new CombineMeshes.LODLayerNode(layer, index1);
          lodNodesPosSorted.InsertSorted<CombineMeshes.LODLayerNode>(lodLayerNode, (IComparer<CombineMeshes.LODLayerNode>) CombineMeshes.LODLayerNodePosComparer.comparer);
          for (int index3 = 0; index3 < layer.renderers.Count; ++index3)
          {
            Renderer renderer = layer.renderers[index3];
            if ((UnityEngine.Object) renderer == (UnityEngine.Object) null)
              ++numNullRenderers;
            else if (renderer.sharedMaterials == null)
              ++numNullRenderers;
            else if (renderer.sharedMaterials.Length == 0)
            {
              ++numNullRenderers;
            }
            else
            {
              MeshFilter component = renderer.GetComponent<MeshFilter>();
              if ((UnityEngine.Object) component == (UnityEngine.Object) null)
                nullRendererMeshfilters = nullRendererMeshfilters + renderer.name + " type of " + renderer.ToString() + " has no mesh filter.\n";
              else if ((UnityEngine.Object) component.sharedMesh == (UnityEngine.Object) null)
              {
                nullRendererMeshfilters = nullRendererMeshfilters + renderer.name + " type of " + renderer.ToString() + " has no sharedMesh.\n";
              }
              else
              {
                if (component.sharedMesh.vertexCount == 0)
                  nullRendererMeshfilters = nullRendererMeshfilters + renderer.name + " type of " + renderer.ToString() + " has no vertices.\n";
                if (component.sharedMesh.subMeshCount != renderer.sharedMaterials.Length)
                  mismatchedRenderers = mismatchedRenderers + renderer.name + " (type of " + renderer.ToString() + ") has a different number of materials\nthan what is demanded by " + component.name + " (type of " + renderer.ToString() + ").\n";
                int num1 = Mathf.Min(component.sharedMesh.subMeshCount, renderer.sharedMaterials.Length);
                for (int index4 = 0; index4 < num1; ++index4)
                {
                  Material sharedMaterial = renderer.sharedMaterials[index4];
                  int num2 = 0;
                  while (num2 < materials.Count && !((UnityEngine.Object) materials[num2] == (UnityEngine.Object) sharedMaterial))
                    ++num2;
                  if (num2 == materials.Count)
                    materials.Add(sharedMaterial);
                  mask.Set((short) num2, true);
                  if (!lodLayerNode.combineInstances.Exists<List<CombineMeshes.LODCombineInstance>>(num2))
                    lodLayerNode.combineInstances.ExpandSetAt<List<CombineMeshes.LODCombineInstance>>(num2, new List<CombineMeshes.LODCombineInstance>());
                  lodLayerNode.combineInstances[num2].Add(new CombineMeshes.LODCombineInstance()
                  {
                    combineInstance = new CombineInstance()
                    {
                      mesh = component.sharedMesh,
                      subMeshIndex = index4,
                      transform = renderer.transform.localToWorldMatrix
                    },
                    pixelSize = layer.pixelsForWorldUnit
                  });
                }
              }
            }
          }
        }
      }
    }
  }

  [DoNotObfuscateControlFlow]
  private static List<CombineMeshes.LODLayerNode> GetNodeListDistSorted(
    List<CombineMeshes.LODLayerNode> lodNodesPosSorted)
  {
    List<CombineMeshes.LODLayerNode> lodLayersDistSorted = new List<CombineMeshes.LODLayerNode>(lodNodesPosSorted.Count);
    for (int layerPosIndex = 0; layerPosIndex < lodNodesPosSorted.Count; ++layerPosIndex)
      CombineMeshes.GetDistanceOfNode(CombineMeshes.LODLayerNodeDistComparer.comparer, lodNodesPosSorted, lodLayersDistSorted, layerPosIndex);
    return lodLayersDistSorted;
  }

  [DoNotObfuscateControlFlow]
  private static void SetMaterialMasks(
    List<CombineMeshes.LODLayerNode> lodNodesPosSorted,
    ref Mask[] lodGroupMaterialMasks)
  {
    for (int index = 0; index < lodNodesPosSorted.Count; ++index)
      lodNodesPosSorted[index].materialMask.Add(lodGroupMaterialMasks[lodNodesPosSorted[index].lodGroupIndex]);
  }

  [DoNotObfuscateControlFlow]
  private static void IterativeMerge(
    List<CombineMeshes.LODLayerNode> lodNodesPosSorted,
    List<CombineMeshes.LODLayerNode> lodNodesDistSorted,
    ref string mergeErrors)
  {
    int index1 = 0;
    while (index1 < lodNodesDistSorted.Count)
    {
      CombineMeshes.LODLayerNode lodLayerNode = lodNodesDistSorted[index1];
      if (lodLayerNode == null)
      {
        mergeErrors += "An element in lodLayersDistSorted was null - this is unexpected behavior.";
        ++index1;
      }
      else
      {
        int layerPosIndex = lodNodesPosSorted.IndexOf(lodLayerNode);
        if (layerPosIndex < 0)
        {
          mergeErrors = mergeErrors + "An element in lodLayersDistSorted " + (object) index1 + " is not contained in lodLayersPosSorted.\n";
          ++index1;
        }
        else
        {
          int index2 = layerPosIndex + 1;
          while (index2 < lodNodesPosSorted.Count && !lodLayerNode.IsMergableWith(lodNodesPosSorted[index2]))
            ++index2;
          if (index2 >= lodNodesPosSorted.Count)
          {
            ++index1;
          }
          else
          {
            CombineMeshes.LODLayerNode otherNode = lodNodesPosSorted[index2];
            lodLayerNode.MergeWith(otherNode);
            lodNodesPosSorted.RemoveAt(index2);
            lodNodesDistSorted.Remove(lodLayerNode);
            lodNodesDistSorted.Remove(otherNode);
            CombineMeshes.GetDistanceOfNode(CombineMeshes.LODLayerNodeDistComparer.comparer, lodNodesPosSorted, lodNodesDistSorted, layerPosIndex);
            if (layerPosIndex >= 1)
            {
              lodNodesDistSorted.Remove(lodNodesPosSorted[layerPosIndex - 1]);
              CombineMeshes.GetDistanceOfNode(CombineMeshes.LODLayerNodeDistComparer.comparer, lodNodesPosSorted, lodNodesDistSorted, layerPosIndex - 1);
            }
          }
        }
      }
    }
  }

  [DoNotObfuscateControlFlow]
  private static void GenerateLodGeometry(
    GameObject combinedObjectsParent,
    List<CombineMeshes.LODLayerNode> lodNodesPosSorted,
    List<Material> materials,
    ref int numNullLayers)
  {
    LODGroupScriptable[] lodGroupScriptableArray = new LODGroupScriptable[materials.Count];
    for (int index = 0; index < lodGroupScriptableArray.Length; ++index)
    {
      lodGroupScriptableArray[index] = new GameObject(materials[index].name).AddComponent<LODGroupScriptable>();
      lodGroupScriptableArray[index].transform.parent = combinedObjectsParent.transform;
      lodGroupScriptableArray[index].layers = new List<LODGroupScriptable.LODLayer>();
    }
    List<CombineInstance> combineInstances = new List<CombineInstance>();
    for (int index1 = 0; index1 < lodNodesPosSorted.Count; ++index1)
    {
      CombineMeshes.LODLayerNode lodLayerNode = lodNodesPosSorted[index1];
      if (lodLayerNode == null)
      {
        ++numNullLayers;
      }
      else
      {
        for (int index2 = 0; index2 < materials.Count; ++index2)
        {
          LODGroupScriptable lodGroupScriptable = lodGroupScriptableArray[index2];
          Material material = materials[index2];
          if (lodLayerNode.materialMask.Get((short) index2))
          {
            LODGroupScriptable.LODLayer lodLayer = new LODGroupScriptable.LODLayer();
            lodLayer.pixelsForWorldUnit = lodLayerNode.pixelSize;
            lodLayer.renderers = new List<Renderer>();
            List<CombineMeshes.LODCombineInstance> lodCombineInstanceList = (List<CombineMeshes.LODCombineInstance>) null;
            if (lodLayerNode.combineInstances.Exists<List<CombineMeshes.LODCombineInstance>>(index2))
              lodCombineInstanceList = lodLayerNode.combineInstances[index2];
            bool flag = true;
            if (lodCombineInstanceList == null)
              flag = false;
            else if (lodCombineInstanceList.Count == 0)
              flag = false;
            if (flag)
            {
              int index3 = 0;
              int num1 = 0;
              while (index3 < lodCombineInstanceList.Count)
              {
                MeshRenderer meshRenderer = new GameObject(material.name + " Renderer (Division " + (object) num1 + ")").AddComponent<MeshRenderer>();
                meshRenderer.useLightProbes = true;
                MeshFilter meshFilter = meshRenderer.gameObject.AddComponent<MeshFilter>();
                meshRenderer.transform.parent = lodGroupScriptable.transform;
                combineInstances.Clear();
                int num2 = 0;
                int num3 = 0;
                for (; index3 < lodCombineInstanceList.Count; ++index3)
                {
                  num2 += lodCombineInstanceList[index3].combineInstance.mesh.vertexCount;
                  if (num2 <= CombineMeshes.maxVertexPerMerge || num3 == 0)
                  {
                    ++num3;
                    combineInstances.Add(lodCombineInstanceList[index3].combineInstance);
                  }
                  else
                    break;
                }
                meshFilter.mesh = MeshUtil.CombineFull(combineInstances, true);
                meshRenderer.sharedMaterial = material;
                lodLayer.renderers.Add((Renderer) meshRenderer);
                ++num1;
              }
            }
            lodGroupScriptableArray[index2].layers.Add(lodLayer);
          }
        }
      }
    }
    for (int index = 0; index < lodGroupScriptableArray.Length; ++index)
      lodGroupScriptableArray[index].Start();
  }

  [DoNotObfuscateControlFlow]
  internal static void CombineScriptableLOD(
    GameObject[] objectsToCombine,
    GameObject combinedObjectsParent,
    bool mergeGeometry)
  {
    int numNullObjects = 0;
    int numNullLayers = 0;
    int numNullRenderers = 0;
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string empty3 = string.Empty;
    try
    {
      List<LODGroupScriptable> lodGroups = CombineMeshes.GetLODGroups(objectsToCombine, ref numNullObjects);
      List<CombineMeshes.LODLayerNode> lodNodesPosSorted = new List<CombineMeshes.LODLayerNode>();
      List<Material> materials = new List<Material>();
      Mask[] lodGroupMaterialMasks = new Mask[lodGroups.Count];
      CombineMeshes.GetNodesMaterialsMasks(lodGroups, lodNodesPosSorted, materials, ref lodGroupMaterialMasks, ref numNullLayers, ref numNullRenderers, ref empty1, ref empty2);
      CombineMeshes.SetMaterialMasks(lodNodesPosSorted, ref lodGroupMaterialMasks);
      List<CombineMeshes.LODLayerNode> nodeListDistSorted = CombineMeshes.GetNodeListDistSorted(lodNodesPosSorted);
      CombineMeshes.IterativeMerge(lodNodesPosSorted, nodeListDistSorted, ref empty3);
      CombineMeshes.GenerateLodGeometry(combinedObjectsParent, lodNodesPosSorted, materials, ref numNullLayers);
      for (int index = 0; index < lodGroups.Count; ++index)
      {
        if ((UnityEngine.Object) lodGroups[index] != (UnityEngine.Object) null)
          lodGroups[index].enabled = false;
      }
    }
    catch (Exception ex)
    {
      throw ex;
    }
  }

  [DoNotObfuscateControlFlow]
  private static List<CombineMeshes.LODGroupCombineInstanceList> GenerateCombineInstanceLists(
    GameObject combinedObjectsParent,
    List<CombineMeshes.LODLayerNode> lodNodesPosSorted,
    List<Material> materials,
    Bounds[] bounds,
    ref int numNullLayers)
  {
    List<CombineMeshes.LODGroupCombineInstanceList> combineInstanceLists = new List<CombineMeshes.LODGroupCombineInstanceList>(materials.Count);
    for (int index = 0; index < materials.Count; ++index)
    {
      combineInstanceLists.Add(new CombineMeshes.LODGroupCombineInstanceList(combinedObjectsParent));
      combineInstanceLists[index].material = materials[index];
      combineInstanceLists[index].bounds = bounds[index];
    }
    for (int index1 = 0; index1 < lodNodesPosSorted.Count; ++index1)
    {
      CombineMeshes.LODLayerNode lodLayerNode = lodNodesPosSorted[index1];
      if (lodLayerNode == null)
      {
        ++numNullLayers;
      }
      else
      {
        for (int index2 = 0; index2 < materials.Count; ++index2)
        {
          if (lodLayerNode.materialMask.Get((short) index2))
          {
            CombineMeshes.LODGroupCombineInstanceList.LODLayer lodLayer = new CombineMeshes.LODGroupCombineInstanceList.LODLayer();
            lodLayer.pixelsForWorldUnit = lodLayerNode.pixelSize;
            List<CombineMeshes.LODCombineInstance> lodCombineInstanceList = (List<CombineMeshes.LODCombineInstance>) null;
            if (lodLayerNode.combineInstances.Exists<List<CombineMeshes.LODCombineInstance>>(index2))
              lodCombineInstanceList = lodLayerNode.combineInstances[index2];
            bool flag = true;
            if (lodCombineInstanceList == null)
              flag = false;
            else if (lodCombineInstanceList.Count == 0)
              flag = false;
            if (flag)
            {
              int index3 = 0;
              while (index3 < lodCombineInstanceList.Count)
              {
                for (; index3 < lodCombineInstanceList.Count; ++index3)
                  lodLayer.combineInstances.Add(lodCombineInstanceList[index3].combineInstance);
              }
            }
            combineInstanceLists[index2].lodLayers.Add(lodLayer);
          }
        }
      }
    }
    return combineInstanceLists;
  }

  [DoNotObfuscateControlFlow]
  internal static List<CombineMeshes.LODGroupCombineInstanceList> CombineScriptableLOD(
    List<LODGroupScriptable> lodGroups,
    GameObject combinedObjectsParent)
  {
    int numNullLayers = 0;
    int numNullRenderers = 0;
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string empty3 = string.Empty;
    try
    {
      lodGroups = lodGroups.Where<LODGroupScriptable>((Func<LODGroupScriptable, bool>) (lg => (UnityEngine.Object) lg != (UnityEngine.Object) null)).Distinct<LODGroupScriptable>().ToList<LODGroupScriptable>();
      List<CombineMeshes.LODLayerNode> lodNodesPosSorted = new List<CombineMeshes.LODLayerNode>();
      List<Material> materials = new List<Material>();
      Mask[] lodGroupMaterialMasks = new Mask[lodGroups.Count];
      CombineMeshes.GetNodesMaterialsMasks(lodGroups, lodNodesPosSorted, materials, ref lodGroupMaterialMasks, ref numNullLayers, ref numNullRenderers, ref empty1, ref empty2);
      Bounds[] materialBounds = CombineMeshes.GetMaterialBounds(lodGroups, materials);
      CombineMeshes.SetMaterialMasks(lodNodesPosSorted, ref lodGroupMaterialMasks);
      List<CombineMeshes.LODLayerNode> nodeListDistSorted = CombineMeshes.GetNodeListDistSorted(lodNodesPosSorted);
      CombineMeshes.IterativeMerge(lodNodesPosSorted, nodeListDistSorted, ref empty3);
      return CombineMeshes.GenerateCombineInstanceLists(combinedObjectsParent, lodNodesPosSorted, materials, materialBounds, ref numNullLayers);
    }
    catch (Exception ex)
    {
      throw ex;
    }
  }

  [DoNotObfuscateControlFlow]
  private static void ReportErrors(
    int numNullObjects,
    int numNullLayers,
    int numNullRenderers,
    string nullRendererMeshfilters,
    string mismatchedRenderers,
    string mergeErrors)
  {
    if (numNullObjects > 0)
      Logger.WarningFormat("numNullObjects = {0}", (object) numNullObjects);
    if (numNullLayers > 0)
      Logger.WarningFormat("numNullLayers = {0}", (object) numNullLayers);
    if (numNullRenderers > 0)
      Logger.WarningFormat("numNullRenderers = {0}", (object) numNullRenderers);
    if (nullRendererMeshfilters != string.Empty)
      Logger.Error(nullRendererMeshfilters);
    if (mismatchedRenderers != string.Empty)
      Logger.Warning(mismatchedRenderers);
    if (!(mergeErrors != string.Empty))
      return;
    Logger.Error(mergeErrors);
  }

  public struct LODCombineInstance
  {
    public CombineInstance combineInstance;
    public float pixelSize;
  }

  [DoNotObfuscateControlFlow]
  private class LODLayerNode
  {
    public List<List<CombineMeshes.LODCombineInstance>> combineInstances;
    public float pixelSize;
    public float distanceToNext;
    public Mask groupIndexMask;
    public int lodGroupIndex;
    public Mask materialMask;

    [DoNotObfuscateControlFlow]
    public LODLayerNode(LODGroupScriptable.LODLayer layer, int _lodGroupIndex)
    {
      this.combineInstances = new List<List<CombineMeshes.LODCombineInstance>>();
      this.pixelSize = layer.pixelsForWorldUnit;
      this.distanceToNext = 0.0f;
      this.lodGroupIndex = _lodGroupIndex;
      this.groupIndexMask = new Mask();
      this.groupIndexMask.Set((short) _lodGroupIndex, true);
      this.materialMask = new Mask();
    }

    [DoNotObfuscateControlFlow]
    public bool IsMergableWith(CombineMeshes.LODLayerNode otherNode)
    {
      return otherNode != null && !this.groupIndexMask.HasAnyOf(otherNode.groupIndexMask) && this.materialMask.HasAnyOf(otherNode.materialMask);
    }

    [DoNotObfuscateControlFlow]
    public void MergeWith(CombineMeshes.LODLayerNode otherNode)
    {
      if (otherNode.combineInstances == null)
        this.LogError<CombineMeshes.LODLayerNode>("otherNode.combineInstances is null - this is unexpected behavior.");
      else if (otherNode.combineInstances.Count != 0)
      {
        int count = otherNode.combineInstances.Count;
        if (this.combineInstances.Count < count)
          this.combineInstances.ExpandSetAt<List<CombineMeshes.LODCombineInstance>>(count - 1, (List<CombineMeshes.LODCombineInstance>) null);
        for (int index = 0; index < count; ++index)
        {
          if (otherNode.combineInstances[index] != null && otherNode.combineInstances[index].Count != 0)
          {
            if (this.combineInstances[index] == null)
              this.combineInstances[index] = new List<CombineMeshes.LODCombineInstance>();
            this.combineInstances[index].AddRange((IEnumerable<CombineMeshes.LODCombineInstance>) otherNode.combineInstances[index]);
          }
        }
      }
      this.pixelSize = (float) (((double) this.pixelSize + (double) otherNode.pixelSize) / 2.0);
      this.groupIndexMask.Add(otherNode.groupIndexMask);
      this.materialMask.Add(otherNode.materialMask);
    }
  }

  [DoNotObfuscateControlFlow]
  private class LODLayerNodePosComparer : IComparer<CombineMeshes.LODLayerNode>
  {
    public static CombineMeshes.LODLayerNodePosComparer comparer = new CombineMeshes.LODLayerNodePosComparer();

    [DoNotObfuscateControlFlow]
    public int Compare(CombineMeshes.LODLayerNode x, CombineMeshes.LODLayerNode y)
    {
      return x.pixelSize.CompareTo(y.pixelSize);
    }
  }

  [DoNotObfuscateControlFlow]
  private class LODLayerNodeDistComparer : IComparer<CombineMeshes.LODLayerNode>
  {
    public static CombineMeshes.LODLayerNodeDistComparer comparer = new CombineMeshes.LODLayerNodeDistComparer();

    [DoNotObfuscateControlFlow]
    public int Compare(CombineMeshes.LODLayerNode x, CombineMeshes.LODLayerNode y)
    {
      return x.distanceToNext.CompareTo(y.distanceToNext);
    }
  }

  [DoNotObfuscateControlFlow]
  public class LODGroupCombineInstanceList
  {
    private static MeshUtilCoroutine _BatchInstance;
    public Bounds bounds;
    public Material material;
    public List<CombineMeshes.LODGroupCombineInstanceList.LODLayer> lodLayers = new List<CombineMeshes.LODGroupCombineInstanceList.LODLayer>();
    public int layerCurrentIndex = -1;
    public int layerBuildingIndex = -1;
    public int layerPendingIndex = -1;
    public GameObject combinedObjectsParent;
    public GameObject gameObject;
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public Mesh mesh;
    public bool isEmptyLODLayer;
    private ActionThread buildThread;
    private static float sqrt3 = Mathf.Sqrt(3f);

    [DoNotObfuscateControlFlow]
    public LODGroupCombineInstanceList(GameObject combinedObjectsParent)
    {
      this.combinedObjectsParent = combinedObjectsParent;
    }

    public static MeshUtilCoroutine BatchInstance
    {
      get
      {
        if ((UnityEngine.Object) CombineMeshes.LODGroupCombineInstanceList._BatchInstance == (UnityEngine.Object) null)
          CombineMeshes.LODGroupCombineInstanceList._BatchInstance = new GameObject("MeshUtilCoroutine").AddComponent<MeshUtilCoroutine>();
        return CombineMeshes.LODGroupCombineInstanceList._BatchInstance;
      }
    }

    [DoNotObfuscateControlFlow]
    ~LODGroupCombineInstanceList()
    {
    }

    [DoNotObfuscateControlFlow]
    public void Refresh()
    {
      this.layerPendingIndex = this.GetDesiredLODIndex();
      if (this.layerPendingIndex == this.layerCurrentIndex)
      {
        this.layerPendingIndex = -1;
        if (!this.IsBuilding())
          return;
        this.AbortBuild();
      }
      else if (this.layerPendingIndex == this.layerBuildingIndex)
      {
        this.layerPendingIndex = -1;
      }
      else
      {
        if (this.layerPendingIndex == this.layerBuildingIndex || this.layerBuildingIndex != -1)
          return;
        this.layerBuildingIndex = this.layerPendingIndex;
        this.layerPendingIndex = -1;
        this.StartBuild();
      }
    }

    [DoNotObfuscateControlFlow]
    private int GetDesiredLODIndex()
    {
      float pixelsForWorldUnit = LODUtil.CalculatePixelsForWorldUnit(Camera.main, LODGroupScriptable.GetClosestRenderPoint(this.bounds, Camera.main.transform.position), QualitySettings.lodBias, true);
      int index = 0;
      while (index < this.lodLayers.Count && (double) pixelsForWorldUnit >= (double) this.lodLayers[index].pixelsForWorldUnit)
        ++index;
      if (index >= this.lodLayers.Count)
        index = this.lodLayers.Count - 1;
      if (index < 0)
        index = 0;
      if (3 - index < QualitySettings.maximumLODLevel)
        index = 3 - QualitySettings.maximumLODLevel;
      return index;
    }

    [DoNotObfuscateControlFlow]
    private bool IsBuilding() => this.layerBuildingIndex != -1;

    [DoNotObfuscateControlFlow]
    private void AbortBuild()
    {
      if ((bool) (UnityEngine.Object) CombineMeshes.LODGroupCombineInstanceList._BatchInstance)
        CombineMeshes.LODGroupCombineInstanceList.BatchInstance.StopAllCoroutines();
      this.layerBuildingIndex = -1;
      if (this.buildThread != null)
        this.buildThread.Exit();
      this.buildThread = (ActionThread) null;
    }

    [DoNotObfuscateControlFlow]
    private void StartBuild()
    {
      if (this.lodLayers[this.layerBuildingIndex].combineInstances.Count == 0)
      {
        this.SubmitEmpty();
      }
      else
      {
        CombineMeshes.LODGroupCombineInstanceList.BatchInstance.StopAllCoroutines();
        CombineMeshes.LODGroupCombineInstanceList.BatchInstance.StartThreadSafeCombineInstances(this.lodLayers[this.layerBuildingIndex].combineInstances, new MeshUtil.CombineCompleteHandler(this.StartThread));
      }
    }

    private void StartThread(
      MeshUtil.CombineInstanceThreadSafe[] combineInstancesThreadSafe)
    {
      this.buildThread = UnityThreadHelper.CreateThread((System.Action) (() => this.Build(combineInstancesThreadSafe)));
    }

    [DoNotObfuscateControlFlow]
    private void Build(
      MeshUtil.CombineInstanceThreadSafe[] combineInstancesThreadSafe)
    {
      MeshUtil.MeshDataThreadSafe meshDataThreadSafe = MeshUtil.CombineFull(combineInstancesThreadSafe, true);
      if (this.layerBuildingIndex == -1)
        return;
      UnityThreadHelper.Dispatcher.Dispatch((System.Action) (() => this.SubmitBuild(meshDataThreadSafe)));
    }

    [DoNotObfuscateControlFlow]
    private void SubmitBuild(MeshUtil.MeshDataThreadSafe meshDataThreadSafe)
    {
      if (meshDataThreadSafe == null)
      {
        this.LogError<CombineMeshes.LODGroupCombineInstanceList>("meshDataThreadSafe == null - This problem should have been caught and handled in StartBuild().");
        this.SubmitEmpty();
      }
      else
      {
        this.isEmptyLODLayer = false;
        if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
        {
          this.gameObject = new GameObject();
          this.gameObject.transform.parent = this.combinedObjectsParent.transform;
          this.meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
          this.meshFilter = this.gameObject.AddComponent<MeshFilter>();
          this.meshRenderer.sharedMaterial = this.material;
          this.meshRenderer.enabled = false;
        }
        if ((UnityEngine.Object) this.mesh == (UnityEngine.Object) null)
          this.mesh = new Mesh();
        this.layerCurrentIndex = this.layerBuildingIndex;
        this.layerBuildingIndex = -1;
        this.buildThread = (ActionThread) null;
        meshDataThreadSafe.ToMesh(this.mesh);
        this.mesh.name = "Combined LOD Mesh - LOD " + (object) this.layerCurrentIndex;
        this.meshFilter.sharedMesh = this.mesh;
      }
    }

    [DoNotObfuscateControlFlow]
    public void SubmitEmpty()
    {
      this.isEmptyLODLayer = true;
      this.layerCurrentIndex = this.layerBuildingIndex;
      this.layerBuildingIndex = -1;
      this.buildThread = (ActionThread) null;
      if ((UnityEngine.Object) this.mesh != (UnityEngine.Object) null)
      {
        this.mesh.Clear();
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.mesh);
      }
      if (!((UnityEngine.Object) this.meshFilter != (UnityEngine.Object) null))
        return;
      this.meshFilter.sharedMesh = (Mesh) null;
    }

    [DoNotObfuscateControlFlow]
    public void Destroy()
    {
      if ((UnityEngine.Object) this.mesh != (UnityEngine.Object) null)
      {
        this.mesh.Clear();
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.mesh);
      }
      if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.gameObject);
    }

    [DoNotObfuscateControlFlow]
    public bool IsReady()
    {
      return this.isEmptyLODLayer || !((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null) && !((UnityEngine.Object) this.mesh == (UnityEngine.Object) null) && !((UnityEngine.Object) this.meshRenderer == (UnityEngine.Object) null) && !((UnityEngine.Object) this.meshFilter == (UnityEngine.Object) null);
    }

    public string status
    {
      [DoNotObfuscateControlFlow] get
      {
        return "Material: " + (!((UnityEngine.Object) this.material != (UnityEngine.Object) null) ? (object) "(None)" : (object) this.material.name) + "\nLayers: " + (object) this.lodLayers.Count + "\nLayer ready for rendering: " + (this.layerCurrentIndex != -1 ? (object) this.layerCurrentIndex.ToString() : (object) "(None)") + "\nLayer being built: " + (this.layerBuildingIndex != -1 ? (object) this.layerBuildingIndex.ToString() : (object) "(None)") + "\nLayer waiting to be built: " + (this.layerPendingIndex != -1 ? (object) this.layerPendingIndex.ToString() : (object) "(None)") + "\nIs ready to render: " + (!this.IsReady() ? (object) "No" : (object) "Yes") + "\n        - Is Empty LOD Layer: " + (!this.isEmptyLODLayer ? (object) "No" : (object) "Yes") + "\n        - GameObject: " + (!(bool) (UnityEngine.Object) this.gameObject ? (object) "No" : (object) "Yes") + "\n        - Mesh: " + (!(bool) (UnityEngine.Object) this.mesh ? (object) "No" : (object) "Yes") + "\n        - MeshRenderer: " + (!(bool) (UnityEngine.Object) this.meshRenderer ? (object) "No" : (object) "Yes") + "\n        - MeshFilter: " + (!(bool) (UnityEngine.Object) this.meshFilter ? (object) "No" : (object) "Yes") + "\n        - Mesh: " + (!(bool) (UnityEngine.Object) this.mesh ? (object) "(None)" : (object) this.mesh.name) + "\n        - GameObject: " + (!(bool) (UnityEngine.Object) this.gameObject ? (object) "(None)" : (object) this.gameObject.name) + "\nThread active / should stop: " + (this.buildThread == null ? (object) "(N/A)" : (object) ((!this.buildThread.IsAlive ? "No" : "Yes") + "/" + (!this.buildThread.ShouldStop ? "No" : "Yes")));
      }
    }

    public class LODLayer
    {
      public float pixelsForWorldUnit;
      public List<CombineInstance> combineInstances = new List<CombineInstance>();
    }
  }
}
