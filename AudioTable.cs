// Decompiled with JetBrains decompiler
// Type: AudioTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (AudioSource))]
public class AudioTable : MonoBehaviour
{
  public AudioTable.AudioTableDimension[] dimensions;
  public AudioTableClip[] audioClips;
  public float lastQuality;

  [ContextMenu("Create from Children")]
  public void Children()
  {
    foreach (AudioSource componentsInChild in this.GetComponentsInChildren<AudioSource>())
    {
      if (!((UnityEngine.Object) componentsInChild == (UnityEngine.Object) this.gameObject.GetComponent<AudioSource>()))
      {
        componentsInChild.gameObject.AddComponent<AudioTableClip>().audioClip = componentsInChild.clip;
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) componentsInChild);
      }
    }
    this.audioClips = this.GetComponentsInChildren<AudioTableClip>();
    foreach (AudioTableClip audioClip in this.audioClips)
    {
      audioClip.gameObject.name = audioClip.audioClip.name;
      if (audioClip.dimensions == null)
      {
        audioClip.dimensions = new AudioTable.AudioTableDimension[this.dimensions.Length];
        for (int index = 0; index < this.dimensions.Length; ++index)
        {
          audioClip.dimensions[index] = new AudioTable.AudioTableDimension();
          audioClip.dimensions[index].name = this.dimensions[index].name;
          audioClip.dimensions[index].position = this.dimensions[index].position;
          audioClip.dimensions[index].importance = this.dimensions[index].importance;
        }
      }
      if (audioClip.dimensions.Length == 0)
      {
        audioClip.dimensions = new AudioTable.AudioTableDimension[this.dimensions.Length];
        for (int index = 0; index < this.dimensions.Length; ++index)
        {
          audioClip.dimensions[index] = new AudioTable.AudioTableDimension();
          audioClip.dimensions[index].name = this.dimensions[index].name;
          audioClip.dimensions[index].position = this.dimensions[index].position;
          audioClip.dimensions[index].importance = this.dimensions[index].importance;
        }
      }
    }
  }

  [ContextMenu("Play Sound")]
  public AudioTableClip PlaySound()
  {
    AudioTableClip sound = this.GetSound();
    this.GetComponent<AudioSource>().Play();
    return sound;
  }

  public AudioTableClip GetSound()
  {
    float f = 0.0f;
    AudioTableClip sound = (AudioTableClip) null;
    foreach (AudioTableClip audioClip in this.audioClips)
    {
      if (audioClip.dimensions.Length != this.dimensions.Length)
      {
        this.LogInfo<AudioTable>("All AudioTableClips must have the same number of dimensions as the AudioTable they're a part of! Skipping this one...");
      }
      else
      {
        float num1 = 0.0f;
        for (int index = 0; index < this.dimensions.Length; ++index)
        {
          float num2 = this.dimensions[index].position - audioClip.dimensions[index].position;
          num1 += num2 * num2 * this.dimensions[index].importance * this.dimensions[index].importance;
        }
        if ((UnityEngine.Object) sound == (UnityEngine.Object) null || (double) num1 < (double) f)
        {
          sound = audioClip;
          f = num1;
        }
      }
    }
    if (!(bool) (UnityEngine.Object) sound)
      return (AudioTableClip) null;
    if (!(bool) (UnityEngine.Object) sound.audioClip)
      return (AudioTableClip) null;
    this.GetComponent<AudioSource>().clip = sound.audioClip;
    this.lastQuality = Mathf.Sqrt(f);
    return sound;
  }

  [Serializable]
  public class AudioTableDimension
  {
    public string name;
    public float position;
    public float importance;

    public AudioTableDimension()
    {
    }

    public AudioTableDimension(string newName) => this.name = newName;
  }
}
