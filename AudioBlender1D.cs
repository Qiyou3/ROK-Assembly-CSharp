// Decompiled with JetBrains decompiler
// Type: AudioBlender1D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class AudioBlender1D
{
  public AnimationCurve VolumeOverParameter;
  public AnimationCurve RangeOverParameter;
  public AudioField1D[] AudioFields;
  public float[] CrossfadePercentages;
  public AudioSource AudioSourcePrefab;

  public void Play(
    float parameter,
    float volumeMultiplier,
    float pitchMultiplier,
    float rangeMultiplier)
  {
  }
}
