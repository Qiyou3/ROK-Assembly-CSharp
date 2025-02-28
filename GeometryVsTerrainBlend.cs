// Decompiled with JetBrains decompiler
// Type: GeometryVsTerrainBlend
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (MeshFilter))]
[AddComponentMenu("Relief Terrain/Geometry Blend")]
[SelectionBase]
public class GeometryVsTerrainBlend : MonoBehaviour
{
  private const int progress_granulation = 1000;
  public double UpdTim;
  private int progress_count_max;
  private int progress_count_current;
  private string progress_description = string.Empty;
  public float blend_distance = 0.1f;
  public GameObject blendedObject;
  public bool VoxelBlendedObject;
  public float _DeferredBlendGloss = 0.8f;
  [HideInInspector]
  public bool undo_flag;
  [HideInInspector]
  public bool paint_flag;
  [HideInInspector]
  public int paint_mode;
  [HideInInspector]
  public float paint_size = 0.5f;
  [HideInInspector]
  public float paint_smoothness;
  [HideInInspector]
  public float paint_opacity = 1f;
  [HideInInspector]
  public RTPColorChannels vertex_paint_channel = RTPColorChannels.A;
  [HideInInspector]
  public int addTrisSubdivision;
  [HideInInspector]
  public float addTrisMinAngle;
  [HideInInspector]
  public float addTrisMaxAngle = 90f;
  private Vector3[] paint_vertices;
  private Vector3[] paint_normals;
  private int[] paint_tris;
  private Transform underlying_transform;
  private MeshRenderer underlying_renderer;
  [HideInInspector]
  public RaycastHit paintHitInfo;
  [HideInInspector]
  public bool paintHitInfo_flag;
  [HideInInspector]
  private Texture2D tmp_globalColorMap;
  [HideInInspector]
  public Vector3[] normals_orig;
  [HideInInspector]
  public Vector4[] tangents_orig;
  [HideInInspector]
  public bool baked_normals;
  [HideInInspector]
  public Mesh orig_mesh;
  [HideInInspector]
  public Mesh pmesh;
  [HideInInspector]
  public bool shader_global_blend_capabilities;
  [HideInInspector]
  public float StickOffset = 0.03f;
  [HideInInspector]
  public bool Sticked;
  [HideInInspector]
  public bool StickedOptimized = true;
  [HideInInspector]
  public bool ModifyTris;
  [HideInInspector]
  public bool BuildMeshFlag;
  [HideInInspector]
  public bool RealizePaint_Flag;
  [HideInInspector]
  public string save_path = string.Empty;
  [HideInInspector]
  public bool isBatched;

  private void Start() => this.SetupValues();

  public void SetupValues()
  {
    if (!(bool) (Object) this.blendedObject || !((Object) this.blendedObject.GetComponent(typeof (MeshRenderer)) != (Object) null) && !((Object) this.blendedObject.GetComponent(typeof (Terrain)) != (Object) null))
      return;
    if ((Object) this.underlying_transform == (Object) null)
      this.underlying_transform = this.transform.FindChild("RTP_blend_underlying");
    if ((Object) this.underlying_transform != (Object) null)
      this.underlying_renderer = (MeshRenderer) this.underlying_transform.gameObject.GetComponent(typeof (MeshRenderer));
    if (!((Object) this.underlying_renderer != (Object) null) || !((Object) this.underlying_renderer.sharedMaterial != (Object) null))
      return;
    ReliefTerrain component1 = (ReliefTerrain) this.blendedObject.GetComponent(typeof (ReliefTerrain));
    if ((bool) (Object) component1)
    {
      Material sharedMaterial = this.underlying_renderer.sharedMaterial;
      component1.RefreshTextures(sharedMaterial);
      component1.globalSettingsHolder.Refresh(sharedMaterial);
      if (sharedMaterial.HasProperty("RTP_DeferredAddPassSpec"))
        sharedMaterial.SetFloat("RTP_DeferredAddPassSpec", this._DeferredBlendGloss);
      if ((bool) (Object) component1.controlA)
        sharedMaterial.SetTexture("_Control", (Texture) component1.controlA);
      if ((bool) (Object) component1.ColorGlobal)
        sharedMaterial.SetTexture("_Splat0", (Texture) component1.ColorGlobal);
      if ((bool) (Object) component1.NormalGlobal)
        sharedMaterial.SetTexture("_Splat1", (Texture) component1.NormalGlobal);
      if ((bool) (Object) component1.TreesGlobal)
        sharedMaterial.SetTexture("_Splat2", (Texture) component1.TreesGlobal);
      if ((bool) (Object) component1.BumpGlobalCombined)
        sharedMaterial.SetTexture("_Splat3", (Texture) component1.BumpGlobalCombined);
    }
    Terrain component2 = (Terrain) this.blendedObject.GetComponent(typeof (Terrain));
    if ((bool) (Object) component2)
    {
      this.underlying_renderer.lightmapIndex = component2.lightmapIndex;
      if (LightmapSettings.lightmapsMode == LightmapsMode.SeparateDirectional)
        this.underlying_renderer.lightmapScaleOffset = new Vector4(0.5f, 0.5f, 0.0f, 0.0f);
      else
        this.underlying_renderer.lightmapScaleOffset = new Vector4(1f, 1f, 0.0f, 0.0f);
    }
    else
    {
      this.underlying_renderer.lightmapIndex = this.blendedObject.GetComponent<Renderer>().lightmapIndex;
      this.underlying_renderer.lightmapScaleOffset = this.blendedObject.GetComponent<Renderer>().lightmapScaleOffset;
    }
    if (!this.Sticked)
      return;
    if ((bool) (Object) component2)
    {
      this.GetComponent<Renderer>().lightmapIndex = component2.lightmapIndex;
      this.GetComponent<Renderer>().lightmapScaleOffset = new Vector4(1f, 1f, 0.0f, 0.0f);
    }
    else
    {
      this.GetComponent<Renderer>().lightmapIndex = this.blendedObject.GetComponent<Renderer>().lightmapIndex;
      this.GetComponent<Renderer>().lightmapScaleOffset = this.blendedObject.GetComponent<Renderer>().lightmapScaleOffset;
    }
  }
}
