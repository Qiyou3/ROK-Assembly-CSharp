// Decompiled with JetBrains decompiler
// Type: IController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public interface IController
{
  Quaternion LookRotation { get; }

  Vector3 DesiredVelocity { get; }

  Vector3 DesiredAcceleration { get; }

  bool Jump { get; }

  bool Up { get; }

  bool Down { get; }

  bool Drop { get; }

  bool Fast { get; }
}
