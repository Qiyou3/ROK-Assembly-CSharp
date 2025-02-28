// Decompiled with JetBrains decompiler
// Type: IAlignmentTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public interface IAlignmentTracker
{
  Vector3 position { get; }

  Quaternion rotation { get; }

  Vector3 velocity { get; }

  Vector3 velocitySmoothed { get; }

  Vector3 acceleration { get; }

  Vector3 accelerationSmoothed { get; }

  Vector3 angularVelocity { get; }

  Vector3 angularVelocitySmoothed { get; }

  Vector3 up { get; }

  Vector3 forward { get; }

  Vector3 right { get; }

  float speed { get; }

  float speedSmooth { get; }

  void Reset();

  void ControlledFixedUpdate();

  void ControlledLateUpdate();
}
