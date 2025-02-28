// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.HiveNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class HiveNode : Targetable
  {
    private const int _maxTargetChainLength = 5;
    public static readonly List<HiveNode> allHiveNodes = new List<HiveNode>();
    public float lastTimeRootSeen;
    private Targetable target;
    public TargetPath CurrentTargetPath;
    private readonly List<Targetable> _targetChainList = new List<Targetable>(5);

    public Targetable Target
    {
      get => this.target;
      set => this.TrySetNextTarget(value);
    }

    public void ClearTarget()
    {
      this.target = (Targetable) null;
      this.InitializeWaypoint();
    }

    private void InitializeWaypoint()
    {
      this.CurrentTargetPath = new TargetPath()
      {
        Waypoint = this.Position
      };
    }

    public void SetTargetAndCurrentTargetPath(ref TargetPath targetPath)
    {
      Vector3 waypoint = this.CurrentTargetPath.Waypoint;
      this.CurrentTargetPath = targetPath;
      if (!targetPath.unobstructedPathExists)
        this.CurrentTargetPath.Waypoint = waypoint;
      this.Target = targetPath.target;
    }

    public Targetable TargetAtEndOfChain
    {
      get
      {
        Targetable target = this.target;
        for (int index = 0; index < 5 && !((UnityEngine.Object) target == (UnityEngine.Object) null) && this.IsFriendOf((Team) target); ++index)
        {
          HiveNode hiveNode = target as HiveNode;
          if (!((UnityEngine.Object) hiveNode == (UnityEngine.Object) null))
            target = hiveNode.target;
          else
            break;
        }
        return target;
      }
    }

    public bool IsValidNodeLink(Targetable possibleNextTarget)
    {
      if ((UnityEngine.Object) possibleNextTarget == (UnityEngine.Object) null || (UnityEngine.Object) this.target == (UnityEngine.Object) possibleNextTarget)
        return true;
      if ((UnityEngine.Object) this == (UnityEngine.Object) possibleNextTarget)
        return false;
      if ((UnityEngine.Object) (possibleNextTarget as HiveNode) != (UnityEngine.Object) null)
      {
        Targetable other = possibleNextTarget;
        for (int index = 0; index < 5; ++index)
        {
          if ((UnityEngine.Object) other == (UnityEngine.Object) this)
            return false;
          if (!((UnityEngine.Object) other == (UnityEngine.Object) null) && this.IsFriendOf((Team) other))
          {
            HiveNode hiveNode = other as HiveNode;
            if (!((UnityEngine.Object) hiveNode == (UnityEngine.Object) null))
              other = hiveNode.target;
            else
              break;
          }
          else
            break;
        }
      }
      return true;
    }

    private void TrySetNextTarget(Targetable possibleNextTarget)
    {
      this.target = this.IsValidNodeLink(possibleNextTarget) ? possibleNextTarget : throw new ArgumentException("Assign attempt invalid - would create circular chain of nodes and thus infinite recursion.");
    }

    public int TargetChainIndirectionLevels => this._targetChainList.Count;

    public List<Targetable> TargetChainList => this._targetChainList;

    public void RefreshTargetChainList()
    {
      this._targetChainList.Clear();
      if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
        return;
      Targetable target = this.target;
      for (int index = 0; index < 5; ++index)
      {
        this._targetChainList.Add(target);
        if (!this.IsFriendOf((Team) target))
          break;
        HiveNode hiveNode = target as HiveNode;
        if ((UnityEngine.Object) hiveNode == (UnityEngine.Object) null)
          break;
        target = hiveNode.target;
        if ((UnityEngine.Object) target == (UnityEngine.Object) null)
          break;
      }
    }

    public bool IsValidEnemyToConsider(Targetable target)
    {
      return this.IsTargetValidBaseTest(target) && !target.Entity.TryGet<IHealth>().IsDead;
    }

    public bool IsValidTargetInTargetChain(Targetable target)
    {
      return this.IsTargetValidBaseTest(target) && (!target.IsFriendOf((Team) this) || this.IsValidFriendHiveNode(target));
    }

    public bool IsValidFriendToConsider(HiveNode target)
    {
      return this.IsTargetValidBaseTest((Targetable) target) && this.IsValidFriendHiveNode((Targetable) target);
    }

    private bool IsTargetValidBaseTest(Targetable targetToValidate)
    {
      return !((UnityEngine.Object) targetToValidate == (UnityEngine.Object) null) && this.IsValidNodeLink(targetToValidate);
    }

    private bool IsValidFriendHiveNode(Targetable target)
    {
      if (!target.IsFriendOf((Team) this))
        return false;
      HiveNode hiveNode = target as HiveNode;
      return !((UnityEngine.Object) hiveNode == (UnityEngine.Object) null) && !((UnityEngine.Object) hiveNode.TargetAtEndOfChain == (UnityEngine.Object) null) && !hiveNode.TargetAtEndOfChain.IsFriendOf((Team) this);
    }

    public bool IsTargetDirectEnemy
    {
      get
      {
        return (UnityEngine.Object) this.Target != (UnityEngine.Object) null && !this.IsFriendOf((Team) this.Target) && this.CurrentTargetPath.targetDirectlyVisible;
      }
    }

    public new void Awake()
    {
      base.Awake();
      HiveNode.allHiveNodes.Add(this);
    }

    public void Start() => this.InitializeWaypoint();

    public void Update()
    {
      if (!this.IsValidEnemyToConsider(this.target))
        this.ClearTarget();
      this.RefreshTargetChainList();
    }

    public new void OnDestroy()
    {
      base.OnDestroy();
      HiveNode.allHiveNodes.Remove(this);
    }
  }
}
