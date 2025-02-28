// Decompiled with JetBrains decompiler
// Type: CombineChildren
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("Mesh/Combine Children")]
public class CombineChildren : MonoBehaviour
{
  public bool generateTriangleStrips;
  public bool addMeshCollider;
  public bool destroyOldMeshes;
  public bool divideUpIntoGrid;
  public Vector2 gridResolution = new Vector2(4f, 4f);
  private int staticID;

  public void Awake()
  {
    Component[] componentsInChildren = this.GetComponentsInChildren(typeof (MeshFilter));
    Matrix4x4 worldToLocalMatrix = this.transform.worldToLocalMatrix;
    Bounds bounds = new Bounds();
    for (int index = 0; index < componentsInChildren.Length; ++index)
    {
      Transform transform = componentsInChildren[index].transform;
      if (index == 0)
        bounds.center = transform.position;
      else
        bounds.Encapsulate(transform.position);
    }
    if (!this.divideUpIntoGrid)
    {
      this.HandleDictionary(CombineChildren.GetDictionary(bounds, componentsInChildren, worldToLocalMatrix));
    }
    else
    {
      bounds.SetMinMax(bounds.min, bounds.min + Vector3.Scale(bounds.size, new Vector3(1f / this.gridResolution.x, 1f, 1f / this.gridResolution.y)));
      for (int index1 = 0; (double) index1 < (double) this.gridResolution.x; ++index1)
      {
        for (int index2 = 0; (double) index2 < (double) this.gridResolution.y; ++index2)
        {
          this.HandleDictionary(CombineChildren.GetDictionary(bounds, componentsInChildren, worldToLocalMatrix));
          bounds.center += Vector3.forward * bounds.size.z;
        }
        bounds.center -= Vector3.forward * bounds.size.z * this.gridResolution.y;
        bounds.center += Vector3.right * bounds.size.x;
      }
      if (!this.destroyOldMeshes)
        return;
      for (int index = 0; index < componentsInChildren.Length; ++index)
        UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChildren[index].gameObject);
    }
  }

  private static Hashtable GetDictionary(Bounds bounds, Component[] filters, Matrix4x4 myTransform)
  {
    Hashtable dictionary = new Hashtable();
    for (int index = 0; index < filters.Length; ++index)
    {
      MeshFilter filter = (MeshFilter) filters[index];
      Transform transform = filter.transform;
      if (bounds.Contains(transform.position))
      {
        Renderer component = filters[index].GetComponent<Renderer>();
        MeshCombineUtility.MeshInstance meshInstance = new MeshCombineUtility.MeshInstance();
        meshInstance.mesh = filter.sharedMesh;
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.enabled && (UnityEngine.Object) meshInstance.mesh != (UnityEngine.Object) null)
        {
          meshInstance.transform = myTransform * filter.transform.localToWorldMatrix;
          Material[] sharedMaterials = component.sharedMaterials;
          for (int val1 = 0; val1 < sharedMaterials.Length; ++val1)
          {
            meshInstance.subMeshIndex = Math.Min(val1, meshInstance.mesh.subMeshCount - 1);
            ArrayList arrayList = (ArrayList) dictionary[(object) sharedMaterials[val1]];
            if (arrayList != null)
              arrayList.Add((object) meshInstance);
            else
              dictionary.Add((object) sharedMaterials[val1], (object) new ArrayList()
              {
                (object) meshInstance
              });
          }
          component.enabled = false;
        }
      }
    }
    return dictionary;
  }

  public void HandleDictionary(Hashtable materialToMesh)
  {
    foreach (DictionaryEntry dictionaryEntry in materialToMesh)
    {
      MeshCombineUtility.MeshInstance[] array1 = (MeshCombineUtility.MeshInstance[]) ((ArrayList) dictionaryEntry.Value).ToArray(typeof (MeshCombineUtility.MeshInstance));
      int index = 0;
      while (index < array1.Length)
      {
        int num = 0;
        List<MeshCombineUtility.MeshInstance> meshInstanceList = new List<MeshCombineUtility.MeshInstance>();
        for (; index < array1.Length && num + array1[index].mesh.vertexCount < 65000; ++index)
        {
          num += array1[index].mesh.vertexCount;
          meshInstanceList.Add(array1[index]);
        }
        MeshCombineUtility.MeshInstance[] array2 = meshInstanceList.ToArray();
        GameObject gameObject = new GameObject("Combined mesh");
        gameObject.transform.parent = this.transform;
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.layer = this.gameObject.layer;
        gameObject.AddComponent(typeof (MeshFilter));
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<Renderer>().material = (Material) dictionaryEntry.Key;
        MeshFilter component = (MeshFilter) gameObject.GetComponent(typeof (MeshFilter));
        component.mesh = MeshCombineUtility.Combine(array2, this.generateTriangleStrips);
        if (this.addMeshCollider)
          gameObject.AddComponent<MeshCollider>().sharedMesh = component.mesh;
      }
    }
  }
}
