// Decompiled with JetBrains decompiler
// Type: Body
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public abstract class Body : EntityBehaviour
{
  public abstract Transform Pivot { get; }

  public abstract Vector3 Position { get; set; }

  public abstract Vector3 Forward { get; }

  public abstract Quaternion Rotation { get; set; }

  public abstract Orientation Orientation { get; set; }
}
