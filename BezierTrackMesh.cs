// Decompiled with JetBrains decompiler
// Type: BezierTrackMesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class BezierTrackMesh : MonoBehaviour
{
  private readonly List<Color> colors = new List<Color>();
  private readonly List<int> triangles = new List<int>();
  private readonly List<Vector2> uv = new List<Vector2>();
  private readonly List<Vector3> vertices = new List<Vector3>();
  public AnimationCurve alpha;
  public BezierTrack bezierTrack;
  public BezierTrackMesh.CrossConnection[] crossConnections;
  public BezierTrackMesh.CrossSectionPoint[] crossSectionPoints;
  private Transform m_guideTransform;
  private Mesh mesh;
  public MeshFilter meshFilter;
  public int numSlices = 100;

  public void Start()
  {
    if ((UnityEngine.Object) this.bezierTrack == (UnityEngine.Object) null)
      this.bezierTrack = this.GetComponentInChildren<BezierTrack>();
    if ((UnityEngine.Object) this.bezierTrack == (UnityEngine.Object) null)
    {
      this.LogError<BezierTrackMesh>("BezierTrack not found.");
      this.enabled = false;
    }
    else
    {
      GameObject gameObject = new GameObject("Beizer Transform Reference");
      gameObject.hideFlags = HideFlags.HideAndDontSave;
      this.m_guideTransform = gameObject.transform;
      this.m_guideTransform.parent = this.transform.parent;
      if ((UnityEngine.Object) this.meshFilter == (UnityEngine.Object) null)
        this.meshFilter = this.GetComponentInChildren<MeshFilter>();
      this.UpdateMesh();
      if (!Application.isPlaying)
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.m_guideTransform.gameObject);
    }
  }

  private void UpdateMesh()
  {
    if ((UnityEngine.Object) this.mesh == (UnityEngine.Object) null)
    {
      this.mesh = new Mesh();
      this.mesh.hideFlags = HideFlags.HideAndDontSave;
    }
    else
      this.mesh.Clear();
    int length = this.crossSectionPoints.Length;
    this.vertices.Clear();
    this.triangles.Clear();
    this.uv.Clear();
    this.colors.Clear();
    for (int index1 = 0; index1 < this.numSlices; ++index1)
    {
      this.bezierTrack.SampleTransform(this.m_guideTransform, (float) (index1 * this.bezierTrack.Count) / (float) (this.numSlices - 1));
      if (index1 != 0)
      {
        int num1 = index1 * length;
        foreach (BezierTrackMesh.CrossConnection crossConnection in this.crossConnections)
        {
          int num2 = num1 + crossConnection.a;
          int num3 = num1 + crossConnection.b;
          int num4 = num2 - length;
          int num5 = num3 - length;
          this.triangles.AddRange((IEnumerable<int>) new int[6]
          {
            num4,
            num5,
            num2,
            num5,
            num3,
            num2
          });
        }
      }
      this.vertices.AddRange(((IEnumerable<BezierTrackMesh.CrossSectionPoint>) this.crossSectionPoints).Select<BezierTrackMesh.CrossSectionPoint, Vector3>((Func<BezierTrackMesh.CrossSectionPoint, Vector3>) (csp => this.m_guideTransform.TransformPoint(csp.position))));
      float u = (float) index1 / (float) (this.numSlices - 1);
      this.uv.AddRange(((IEnumerable<BezierTrackMesh.CrossSectionPoint>) this.crossSectionPoints).Select<BezierTrackMesh.CrossSectionPoint, Vector2>((Func<BezierTrackMesh.CrossSectionPoint, Vector2>) (csp => new Vector2(u * csp.uMult + csp.u, csp.v))));
      Color color = new Color(1f, 1f, 1f, this.alpha.Evaluate(u));
      for (int index2 = 0; index2 < length; ++index2)
        this.colors.Add(color);
    }
    this.mesh.vertices = this.vertices.ToArray();
    this.mesh.uv = this.uv.ToArray();
    this.mesh.triangles = this.triangles.ToArray();
    this.mesh.colors = this.colors.ToArray();
    this.mesh.RecalculateBounds();
    this.mesh.RecalculateNormals();
    this.mesh.Optimize();
    if (!((UnityEngine.Object) this.meshFilter != (UnityEngine.Object) null))
      return;
    this.meshFilter.sharedMesh = this.mesh;
  }

  public void Update()
  {
    if (Application.isPlaying)
      return;
    this.UpdateMesh();
  }

  public void OnDestroy()
  {
    if ((bool) (UnityEngine.Object) this.m_guideTransform)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.m_guideTransform.gameObject);
    if (!(bool) (UnityEngine.Object) this.mesh)
      return;
    this.mesh.Clear();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.mesh);
  }

  [Serializable]
  public class CrossConnection
  {
    public int a;
    public int b;
  }

  [Serializable]
  public class CrossSectionPoint
  {
    public Vector3 position;
    public float u;
    public float uMult = 1f;
    public float v = 0.5f;
  }
}
