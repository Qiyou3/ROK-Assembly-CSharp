// Decompiled with JetBrains decompiler
// Type: GraphUpdateScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using Pathfinding;
using UnityEngine;

#nullable disable
[AddComponentMenu("Pathfinding/GraphUpdateScene")]
public class GraphUpdateScene : GraphModifier
{
  public Vector3[] points;
  private Vector3[] convexPoints;
  [HideInInspector]
  public bool convex = true;
  [HideInInspector]
  public float minBoundsHeight = 1f;
  [HideInInspector]
  public int penaltyDelta;
  [HideInInspector]
  public bool modifyWalkability;
  [HideInInspector]
  public bool setWalkability;
  [HideInInspector]
  public bool applyOnStart = true;
  [HideInInspector]
  public bool applyOnScan = true;
  [HideInInspector]
  public bool useWorldSpace = true;
  public bool updatePhysics;
  public bool resetPenaltyOnPhysics = true;
  public bool updateErosion = true;
  [HideInInspector]
  public bool lockToY;
  [HideInInspector]
  public float lockToYValue;
  [HideInInspector]
  public bool modifyTag;
  [HideInInspector]
  public int setTag;
  private int setTagInvert;
  private bool firstApplied;

  public void Start()
  {
    if (this.firstApplied || !this.applyOnStart)
      return;
    this.Apply();
  }

  public override void OnPostScan()
  {
    if (!this.applyOnScan)
      return;
    this.Apply();
  }

  public virtual void InvertSettings()
  {
    this.setWalkability = !this.setWalkability;
    this.penaltyDelta = -this.penaltyDelta;
    if (this.setTagInvert == 0)
    {
      this.setTagInvert = this.setTag;
      this.setTag = 0;
    }
    else
    {
      this.setTag = this.setTagInvert;
      this.setTagInvert = 0;
    }
  }

  public void RecalcConvex()
  {
    if (this.convex)
      this.convexPoints = Polygon.ConvexHull(this.points);
    else
      this.convexPoints = (Vector3[]) null;
  }

  public void ToggleUseWorldSpace()
  {
    this.useWorldSpace = !this.useWorldSpace;
    if (this.points == null)
      return;
    Matrix4x4 matrix4x4 = !this.useWorldSpace ? this.transform.worldToLocalMatrix : this.transform.localToWorldMatrix;
    for (int index = 0; index < this.points.Length; ++index)
      this.points[index] = matrix4x4.MultiplyPoint3x4(this.points[index]);
  }

  public void LockToY()
  {
    if (this.points == null)
      return;
    for (int index = 0; index < this.points.Length; ++index)
      this.points[index].y = this.lockToYValue;
  }

  public void Apply(AstarPath active)
  {
    if (!this.applyOnScan)
      return;
    this.Apply();
  }

  public void Apply()
  {
    if ((Object) AstarPath.active == (Object) null)
    {
      this.LogError<GraphUpdateScene>("There is no AstarPath object in the scene");
    }
    else
    {
      this.firstApplied = true;
      GraphUpdateShape graphUpdateShape = new GraphUpdateShape();
      graphUpdateShape.convex = this.convex;
      Vector3[] vector3Array = this.points;
      if (!this.useWorldSpace)
      {
        vector3Array = new Vector3[this.points.Length];
        Matrix4x4 localToWorldMatrix = this.transform.localToWorldMatrix;
        for (int index = 0; index < vector3Array.Length; ++index)
          vector3Array[index] = localToWorldMatrix.MultiplyPoint3x4(this.points[index]);
      }
      graphUpdateShape.points = vector3Array;
      Bounds bounds = graphUpdateShape.GetBounds();
      if ((double) bounds.size.y < (double) this.minBoundsHeight)
        bounds.size = new Vector3(bounds.size.x, this.minBoundsHeight, bounds.size.z);
      AstarPath.active.UpdateGraphs(new GraphUpdateObject(bounds)
      {
        shape = graphUpdateShape,
        modifyWalkability = this.modifyWalkability,
        setWalkability = this.setWalkability,
        addPenalty = this.penaltyDelta,
        updatePhysics = this.updatePhysics,
        updateErosion = this.updateErosion,
        resetPenaltyOnPhysics = this.resetPenaltyOnPhysics,
        modifyTag = this.modifyTag,
        setTag = this.setTag
      });
    }
  }

  public void OnDrawGizmos() => this.OnDrawGizmos(false);

  public void OnDrawGizmosSelected() => this.OnDrawGizmos(true);

  public void OnDrawGizmos(bool selected)
  {
    if (this.points == null)
      return;
    Gizmos.color = !selected ? new Color(0.0f, 0.9f, 0.0f, 0.5f) : new Color(0.0f, 0.9f, 0.0f, 1f);
    Matrix4x4 matrix4x4 = !this.useWorldSpace ? this.transform.localToWorldMatrix : Matrix4x4.identity;
    for (int index = 0; index < this.points.Length; ++index)
      Gizmos.DrawLine(matrix4x4.MultiplyPoint3x4(this.points[index]), matrix4x4.MultiplyPoint3x4(this.points[(index + 1) % this.points.Length]));
    if (!this.convex)
      return;
    if (this.convexPoints == null)
      this.RecalcConvex();
    Gizmos.color = !selected ? new Color(0.9f, 0.0f, 0.0f, 0.5f) : new Color(0.9f, 0.0f, 0.0f, 1f);
    for (int index = 0; index < this.convexPoints.Length; ++index)
      Gizmos.DrawLine(matrix4x4.MultiplyPoint3x4(this.convexPoints[index]), matrix4x4.MultiplyPoint3x4(this.convexPoints[(index + 1) % this.convexPoints.Length]));
  }
}
