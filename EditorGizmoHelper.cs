// Decompiled with JetBrains decompiler
// Type: EditorGizmoHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;
using UnityEngine;

#nullable disable
public static class EditorGizmoHelper
{
  public static Vector3 CameraPosition
  {
    get
    {
      Camera camera = Camera.current;
      if ((Object) camera == (Object) null)
        camera = Camera.main;
      return (Object) camera == (Object) null ? Vector3.zero : camera.transform.position;
    }
  }

  [Conditional("UNITY_EDITOR")]
  public static void DrawWireSphere(Vector3 position, float radius, Color color)
  {
  }

  [Conditional("UNITY_EDITOR")]
  public static void DrawWireCapsule(Vector3 start, Vector3 end, float radius, Color color)
  {
  }

  [Conditional("UNITY_EDITOR")]
  public static void DrawBone(Vector3 start, Vector3 end, Color color)
  {
  }

  private static float SphereAngularSize(Vector3 start, float radius, Vector3 cameraPosition)
  {
    float squareDistance = Vector3.SqrMagnitude(cameraPosition - start);
    return EditorGizmoHelper.CircleAngularSize(radius, squareDistance);
  }

  private static float CircleAngularSize(float radius, float squareDistance)
  {
    float num = Mathf.Tan(Mathf.Acos(Mathf.Sqrt(Mathf.Max(0.0f, squareDistance - radius * radius) / squareDistance)));
    return Mathf.Sqrt(squareDistance) * num;
  }

  [Conditional("UNITY_EDITOR")]
  public static void DrawJointLimit(
    Vector3 globalAnchorPosition,
    Vector3 globalRestDirection,
    Vector3 globalAxis,
    float highLimit,
    float lowLimit,
    Color color)
  {
  }

  [Conditional("UNITY_EDITOR")]
  public static void DrawAmputationPoint(
    Vector3 cutPoint,
    Vector3 cutDir,
    float radius,
    Color color)
  {
  }

  [Conditional("UNITY_EDITOR")]
  public static void DrawInertiaTensor(Rigidbody instantiatedRigidbody)
  {
  }
}
