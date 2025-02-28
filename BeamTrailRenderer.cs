// Decompiled with JetBrains decompiler
// Type: BeamTrailRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (MeshFilter), typeof (MeshRenderer))]
public class BeamTrailRenderer : MonoBehaviour
{
  public Transform BaseTransform;
  public Transform TipTransform;
  public int NumberOfBeams;
  private Mesh _mesh;
  private MeshRenderer _meshRenderer;
  private Material _materialInstance;
  private Vector3[] _vertices;
  private int[] _indices;
  private Vector2[] _uv;
  private int _startBeam;
  private float _spatioOffset;
  private float _temporalOffset;
  private Vector3 _previousBase;
  private Vector3 _previousTip;

  public void Start()
  {
    this._mesh = new Mesh();
    if (this.NumberOfBeams < 2)
      this.NumberOfBeams = 2;
    this.NumberOfBeams = Mathf.Max(2, this.NumberOfBeams);
    int length = Mathf.Max(4, this.NumberOfBeams * 2);
    this._vertices = new Vector3[length];
    this._uv = new Vector2[length];
    for (int index = 0; index < length; ++index)
      this._uv[index] = new Vector2(0.0f, index % 2 != 0 ? 1f : 0.0f);
    this._indices = new int[(this.NumberOfBeams - 1) * 6];
    int num1 = 0;
    int num2 = 0;
    int index1 = 0;
    while (num1 < this.NumberOfBeams - 1)
    {
      this._indices[index1] = num2;
      this._indices[index1 + 1] = num2 + 1;
      this._indices[index1 + 2] = num2 + 2;
      this._indices[index1 + 3] = num2 + 1;
      this._indices[index1 + 4] = num2 + 3;
      this._indices[index1 + 5] = num2 + 2;
      ++num1;
      num2 += 2;
      index1 += 6;
    }
    this.gameObject.GetComponent<MeshFilter>().sharedMesh = this._mesh;
    this.transform.parent = (Transform) null;
    this.transform.localPosition = Vector3.zero;
    this.transform.localRotation = Quaternion.identity;
    this.transform.localScale = Vector3.one;
    this._mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
    this._materialInstance = this.gameObject.GetComponent<MeshRenderer>().material;
  }

  public void LateUpdate()
  {
    if ((Object) this.BaseTransform == (Object) null || (Object) this.TipTransform == (Object) null)
    {
      Object.Destroy((Object) this.gameObject);
      this.enabled = false;
    }
    else
    {
      int index1 = this._startBeam * 2;
      this._vertices[index1] = this.BaseTransform.position;
      this._vertices[index1 + 1] = this.TipTransform.position;
      this._temporalOffset += Time.deltaTime;
      this._spatioOffset += Mathf.Max(Vector3.Distance(this.BaseTransform.position, this._previousBase), Vector3.Distance(this.TipTransform.position, this._previousTip));
      this._previousBase = this.BaseTransform.position;
      this._previousTip = this.TipTransform.position;
      this._uv[index1] = new Vector2(this._spatioOffset, this._temporalOffset);
      this._uv[index1 + 1] = new Vector2(-this._spatioOffset, -this._temporalOffset);
      int length = this._vertices.Length;
      for (int index2 = 0; index2 < this._indices.Length; ++index2)
      {
        int num = this._indices[index2] + 2;
        if (num >= length)
          num -= length;
        this._indices[index2] = num;
      }
      ++this._startBeam;
      if (this._startBeam >= this.NumberOfBeams)
        this._startBeam = 0;
      this._mesh.vertices = this._vertices;
      this._mesh.triangles = this._indices;
      this._mesh.uv = this._uv;
      this._mesh.RecalculateBounds();
      this._materialInstance.SetFloat("_SpatioOffset", this._spatioOffset);
      this._materialInstance.SetFloat("_TemporalOffset", this._temporalOffset);
    }
  }
}
