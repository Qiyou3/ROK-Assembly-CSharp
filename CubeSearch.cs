// Decompiled with JetBrains decompiler
// Type: CubeSearch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
public static class CubeSearch
{
  private static readonly Ray _planeNx = new Ray(new Vector3(-0.5f, 0.0f, 0.0f), new Vector3(-1f, 0.0f, 0.0f));
  private static readonly Ray _planePX = new Ray(new Vector3(0.5f, 0.0f, 0.0f), new Vector3(1f, 0.0f, 0.0f));
  private static readonly Ray _planeNy = new Ray(new Vector3(0.0f, -0.5f, 0.0f), new Vector3(0.0f, -1f, 0.0f));
  private static readonly Ray _planePy = new Ray(new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.0f, 1f, 0.0f));
  private static readonly Ray _planeNz = new Ray(new Vector3(0.0f, 0.0f, -0.5f), new Vector3(0.0f, 0.0f, -1f));
  private static readonly Ray _planePz = new Ray(new Vector3(0.0f, 0.0f, 0.5f), new Vector3(0.0f, 0.0f, 1f));

  public static Vector3 GetNormal(Transform space, Vector3 cubePosition, Ray ray)
  {
    if ((Object) space == (Object) null)
      return CubeSearch.GetNormal(cubePosition, ray);
    ray.origin = space.InverseTransformPoint(ray.origin);
    ray.direction = space.InverseTransformVector(ray.direction);
    cubePosition = space.InverseTransformPoint(cubePosition);
    Vector3 normal = CubeSearch.GetNormal(cubePosition, ray);
    return space.TransformVector(normal).normalized;
  }

  public static Vector3 GetNormal(Vector3 cubePosition, Ray ray)
  {
    ray.origin -= cubePosition;
    ray.direction = -ray.direction;
    return CubeSearch.Trace(ray);
  }

  public static void GetCubes(
    Transform space,
    Vector3 start,
    Vector3 end,
    Vector3[] cubesBuffer,
    out int numResults)
  {
    if ((Object) space == (Object) null)
    {
      CubeSearch.GetCubes(start, end, cubesBuffer, out numResults);
    }
    else
    {
      start = space.InverseTransformPoint(start);
      end = space.InverseTransformPoint(end);
      CubeSearch.GetCubes(start, end, cubesBuffer, out numResults);
      for (int index = 0; index < numResults; ++index)
        cubesBuffer[index] = space.TransformPoint(cubesBuffer[index]);
    }
  }

  public static void GetCubes(
    Vector3 start,
    Vector3 end,
    Vector3[] cubesBuffer,
    out int numResults)
  {
    Vector3 normalized = (end - start).normalized;
    Vector3 vector = start.RoundPoint();
    cubesBuffer[0] = vector;
    numResults = 1;
    while (numResults < cubesBuffer.Length)
    {
      vector += CubeSearch.Trace(new Ray(start - vector, normalized));
      cubesBuffer[numResults] = vector;
      if ((double) Vector3.Distance(vector.ClosestOnLineSegment(start, end), end) < 9.9999997473787516E-05)
        break;
      ++numResults;
    }
  }

  private static Vector3 Trace(Ray ray)
  {
    if ((double) ray.direction.x < 0.0)
    {
      Vector3 vector3 = Vector3UtilEx.RayIntersectPlane(ray, CubeSearch._planeNx);
      if ((double) Mathf.Abs(vector3.y) <= 0.5 && (double) Mathf.Abs(vector3.z) <= 0.5)
        return CubeSearch._planeNx.direction;
    }
    else
    {
      Vector3 vector3 = Vector3UtilEx.RayIntersectPlane(ray, CubeSearch._planePX);
      if ((double) Mathf.Abs(vector3.y) <= 0.5 && (double) Mathf.Abs(vector3.z) <= 0.5)
        return CubeSearch._planePX.direction;
    }
    if ((double) ray.direction.y < 0.0)
    {
      Vector3 vector3 = Vector3UtilEx.RayIntersectPlane(ray, CubeSearch._planeNy);
      if ((double) Mathf.Abs(vector3.x) <= 0.5 && (double) Mathf.Abs(vector3.z) <= 0.5)
        return CubeSearch._planeNy.direction;
    }
    else
    {
      Vector3 vector3 = Vector3UtilEx.RayIntersectPlane(ray, CubeSearch._planePy);
      if ((double) Mathf.Abs(vector3.x) <= 0.5 && (double) Mathf.Abs(vector3.z) <= 0.5)
        return CubeSearch._planePy.direction;
    }
    if ((double) ray.direction.z < 0.0)
    {
      Vector3 vector3 = Vector3UtilEx.RayIntersectPlane(ray, CubeSearch._planeNz);
      if ((double) Mathf.Abs(vector3.x) <= 0.5 && (double) Mathf.Abs(vector3.y) <= 0.5)
        return CubeSearch._planeNz.direction;
    }
    else
    {
      Vector3 vector3 = Vector3UtilEx.RayIntersectPlane(ray, CubeSearch._planePz);
      if ((double) Mathf.Abs(vector3.x) <= 0.5 && (double) Mathf.Abs(vector3.y) <= 0.5)
        return CubeSearch._planePz.direction;
    }
    return Vector3.zero;
  }
}
