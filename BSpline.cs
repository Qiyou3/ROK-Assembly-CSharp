// Decompiled with JetBrains decompiler
// Type: BSpline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class BSpline : MonoBehaviour
{
  private Vector3[] _cachedControlPoints;
  public BSplineControlPoint[] controlPoints;
  public int n = 2;
  public float quality = 10f;
  private int[] _nV;

  public void Start()
  {
    this._cachedControlPoints = new Vector3[this.controlPoints.Length];
    this.CacheControlPoints();
    this._nV = new int[this._cachedControlPoints.Length + 5];
    this.CreateNodeVector();
    LineRenderer component = this.GetComponent<LineRenderer>();
    if (!(bool) (Object) component)
      return;
    int index = 0;
    Vector3[] array = this.Points.ToArray<Vector3>();
    component.SetVertexCount(array.Length);
    foreach (Vector3 position in array)
    {
      component.SetPosition(index, position);
      ++index;
    }
    component.useWorldSpace = false;
  }

  public Vector3 DeBoor(int r, int i, float u)
  {
    if (r == 0)
      return this._cachedControlPoints[i];
    float num = (u - (float) this._nV[i + r]) / (float) (this._nV[i + this.n + 1] - this._nV[i + r]);
    return this.DeBoor(r - 1, i, u) * (1f - num) + this.DeBoor(r - 1, i + 1, u) * num;
  }

  public void CreateNodeVector()
  {
    int num = 0;
    for (int index = 0; index < this.n + this._cachedControlPoints.Length + 1; ++index)
      this._nV[index] = index <= this.n ? num : (index > this._cachedControlPoints.Length ? num : ++num);
  }

  private void CacheControlPoints()
  {
    for (int index = 0; index < this.controlPoints.Length; ++index)
      this._cachedControlPoints[index] = this.controlPoints[index].transform.localPosition;
  }

  public void OnDrawGizmos()
  {
    if (Application.isPlaying || this.controlPoints.Length <= 0)
      return;
    this._cachedControlPoints = new Vector3[this.controlPoints.Length];
    this.CacheControlPoints();
    if (this._cachedControlPoints.Length <= 0)
      return;
    this._nV = new int[this._cachedControlPoints.Length + 5];
    this.CreateNodeVector();
    LineRenderer component = this.GetComponent<LineRenderer>();
    if ((bool) (Object) component)
    {
      int index = 0;
      Vector3[] array = this.Points.ToArray<Vector3>();
      component.SetVertexCount(array.Length);
      foreach (Vector3 position in array)
      {
        component.SetPosition(index, position);
        ++index;
      }
      component.useWorldSpace = false;
    }
    else
    {
      Gizmos.color = Color.gray;
      Vector3 from = this._cachedControlPoints[0];
      Vector3 to = Vector3.zero;
      for (float u = 0.0f; (double) u < (double) this._nV[this.n + this._cachedControlPoints.Length]; u += 0.1f)
      {
        for (int i = 0; i < this._cachedControlPoints.Length; ++i)
        {
          if ((double) u >= (double) i)
            to = this.DeBoor(this.n, i, u);
        }
        Gizmos.DrawLine(from, to);
        from = to;
      }
    }
  }

  public IEnumerable<Vector3> Points
  {
    get
    {
      BSpline.\u003C\u003Ec__IteratorDC points = new BSpline.\u003C\u003Ec__IteratorDC()
      {
        \u003C\u003Ef__this = this
      };
      points.\u0024PC = -2;
      return (IEnumerable<Vector3>) points;
    }
  }
}
