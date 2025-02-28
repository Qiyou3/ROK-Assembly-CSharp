// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Geometry.OctAtom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common.Attributes;
using CodeHatch.ModTools.Textures;
using SmartAssembly.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Geometry
{
  [DoNotObfuscate]
  public class OctAtom : OctTile
  {
    [HideInInspector]
    public int atom;
    [NoEdit]
    public byte TilesetID;
    public OctAtom.CornerData[] Corners;
    private OctAtom.CornerData[] _CachedCorners;
    private SimpleLOD _LODHandler;
    private bool _CornersTransformed;
    private OctAtom.CornerData[] _newCorners = new OctAtom.CornerData[8];

    public SimpleLOD LODHandler
    {
      get
      {
        if ((UnityEngine.Object) this._LODHandler == (UnityEngine.Object) null)
          this._LODHandler = this.GetComponent<SimpleLOD>();
        return this._LODHandler;
      }
    }

    public void Awake() => this._CachedCorners = this.Corners;

    public void Init()
    {
      if (!this._CornersTransformed)
      {
        this.RotateCorners();
        this._CornersTransformed = true;
      }
      this.LODHandler.SetLOD(QualitySettings.maximumLODLevel);
    }

    public void Reset()
    {
      this.Corners = this._CachedCorners;
      this._CornersTransformed = false;
      this.transform.localPosition = Vector3.zero;
      this.transform.rotation = Quaternion.identity;
      this.transform.localScale = Vector3.one;
    }

    public void SetColor(int corner, Color newColor)
    {
      if (corner < 0 || corner >= this.Corners.Length)
        return;
      this.Corners[corner].SetColor(newColor, this.TilesetID);
    }

    [DebuggerHidden]
    public override IEnumerable<OctTileData> GetData()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      OctAtom.\u003CGetData\u003Ec__Iterator14 data = new OctAtom.\u003CGetData\u003Ec__Iterator14()
      {
        \u003C\u003Ef__this = this
      };
      // ISSUE: reference to a compiler-generated field
      data.\u0024PC = -2;
      return (IEnumerable<OctTileData>) data;
    }

    private void RotateCorners()
    {
      if (this.Corners.Length < 8)
        return;
      Quaternion localRotation = this.transform.localRotation;
      Vector3 localScale = this.transform.localScale;
      for (int index1 = 0; index1 < 2; ++index1)
      {
        for (int index2 = 0; index2 < 2; ++index2)
        {
          for (int index3 = 0; index3 < 2; ++index3)
          {
            float x = 1f;
            float y = 1f;
            float z = 1f;
            if (index2 == 0)
              x = -1f;
            if (index1 == 0)
              y = -1f;
            if (index3 == 0)
              z = -1f;
            Vector3 vector3 = new Vector3(x, y, z);
            vector3 = new Vector3(vector3.x * localScale.x, vector3.y * localScale.y, vector3.z * localScale.z);
            vector3 = localRotation * vector3;
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            if ((double) vector3.x > 0.0)
              num1 = 1;
            if ((double) vector3.y > 0.0)
              num2 = 1;
            if ((double) vector3.z > 0.0)
              num3 = 1;
            int index4 = index1 * 4 + index2 * 2 + index3;
            this._newCorners[num2 * 4 + num1 * 2 + num3] = this.Corners[index4];
          }
        }
      }
      this._newCorners.CopyTo((Array) this.Corners, 0);
    }

    [Serializable]
    public class CornerData
    {
      public RendererMaterialBinding[] CornerRenderers;

      public void SetColor(Color newColor, byte tilesetID)
      {
        OctTileset tilesetWithId = TilesetLibrary.GetTilesetWithID(tilesetID);
        for (int index = 0; index < this.CornerRenderers.Length; ++index)
          TilesetLibrary.ColorBinding(newColor, this.CornerRenderers[index], tilesetWithId);
      }
    }
  }
}
