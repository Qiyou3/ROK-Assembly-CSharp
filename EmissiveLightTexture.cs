// Decompiled with JetBrains decompiler
// Type: EmissiveLightTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class EmissiveLightTexture
{
  public Renderer renderer;
  public EmissiveLightTexture.Channel channel;
  public float offIntensity;
  public float onIntensity = 1f;

  public enum Channel
  {
    R,
    G,
    B,
  }
}
