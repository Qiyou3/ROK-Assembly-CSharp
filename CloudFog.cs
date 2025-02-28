// Decompiled with JetBrains decompiler
// Type: CloudFog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class CloudFog : MonoBehaviour
{
  public float scaling = 1.2f;
  public float centerAltitude;
  public float thickness;
  public Transform sky;
  public float skyMaterialTileSize;
  public Vector2 skyMaterialTileOffset;
  private Renderer[] _renderers;
  private Material[] _materials;

  public Renderer[] Renderers
  {
    get
    {
      if (this._renderers == null)
        this._renderers = this.GetComponentsInChildren<Renderer>();
      return this._renderers;
    }
  }

  public Material[] Materials
  {
    get
    {
      if (this._materials == null)
        this._materials = ((IEnumerable<Renderer>) this.Renderers).SelectMany<Renderer, Material>((Func<Renderer, IEnumerable<Material>>) (r => (IEnumerable<Material>) r.materials)).ToArray<Material>();
      return this._materials;
    }
  }

  public void OnWillRenderObject()
  {
    Camera current = Camera.current;
    this.transform.position = current.transform.position;
    this.transform.localScale = new Vector3(current.nearClipPlane * this.scaling, current.nearClipPlane * this.scaling, current.nearClipPlane * this.scaling);
    if (Application.isPlaying)
    {
      Color color = this.GetComponent<Renderer>().material.color with
      {
        a = Mathf.Max(0.0f, this.thickness - Mathf.Abs(current.transform.position.y - this.centerAltitude)) / this.thickness
      };
      this.GetComponent<Renderer>().material.color = color;
      Vector3 vector3 = Quaternion.Inverse(this.sky.rotation) * -this.sky.position;
      this.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(this.skyMaterialTileOffset.x - vector3.x / this.skyMaterialTileSize, this.skyMaterialTileOffset.y - vector3.z / this.skyMaterialTileSize));
    }
    else
    {
      Color color = this.GetComponent<Renderer>().sharedMaterial.color with
      {
        a = Mathf.Max(0.0f, this.thickness - Mathf.Abs(current.transform.position.y - this.centerAltitude)) / this.thickness
      };
      this.GetComponent<Renderer>().sharedMaterial.color = color;
      Vector3 vector3 = Quaternion.Inverse(this.sky.rotation) * -this.sky.position;
      this.GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", new Vector2(this.skyMaterialTileOffset.x - vector3.x / this.skyMaterialTileSize, this.skyMaterialTileOffset.y - vector3.z / this.skyMaterialTileSize));
    }
  }
}
