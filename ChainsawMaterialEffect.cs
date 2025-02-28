// Decompiled with JetBrains decompiler
// Type: ChainsawMaterialEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ChainsawMaterialEffect : MonoBehaviour
{
  public AnimationCurve speedForVolume;
  public AnimationCurve speedForPitch;
  public AudioClip[] clips;
  public AudioClip[] censoredClips;
  public GameObject effect;
  public GameObject censoredEffect;
  public float resistance = 7f;
  public float resistanceVariation = 1.5f;
  public float fadeRate = 1f;
  public bool loop = true;
  public bool parentEffectToChainsaw = true;

  public GameObject GetEffect(bool censored) => censored ? this.censoredEffect : this.effect;

  public AudioClip[] GetClips(bool censored) => censored ? this.censoredClips : this.clips;
}
