// Decompiled with JetBrains decompiler
// Type: CarDamage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class CarDamage : MonoBehaviour
{
  public MeshCollider meshCollider;
  public MeshFilter[] meshFilters;
  private MeshFilter[] m_meshFilters;
  public float deformNoise = 0.005f;
  public float deformRadius = 0.5f;
  private float bounceBackSleepCap = 1f / 500f;
  public float bounceBackSpeed = 2f;
  private Vector3[] colliderVerts;
  private CarDamage.permaVertsColl[] originalMeshData;
  private bool sleep = true;
  public float maxDeform = 0.5f;
  private float minForce = 5f;
  public float multiplier = 0.1f;
  public float YforceDamp = 1f;
  [HideInInspector]
  public bool repair;
  private Vector3 vec;
  private Transform trs;
  private Transform myTransform;
  private GameObject GObody;
  private CarDynamics carDynamics;
  private Axles axles;
  private Rigidbody body;
  public float sign = 1f;
  private Quaternion rot = Quaternion.identity;
  private int wheelLayer;
  private int carLayer;
  private int i;

  public void Start()
  {
    this.myTransform = this.transform;
    this.body = this.GetComponent<Rigidbody>();
    if ((double) this.sign < -1.0 || (double) this.sign > 1.0)
      this.sign = 1f;
    if (this.meshFilters.Length == 0)
    {
      this.m_meshFilters = this.GetComponentsInChildren<MeshFilter>();
      int length = 0;
      for (this.i = 0; this.i < this.m_meshFilters.Length; ++this.i)
      {
        if ((UnityEngine.Object) this.m_meshFilters[this.i].GetComponent<Collider>() == (UnityEngine.Object) null)
          ++length;
      }
      this.meshFilters = new MeshFilter[length];
      int index = 0;
      for (this.i = 0; this.i < this.m_meshFilters.Length; ++this.i)
      {
        if ((UnityEngine.Object) this.m_meshFilters[this.i].GetComponent<Collider>() == (UnityEngine.Object) null)
        {
          this.meshFilters[index] = this.m_meshFilters[this.i];
          ++index;
        }
      }
    }
    if ((UnityEngine.Object) this.meshCollider != (UnityEngine.Object) null)
      this.colliderVerts = this.meshCollider.sharedMesh.vertices;
    this.LoadoriginalMeshData();
    foreach (Transform transform in this.transform)
    {
      if (transform.gameObject.tag == "Body" || transform.gameObject.name == "Body" || transform.gameObject.name == "body")
        this.GObody = transform.gameObject;
    }
    if ((bool) (UnityEngine.Object) this.GObody)
    {
      if ((double) this.sign == 0.0)
        this.sign = Mathf.Cos(this.GObody.transform.localEulerAngles.y * ((float) Math.PI / 180f));
      if ((double) this.GObody.transform.localEulerAngles.x != 0.0)
        this.rot = Quaternion.AngleAxis(this.GObody.transform.localEulerAngles.x * 3f, Vector3.right);
    }
    this.carDynamics = this.GetComponent<CarDynamics>();
    this.axles = this.GetComponent<Axles>();
    this.wheelLayer = this.axles.allWheels[0].transform.gameObject.layer;
    this.carLayer = this.transform.gameObject.layer;
  }

  private void LoadoriginalMeshData()
  {
    this.originalMeshData = new CarDamage.permaVertsColl[this.meshFilters.Length];
    for (this.i = 0; this.i < this.meshFilters.Length; ++this.i)
      this.originalMeshData[this.i].permaVerts = this.meshFilters[this.i].mesh.vertices;
  }

  public void Update()
  {
    if (!this.sleep && this.repair && (double) this.bounceBackSpeed > 0.0)
    {
      this.sleep = true;
      for (int index1 = 0; index1 < this.meshFilters.Length; ++index1)
      {
        Vector3[] vertices = this.meshFilters[index1].mesh.vertices;
        if (this.originalMeshData == null)
          this.LoadoriginalMeshData();
        for (int index2 = 0; index2 < vertices.Length; ++index2)
        {
          vertices[index2] += (this.originalMeshData[index1].permaVerts[index2] - vertices[index2]) * (Time.deltaTime * this.bounceBackSpeed);
          if ((double) (this.originalMeshData[index1].permaVerts[index2] - vertices[index2]).magnitude >= (double) this.bounceBackSleepCap)
            this.sleep = false;
        }
        this.meshFilters[index1].mesh.vertices = vertices;
        this.meshFilters[index1].mesh.RecalculateNormals();
        this.meshFilters[index1].mesh.RecalculateBounds();
      }
      if ((UnityEngine.Object) this.meshCollider != (UnityEngine.Object) null)
      {
        Mesh mesh = new Mesh();
        mesh.vertices = this.colliderVerts;
        mesh.triangles = this.meshCollider.sharedMesh.triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        this.meshCollider.sharedMesh = mesh;
        this.body.centerOfMass = this.carDynamics.centerOfMass.localPosition;
      }
      if (this.sleep)
        this.repair = false;
    }
    if (!Application.isEditor)
      return;
    this.YforceDamp = Mathf.Clamp01(this.YforceDamp);
  }

  public void OnCollisionEnter(Collision collision)
  {
    if ((UnityEngine.Object) collision.rigidbody != (UnityEngine.Object) null && (double) collision.rigidbody.mass < 1.0 / 1000.0 || collision.contacts.Length <= 0 || !((UnityEngine.Object) this.myTransform != (UnityEngine.Object) null))
      return;
    Vector3 direction = collision.relativeVelocity * (float) (1.0 - (double) Mathf.Abs(Vector3.Dot(this.myTransform.up, collision.contacts[0].normal)) * (double) this.YforceDamp);
    float cos = Mathf.Abs(Vector3.Dot(collision.contacts[0].normal, direction.normalized));
    if ((double) direction.magnitude * (double) cos < (double) this.minForce)
      return;
    this.sleep = false;
    this.vec = this.myTransform.InverseTransformDirection(direction) * this.multiplier * 0.1f;
    if (this.originalMeshData == null)
      this.LoadoriginalMeshData();
    for (int index = 0; index < this.meshFilters.Length; ++index)
    {
      if (this.meshFilters[index].gameObject.layer != this.wheelLayer || this.carLayer == this.wheelLayer)
        this.DeformMesh(this.meshFilters[index].mesh, this.originalMeshData[index].permaVerts, collision, cos, this.meshFilters[index].transform, this.sign, this.rot);
    }
    if (!((UnityEngine.Object) this.meshCollider != (UnityEngine.Object) null))
      return;
    Mesh mesh = new Mesh();
    mesh.vertices = this.meshCollider.sharedMesh.vertices;
    mesh.triangles = this.meshCollider.sharedMesh.triangles;
    this.DeformMesh(mesh, this.colliderVerts, collision, cos, this.meshCollider.transform, 1f, Quaternion.identity);
    this.meshCollider.sharedMesh = mesh;
    this.meshCollider.sharedMesh.RecalculateNormals();
    this.meshCollider.sharedMesh.RecalculateBounds();
    this.body.centerOfMass = this.carDynamics.centerOfMass.localPosition;
  }

  private void DeformMesh(
    Mesh mesh,
    Vector3[] originalMesh,
    Collision collision,
    float cos,
    Transform meshTransform,
    float sign,
    Quaternion rot)
  {
    Vector3[] vertices = mesh.vertices;
    foreach (ContactPoint contact in collision.contacts)
    {
      Vector3 vector3 = meshTransform.InverseTransformPoint(contact.point);
      for (int index = 0; index < vertices.Length; ++index)
      {
        if ((double) (vector3 - vertices[index]).magnitude < (double) this.deformRadius)
        {
          vertices[index] += rot * (this.vec * (this.deformRadius - (vector3 - vertices[index]).magnitude) / this.deformRadius * cos + UnityEngine.Random.onUnitSphere * this.deformNoise) * sign;
          if ((double) this.maxDeform > 0.0 && (double) (vertices[index] - originalMesh[index]).magnitude > (double) this.maxDeform)
            vertices[index] = originalMesh[index] + (vertices[index] - originalMesh[index]).normalized * this.maxDeform;
        }
      }
    }
    mesh.vertices = vertices;
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
  }

  private struct permaVertsColl
  {
    public Vector3[] permaVerts;
  }
}
