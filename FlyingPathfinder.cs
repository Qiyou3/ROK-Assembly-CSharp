// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.FlyingPathfinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using System;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  [RequireComponent(typeof (FlyingTargeter))]
  public class FlyingPathfinder : PathfindingComponentBase
  {
    public int raysPerCycle = 10;
    public float rayLength = 1000f;

    public void UpdateCurrentTargetPath(HiveNode node)
    {
      TargetPath targetPath;
      this.TryFindPathToTarget(node.Target, out targetPath, node);
      node.SetTargetAndCurrentTargetPath(ref targetPath);
    }

    public override bool TryFindPathToTarget(
      Targetable target,
      out TargetPath targetPath,
      HiveNode node)
    {
      this.PrepareTracer(target, node);
      Vector3 position = target.Position;
      if (this.IsPointVisibleFromMyPosition(position, node))
      {
        targetPath = new TargetPath();
        targetPath.target = target;
        targetPath.targetDirectlyVisible = true;
        targetPath.unobstructedPathExists = true;
        targetPath.Waypoint = node.Position;
        return true;
      }
      Vector3 waypointOut;
      if (this.TryFindIndirectWaypointToTarget(position, out waypointOut, node))
      {
        targetPath = new TargetPath();
        targetPath.target = target;
        targetPath.targetDirectlyVisible = false;
        targetPath.unobstructedPathExists = true;
        targetPath.Waypoint = waypointOut;
        return true;
      }
      targetPath = new TargetPath();
      targetPath.target = target;
      targetPath.targetDirectlyVisible = false;
      targetPath.unobstructedPathExists = false;
      targetPath.Waypoint = node.Position;
      return false;
    }

    public void PrepareTracer(Targetable nextTargetToConsider, HiveNode node)
    {
      throw new NotImplementedException();
    }

    private bool TryFindIndirectWaypointToTarget(
      Vector3 targetPosition,
      out Vector3 waypointOut,
      HiveNode node)
    {
      Vector3 previousWaypoint = this.GetSearchRayDirectionFromPreviousWaypoint(targetPosition, ref node.CurrentTargetPath);
      for (int index = 0; index < this.raysPerCycle; ++index)
      {
        if (this.TryTraceIndirectPath(targetPosition, previousWaypoint, out waypointOut, node))
          return true;
        FlyingPathfinder.RandomizeSearchRayDirection(ref previousWaypoint);
      }
      waypointOut = Vector3.zero;
      return false;
    }

    private Vector3 GetSearchRayDirectionFromPreviousWaypoint(
      Vector3 enemyPosition,
      ref TargetPath currentTargetPath)
    {
      if (currentTargetPath.targetDirectlyVisible)
        return UnityEngine.Random.onUnitSphere;
      Vector3 vector3 = currentTargetPath.Waypoint - enemyPosition;
      return vector3 == Vector3.zero ? UnityEngine.Random.onUnitSphere : vector3.normalized;
    }

    private static void RandomizeSearchRayDirection(ref Vector3 searchRayDirection)
    {
      searchRayDirection = UnityEngine.Random.rotation * searchRayDirection;
    }

    private bool TryTraceIndirectPath(
      Vector3 targetPosition,
      Vector3 directionFromTarget,
      out Vector3 waypointOut,
      HiveNode node)
    {
      Vector3 visibilityEndpoint = this.GetVisibilityEndpoint(targetPosition, directionFromTarget);
      if (this.IsClosestPointOnLineVisibleFromMyPosition(targetPosition, visibilityEndpoint, out waypointOut, node))
        return true;
      if (this.IsPointVisibleFromMyPosition(visibilityEndpoint, node))
      {
        waypointOut = visibilityEndpoint;
        return true;
      }
      for (int index = 0; index < 2; ++index)
      {
        if (this.IsRandomPointOnLineVisibleFromMyPosition(targetPosition, visibilityEndpoint, out waypointOut, node))
          return true;
      }
      return false;
    }

    private bool IsClosestPointOnLineVisibleFromMyPosition(
      Vector3 targetPosition,
      Vector3 visibilityEndpoint,
      out Vector3 waypointOut,
      HiveNode node)
    {
      waypointOut = node.Position.ClosestOnLineSegment(targetPosition, visibilityEndpoint);
      return this.IsPointVisibleFromMyPosition(waypointOut, node);
    }

    private bool IsRandomPointOnLineVisibleFromMyPosition(
      Vector3 targetPosition,
      Vector3 visibilityEndpoint,
      out Vector3 waypointOut,
      HiveNode node)
    {
      waypointOut = Vector3.Lerp(targetPosition, visibilityEndpoint, UnityEngine.Random.value);
      return this.IsPointVisibleFromMyPosition(waypointOut, node);
    }

    private bool IsPointVisibleFromMyPosition(Vector3 point, HiveNode node)
    {
      return this.ArePointsVisibleToEachother(node.Position, point);
    }

    private Vector3 GetVisibilityEndpoint(Vector3 targetPosition, Vector3 directionFromTarget)
    {
      Ray ray = new Ray(targetPosition, directionFromTarget);
      throw new NotImplementedException();
    }

    private static void RetractEndpointDistanceToPreventClipping(ref RaycastHit raycastHit)
    {
      raycastHit.distance = Mathf.Max(0.0f, raycastHit.distance - 0.05f);
    }

    private bool ArePointsVisibleToEachother(Vector3 first, Vector3 second)
    {
      Vector3 direction = first - second;
      Ray ray = new Ray(second, direction);
      throw new NotImplementedException();
    }
  }
}
