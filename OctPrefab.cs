// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Geometry.OctPrefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Blocks.Components;
using CodeHatch.Common;
using CodeHatch.Common.Attributes;
using CodeHatch.ModTools.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Geometry
{
  public class OctPrefab : MonoBehaviour
  {
    public const int Edge0 = 1;
    public const int Edge1 = 2;
    public const int Edge2 = 4;
    public const int Edge3 = 8;
    public const int Edge4 = 16;
    public const int Edge5 = 32;
    public const int Edge6 = 64;
    public const int Edge7 = 128;
    public const int Edge8 = 256;
    public const int Edge9 = 512;
    public const int Edge10 = 1024;
    public const int Edge11 = 2048;
    private static readonly int[] Edges = new int[12]
    {
      1,
      2,
      4,
      8,
      16,
      32,
      64,
      128,
      256,
      512,
      1024,
      2048
    };
    private static readonly Vector3[] EdgeMid = new Vector3[12]
    {
      Vector3.forward - Vector3.up,
      Vector3.right - Vector3.up,
      -Vector3.forward - Vector3.up,
      -Vector3.right - Vector3.up,
      Vector3.forward + Vector3.up,
      Vector3.right + Vector3.up,
      -Vector3.forward + Vector3.up,
      -Vector3.right + Vector3.up,
      Vector3.forward - Vector3.right,
      Vector3.forward + Vector3.right,
      -Vector3.forward + Vector3.right,
      -Vector3.forward - Vector3.right
    };
    [MinValue(0.0f)]
    public int PrefabID;
    [MinValue(1f)]
    public int TilesetID;
    [Bitwise(12, "(x, y, z)", new string[] {"(0, 0, 0)", "(0, 90, 0)", "(0, 180, 0)", "(0, 270, 0)", "(180, -180, 0)", "(180, -90, 0)", "(180, 0, 0)", "(180, -270, 0)", "(180, 0, 90)", "(180, 90, 90)", "(180, 180, 90)", "(180, 270, 90)"})]
    public int PermittedRotations;
    public Vector3[] BlockOffsets = new Vector3[1]
    {
      Vector3.zero
    };
    public OctPrefabGizmosSettings GizmoSettings;
    [MinValue(-1f)]
    [SerializeField]
    private float _BuildTime = -1f;
    public bool UseDefaultBuildRange = true;
    [Tooltip("The Build Range used if 'UseDefualtBuildRange' is false")]
    public float BuildRange = 7f;
    private RendererMaterialBinding[] _ChildBindings;
    public Bounds Bounds;

    public RendererMaterialBinding[] ChildBindings
    {
      get
      {
        if (this._ChildBindings == null)
          this._ChildBindings = this.GetComponentsInChildren<RendererMaterialBinding>(true);
        return this._ChildBindings;
      }
    }

    public bool IsMultiBlock() => this.BlockOffsets != null && this.BlockOffsets.Length > 1;

    public void OnValidate()
    {
      this.Bounds = new Bounds();
      Bounds bounds = new Bounds(Vector3.zero, Vector3.one * 1.2f);
      for (int index = 0; index < this.BlockOffsets.Length; ++index)
      {
        bounds.center = this.BlockOffsets[index] * 1.2f;
        this.Bounds.Encapsulate(bounds);
      }
    }

    public void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube(this.transform.position + this.Bounds.center, this.Bounds.size);
      for (int index = 0; index < this.BlockOffsets.Length; ++index)
      {
        Gizmos.color = Color.Lerp(Color.green, Color.blue, (float) index / (float) this.BlockOffsets.Length);
        if (this.GizmoSettings.gizmoType == OctPrefabGizmosSettings.GizmoType.Solid)
          Gizmos.DrawCube(this.transform.position + this.BlockOffsets[index] * 1.2f, Vector3.one * 1.2f);
        else
          Gizmos.DrawWireCube(this.transform.position + this.BlockOffsets[index] * 1.2f, Vector3.one * 1.2f);
      }
    }

    public void Start()
    {
      BundleLoadRendererAssets component = this.GetComponent<BundleLoadRendererAssets>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        if (!component.done)
          component.OnComplete += new System.Action(this.OnRenderersInit);
        else
          this.OnRenderersInit();
      }
      if (!this.IsMultiBlock())
        return;
      this.UpdatePrefabLinks();
    }

    public float BuildTime
    {
      get => this._BuildTime;
      set
      {
        if ((double) value < -1.0)
          this._BuildTime = -1f;
        else
          this._BuildTime = value;
      }
    }

    public void SetColor(Color newColor)
    {
      OctTileset tilesetWithId = TilesetLibrary.GetTilesetWithID((byte) this.TilesetID);
      for (int index = 0; index < this.ChildBindings.Length; ++index)
        TilesetLibrary.ColorBinding(newColor, this.ChildBindings[index], tilesetWithId);
    }

    public bool CanPlace(Vector3 blockPos, PageGrid octTree, Quaternion rotation)
    {
      Debug.LogError((object) "THIS METHOD WAS CHANGED BUT UNUSED, THEREFORE UNTESTED, TEST BEFORE USING");
      for (int index = 0; index < this.BlockOffsets.Length; ++index)
      {
        CubeInfo cubeInfoAt = octTree.GetCubeInfoAt(blockPos + rotation * this.BlockOffsets[index]);
        if (cubeInfoAt.PrefabID != (byte) 0 || cubeInfoAt.MaterialID != (byte) 0)
          return false;
      }
      return true;
    }

    public List<Vector3Int> GetBlockCoords(Vector3 worldPoint, Quaternion rotation)
    {
      List<Vector3Int> blockCoords = new List<Vector3Int>();
      Vector3Int localCoordinate = BlockManager.DefaultCubeGrid.WorldToLocalCoordinate(worldPoint);
      for (int index = 0; index < this.BlockOffsets.Length; ++index)
        blockCoords.Add(new Vector3Int((Vector3) localCoordinate + rotation * this.BlockOffsets[index]));
      return blockCoords;
    }

    public List<Vector3Int> GetBlockCoords()
    {
      List<Vector3Int> blockCoords = new List<Vector3Int>();
      Vector3Int localCoordinate = BlockManager.DefaultCubeGrid.WorldToLocalCoordinate(this.transform.position);
      for (int index = 0; index < this.BlockOffsets.Length; ++index)
        blockCoords.Add(new Vector3Int((Vector3) localCoordinate + this.transform.rotation * this.BlockOffsets[index]));
      return blockCoords;
    }

    public void UpdatePrefabLinks()
    {
      this.gameObject.FirstComponentAncestor<RootCubeGrid>().UpdatePrefabReferences(this);
    }

    private void OnRenderersInit()
    {
      SimpleLOD component1 = this.GetComponent<SimpleLOD>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.SetLOD(component1.CurrentLevel);
      ColorableOctPrefab component2 = this.GetComponent<ColorableOctPrefab>();
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
        return;
      this.SetColor(component2.CurrentColor(this.transform.position));
    }

    public static Quaternion AlignPrefab(
      Vector3 normal,
      Vector3 playerViewNormal,
      Vector3 worldPoint,
      RootCubeGrid grid,
      OctPrefab prefab)
    {
      if ((UnityEngine.Object) grid == (UnityEngine.Object) null)
      {
        Logger.Null<OctPrefab>(nameof (grid));
        return Quaternion.identity;
      }
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      {
        Logger.Null<OctPrefab>(nameof (prefab));
        return Quaternion.identity;
      }
      normal = OctPrefab.AlignNormal(grid.transform.InverseTransformDirection(normal));
      playerViewNormal = OctPrefab.AlignNormalFlat(grid.transform.InverseTransformDirection(playerViewNormal)) * 0.5f;
      playerViewNormal = new Vector3(playerViewNormal.x, -normal.y, playerViewNormal.z);
      Vector3 vector3_1 = grid.transform.InverseTransformPoint(worldPoint);
      Vector3 vector3_2 = new Vector3(Mathf.Round(vector3_1.x), Mathf.Round(vector3_1.y), Mathf.Round(vector3_1.z));
      Vector3 hitLocation = vector3_1 - vector3_2;
      float x = hitLocation.x;
      float y = hitLocation.y;
      float z = hitLocation.z;
      if ((double) normal.x > 0.0 && (double) x < 0.0 || (double) normal.x < 0.0 && (double) x > 0.0)
        x = -x;
      if ((double) normal.z > 0.0 && (double) z < 0.0 || (double) normal.z < 0.0 && (double) z > 0.0)
        z = -z;
      if ((double) normal.y > 0.0 && (double) y > 0.0 || (double) normal.y < 0.0 && (double) y < 0.0)
        y = -y;
      hitLocation = new Vector3(x, y, z);
      float num1 = Mathf.Abs(normal.x);
      float num2 = Mathf.Abs(normal.y);
      float num3 = Mathf.Abs(normal.z);
      if ((double) num2 > (double) num1 && (double) num2 > (double) num3)
      {
        float num4 = -x;
        float num5 = -z;
      }
      Quaternion quaternion = Quaternion.Euler(0.0f, 45f, 0.0f);
      OctPrefab.AlignmentProb[] alignmentProbArray = OctPrefab.ResolveLikelyRotations(hitLocation, normal, playerViewNormal);
      for (int index = 0; index < 12; ++index)
      {
        bool flag = false;
        if ((alignmentProbArray[index].Alignment & prefab.PermittedRotations) != 0)
        {
          int alignment = alignmentProbArray[index].Alignment;
          switch (alignment)
          {
            case 1:
              quaternion = (double) num3 <= (double) num2 ? Quaternion.Euler(0.0f, 180f, 0.0f) : Quaternion.identity;
              goto label_38;
            case 2:
              quaternion = (double) num1 <= (double) num2 ? Quaternion.Euler(0.0f, 270f, 0.0f) : Quaternion.Euler(0.0f, 90f, 0.0f);
              goto label_38;
            case 4:
              quaternion = (double) num3 <= (double) num2 ? Quaternion.identity : Quaternion.Euler(0.0f, 180f, 0.0f);
              goto label_38;
            case 8:
              quaternion = (double) num1 <= (double) num2 ? Quaternion.Euler(0.0f, 90f, 0.0f) : Quaternion.Euler(0.0f, 270f, 0.0f);
              goto label_38;
            default:
              if (alignment != 16)
              {
                if (alignment != 32)
                {
                  if (alignment != 64)
                  {
                    if (alignment != 128)
                    {
                      if (alignment != 256)
                      {
                        if (alignment != 512)
                        {
                          if (alignment != 1024)
                          {
                            if (alignment == 2048)
                            {
                              quaternion = (double) num3 <= (double) num1 ? Quaternion.Euler(180f, 90f, 90f) : Quaternion.Euler(180f, 270f, 90f);
                              goto label_38;
                            }
                            else
                              goto label_38;
                          }
                          else
                          {
                            quaternion = (double) num1 <= (double) num3 ? Quaternion.Euler(180f, 0.0f, 90f) : Quaternion.Euler(180f, 180f, 90f);
                            goto label_38;
                          }
                        }
                        else
                        {
                          quaternion = (double) num3 <= (double) num1 ? Quaternion.Euler(180f, 270f, 90f) : Quaternion.Euler(180f, 90f, 90f);
                          goto label_38;
                        }
                      }
                      else
                      {
                        quaternion = (double) num1 <= (double) num3 ? Quaternion.Euler(180f, 180f, 90f) : Quaternion.Euler(180f, 0.0f, 90f);
                        goto label_38;
                      }
                    }
                    else
                    {
                      quaternion = (double) num1 <= (double) num2 ? Quaternion.Euler(180f, -90f, 0.0f) : Quaternion.Euler(180f, -270f, 0.0f);
                      goto label_38;
                    }
                  }
                  else
                  {
                    quaternion = (double) num3 <= (double) num2 ? Quaternion.Euler(180f, 180f, 0.0f) : Quaternion.Euler(180f, 0.0f, 0.0f);
                    goto label_38;
                  }
                }
                else
                {
                  quaternion = (double) num1 <= (double) num2 ? Quaternion.Euler(180f, -270f, 0.0f) : Quaternion.Euler(180f, -90f, 0.0f);
                  goto label_38;
                }
              }
              else
              {
                quaternion = (double) num3 <= (double) num2 ? Quaternion.Euler(180f, 0.0f, 0.0f) : Quaternion.Euler(180f, 180f, 0.0f);
                goto label_38;
              }
          }
        }
        else if (flag)
          break;
      }
label_38:
      return quaternion;
    }

    private static OctPrefab.AlignmentProb[] ResolveLikelyRotations(
      Vector3 hitLocation,
      Vector3 normal,
      Vector3 playerViewNormal)
    {
      OctPrefab.AlignmentProb[] source = new OctPrefab.AlignmentProb[12];
      bool flag = false;
      if ((double) Mathf.Abs(normal.y) > 0.5)
        flag = true;
      for (int index = 0; index < 12; ++index)
      {
        source[index].Alignment = OctPrefab.Edges[index];
        source[index].Prob = !flag ? Vector3.SqrMagnitude(OctPrefab.EdgeMid[index] - hitLocation) : Vector3.SqrMagnitude(OctPrefab.EdgeMid[index] - playerViewNormal);
      }
      return ((IEnumerable<OctPrefab.AlignmentProb>) source).OrderBy<OctPrefab.AlignmentProb, float>((Func<OctPrefab.AlignmentProb, float>) (o => o.Prob)).ToArray<OctPrefab.AlignmentProb>();
    }

    private static Vector3 AlignNormal(Vector3 normal)
    {
      normal = normal.normalized;
      float num1 = Mathf.Abs(normal.x);
      float num2 = Mathf.Abs(normal.y);
      float num3 = Mathf.Abs(normal.z);
      if ((double) num1 > (double) num2 && (double) num1 > (double) num3)
        return Vector3.right * Mathf.Sign(normal.x);
      return (double) num2 > (double) num3 ? Vector3.up * Mathf.Sign(normal.y) : Vector3.forward * Mathf.Sign(normal.z);
    }

    private static Vector3 AlignNormalFlat(Vector3 normal)
    {
      normal = normal.normalized;
      return (double) Mathf.Abs(normal.x) > (double) Mathf.Abs(normal.z) ? Vector3.right * Mathf.Sign(normal.x) : Vector3.forward * Mathf.Sign(normal.z);
    }

    public struct AlignmentProb
    {
      public int Alignment;
      public float Prob;
    }
  }
}
