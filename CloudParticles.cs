// Decompiled with JetBrains decompiler
// Type: CloudParticles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class CloudParticles : MonoBehaviour
{
  private CloudParticles.CloudParticle[] cloudParticles;
  private Vector3[] vertices;
  private Vector3[] normals;
  private Vector4[] tangents;
  private Vector2[] uv;
  private int[] indices;
  public Mesh mesh;
  public MeshFilter meshFilter;
  public Camera m_camera;
  private Renderer[] _renderers;
  private Material[] _materials;
  public int numParticles = 1000;
  public float range = 1000f;
  public float altitude = 1000f;
  public float thickness = 200f;
  public float width = 500f;
  public float updateRate = 10f;
  private float lastUpdateTime;
  public float minCameraMove = 10f;
  private Vector3 lastCameraUpdate;

  public Camera camera
  {
    get
    {
      if ((UnityEngine.Object) this.m_camera == (UnityEngine.Object) null)
      {
        CloudLayer cloudLayer = this.gameObject.FirstComponentAncestor<CloudLayer>();
        if ((UnityEngine.Object) cloudLayer != (UnityEngine.Object) null)
          this.m_camera = cloudLayer.camera;
        if ((UnityEngine.Object) this.m_camera == (UnityEngine.Object) null)
          this.m_camera = Camera.main;
        if ((UnityEngine.Object) this.m_camera == (UnityEngine.Object) null)
          this.m_camera = UnityEngine.Object.FindObjectOfType(typeof (Camera)) as Camera;
        if ((UnityEngine.Object) this.m_camera == (UnityEngine.Object) null)
          this.LogError<CloudParticles>("m_camera == null");
      }
      return this.m_camera;
    }
  }

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

  public Vector3 LocalCameraPosition
  {
    get => this.transform.InverseTransformPoint(this.camera.transform.position);
  }

  public float Altitude => this.altitude / this.transform.lossyScale.y;

  private void DrawPoint(Vector3 point, Color c)
  {
    Debug.DrawRay(point, Vector3.up * this.range / 1000f, c);
  }

  private void DrawPoint(Vector3 point) => this.DrawPoint(point, Color.white);

  private void InitParticle(ref CloudParticles.CloudParticle particle)
  {
    particle.position = this.LocalCameraPosition;
    Vector2 vector2 = UnityEngine.Random.insideUnitCircle * this.range;
    particle.position.x += vector2.x;
    particle.position.z += vector2.y;
    particle.position.y = this.Altitude;
    particle.normal = Vector3.back;
    particle.tangent = Vector3.right;
    particle.binormal = Vector3.up;
    this.UpdateParticleTangentSpace(ref particle);
    this.DrawPoint(particle.position);
  }

  private void UpdateParticle(ref CloudParticles.CloudParticle particle)
  {
    particle.position.y = this.Altitude;
    Vector2 vector2_1;
    vector2_1.x = particle.position.x - this.LocalCameraPosition.x;
    vector2_1.y = particle.position.z - this.LocalCameraPosition.z;
    float magnitude = vector2_1.magnitude;
    if ((double) magnitude > (double) this.range)
    {
      Vector2 vector2_2 = vector2_1 / magnitude + UnityEngine.Random.insideUnitCircle;
      if ((double) vector2_2.magnitude > 1.0)
        vector2_2.Normalize();
      particle.position = this.LocalCameraPosition;
      particle.position.y = this.Altitude;
      particle.position.x -= vector2_2.x * this.range;
      particle.position.z -= vector2_2.y * this.range;
    }
    this.UpdateParticleTangentSpace(ref particle);
  }

  private void UpdateParticleTangentSpace(ref CloudParticles.CloudParticle particle)
  {
    particle.normal = (this.LocalCameraPosition - particle.position).normalized;
    particle.binormal = -Vector3.Cross(particle.normal, particle.tangent).normalized;
    particle.tangent = Vector3.Cross(particle.normal, particle.binormal).normalized;
  }

  private void AssignVertices()
  {
    for (int i = 0; i < this.numParticles; ++i)
      this.AssignVertices(this.cloudParticles[i], i);
  }

  private void AssignVertices(CloudParticles.CloudParticle particle, int i)
  {
    Vector3 vector3_1 = particle.position + particle.tangent * -this.width + particle.binormal * -this.thickness;
    Vector3 vector3_2 = particle.position + particle.tangent * this.width + particle.binormal * -this.thickness;
    Vector3 vector3_3 = particle.position + particle.tangent * -this.width + particle.binormal * this.thickness;
    Vector3 vector3_4 = particle.position + particle.tangent * this.width + particle.binormal * this.thickness;
    int index = i * 4;
    this.vertices[index] = vector3_1;
    this.vertices[index + 1] = vector3_2;
    this.vertices[index + 2] = vector3_3;
    this.vertices[index + 3] = vector3_4;
  }

  private void AssignNormals()
  {
    for (int index1 = 0; index1 < this.numParticles; ++index1)
    {
      int index2 = index1 * 4;
      this.normals[index2] = this.cloudParticles[index1].normal;
      this.normals[index2 + 1] = this.cloudParticles[index1].normal;
      this.normals[index2 + 2] = this.cloudParticles[index1].normal;
      this.normals[index2 + 3] = this.cloudParticles[index1].normal;
    }
  }

  private void AssignTangents()
  {
    for (int index1 = 0; index1 < this.numParticles; ++index1)
    {
      int index2 = index1 * 4;
      Vector4 vector4 = new Vector4(this.cloudParticles[index1].tangent.x, this.cloudParticles[index1].tangent.y, this.cloudParticles[index1].tangent.z, -1f);
      this.tangents[index2] = vector4;
      this.tangents[index2 + 1] = vector4;
      this.tangents[index2 + 2] = vector4;
      this.tangents[index2 + 3] = vector4;
    }
  }

  private void AssignUVs()
  {
    Vector2 vector2_1 = new Vector2(0.0f, 0.0f);
    Vector2 vector2_2 = new Vector2(1f, 0.0f);
    Vector2 vector2_3 = new Vector2(0.0f, 1f);
    Vector2 vector2_4 = new Vector2(1f, 1f);
    for (int index1 = 0; index1 < this.numParticles; ++index1)
    {
      int index2 = 4 * index1;
      this.uv[index2] = vector2_1;
      this.uv[index2 + 1] = vector2_2;
      this.uv[index2 + 2] = vector2_3;
      this.uv[index2 + 3] = vector2_4;
    }
  }

  private void AssignIndices()
  {
    for (int index1 = 0; index1 < this.numParticles; ++index1)
    {
      int num1 = 4 * index1;
      int num2 = num1 + 2;
      int num3 = num1 + 1;
      int num4 = num1 + 1;
      int num5 = num1 + 2;
      int num6 = num1 + 3;
      int index2 = index1 * 6;
      this.indices[index2] = num1;
      this.indices[index2 + 1] = num2;
      this.indices[index2 + 2] = num3;
      this.indices[index2 + 3] = num4;
      this.indices[index2 + 4] = num5;
      this.indices[index2 + 5] = num6;
    }
  }

  public void Start()
  {
    this.cloudParticles = new CloudParticles.CloudParticle[this.numParticles];
    for (int index = 0; index < this.numParticles; ++index)
      this.InitParticle(ref this.cloudParticles[index]);
    this.vertices = new Vector3[this.numParticles * 4];
    this.normals = new Vector3[this.numParticles * 4];
    this.tangents = new Vector4[this.numParticles * 4];
    this.uv = new Vector2[this.numParticles * 4];
    this.indices = new int[this.numParticles * 6];
    this.AssignVertices();
    this.AssignNormals();
    this.AssignTangents();
    this.AssignUVs();
    this.AssignIndices();
    this.mesh = new Mesh();
    this.mesh.vertices = this.vertices;
    this.mesh.normals = this.normals;
    this.mesh.tangents = this.tangents;
    this.mesh.uv = this.uv;
    this.mesh.triangles = this.indices;
    this.mesh.RecalculateBounds();
    if ((UnityEngine.Object) this.meshFilter == (UnityEngine.Object) null)
      this.GetComponent<MeshFilter>();
    if ((UnityEngine.Object) this.meshFilter != (UnityEngine.Object) null)
      this.meshFilter.mesh = this.mesh;
    this.lastCameraUpdate = this.LocalCameraPosition;
  }

  public void LateUpdate()
  {
    if ((double) Time.time - (double) this.lastUpdateTime <= 1.0 / (double) this.updateRate)
      return;
    if ((double) Vector3.Distance(this.LocalCameraPosition, this.lastCameraUpdate) <= (double) this.minCameraMove)
    {
      this.lastUpdateTime = Time.time;
    }
    else
    {
      for (int index = 0; index < this.numParticles; ++index)
        this.UpdateParticle(ref this.cloudParticles[index]);
      this.AssignVertices();
      this.AssignNormals();
      this.AssignTangents();
      this.mesh.vertices = this.vertices;
      this.mesh.normals = this.normals;
      this.mesh.tangents = this.tangents;
      this.lastCameraUpdate = this.LocalCameraPosition;
      this.LocalCameraPosition.y = this.Altitude;
      this.mesh.RecalculateBounds();
      this.lastUpdateTime = Time.time;
    }
  }

  public struct CloudParticle
  {
    public Vector3 position;
    public Vector3 normal;
    public Vector3 tangent;
    public Vector3 binormal;
  }
}
