// Decompiled with JetBrains decompiler
// Type: IMotionAnalyzer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public abstract class IMotionAnalyzer
{
  [HideInInspector]
  public string name;
  public AnimationClip animation;
  public MotionType motionType;
  public string motionGroup = "locomotion";

  public abstract int samples { get; }

  public abstract LegCycleData[] cycles { get; }

  public abstract Vector3 cycleDirection { get; }

  public abstract float cycleDistance { get; }

  public abstract Vector3 cycleVector { get; }

  public abstract float cycleDuration { get; }

  public abstract float cycleSpeed { get; }

  public abstract Vector3 cycleVelocity { get; }

  public abstract Vector3 GetFlightFootPosition(int leg, float flightTime, int phase);

  public abstract float cycleOffset { get; set; }

  public abstract void Analyze(GameObject o);
}
