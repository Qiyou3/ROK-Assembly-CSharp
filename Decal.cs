// Decompiled with JetBrains decompiler
// Type: Decal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.EffectsPooling;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class Decal : MonoBehaviour
{
  [SerializeField]
  private Material m_Material;
  public bool IsPersistant;
  private Vector3 _lastPosition;
  private Quaternion _lastRotation;
  private Vector3 _lastScale;
  private Material _lastMaterial;
  private bool _isVisible;
  private MeshRenderer _fauxRenderer;
  private MeshFilter _fauxFilter;
  private float _enableTime;
  private ChangeBlindVariable<bool> _changeBlindFadeVisible;

  public Material Material
  {
    get => this.m_Material;
    set => this.m_Material = value;
  }

  private bool IsVisible => this._fauxRenderer.isVisible;

  public void Awake()
  {
    this._enableTime = Time.time;
    this._changeBlindFadeVisible = new ChangeBlindVariable<bool>(10f, 30f);
  }

  public void OnEnable()
  {
    DeferredDecalSystem.AddDecal(this);
    this._fauxRenderer = this.gameObject.GetComponent<MeshRenderer>();
    if ((Object) this._fauxRenderer == (Object) null)
      this._fauxRenderer = this.gameObject.AddComponent<MeshRenderer>();
    this._fauxFilter = this.gameObject.GetComponent<MeshFilter>();
    if ((Object) this._fauxFilter == (Object) null)
      this._fauxFilter = this.gameObject.AddComponent<MeshFilter>();
    this._fauxFilter.mesh = new Mesh();
    this._fauxFilter.sharedMesh.bounds = new Bounds(Vector3.zero, Vector3.one);
  }

  public void OnDisable() => DeferredDecalSystem.RemoveDecal(this);

  public void UpdateVisibility()
  {
    if (!this.IsPersistant && (Object) DecalsQuality.Instance != (Object) null)
    {
      this._changeBlindFadeVisible.Value = (double) Time.time - (double) this._enableTime < (double) DecalsQuality.Instance.DecalFadeTime;
      if (!this._changeBlindFadeVisible.Value)
      {
        GameObject gameObject = this.gameObject;
        EffectObject componentInParent = this.gameObject.GetComponentInParent<EffectObject>();
        if ((bool) (Object) componentInParent)
          gameObject = componentInParent.gameObject;
        EffectsPool.Destroy(gameObject);
      }
    }
    if (this.IsVisible)
      return;
    this.UpdateState();
  }

  public void OnWillRenderObject() => this.UpdateState();

  public void UpdateState()
  {
    bool isVisible = this.IsVisible;
    if (isVisible != this._isVisible)
    {
      this._isVisible = isVisible;
      if (isVisible)
        DeferredDecalSystem.AddDecal(this);
      else
        DeferredDecalSystem.RemoveDecal(this);
    }
    else
    {
      if (this._lastPosition != this.transform.position || this._lastRotation != this.transform.rotation || this._lastScale != this.transform.lossyScale || (Object) this._lastMaterial != (Object) this.m_Material)
        DeferredDecalSystem.Instance.RequestUpdate();
      this._lastPosition = this.transform.position;
      this._lastRotation = this.transform.rotation;
      this._lastScale = this.transform.lossyScale;
      this._lastMaterial = this.m_Material;
    }
  }

  private void DrawGizmo(bool selected)
  {
    Color color = new Color(0.0f, 0.7f, 1f, 1f);
    color.a = !selected ? 0.0f : 0.05f;
    Gizmos.color = color;
    Gizmos.matrix = this.transform.localToWorldMatrix;
    Gizmos.DrawCube(Vector3.zero, Vector3.one);
    color.a = !selected ? 0.0f : 0.2f;
    Gizmos.color = color;
    Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
  }

  public void OnDrawGizmos() => this.DrawGizmo(false);

  public void OnDrawGizmosSelected() => this.DrawGizmo(true);
}
