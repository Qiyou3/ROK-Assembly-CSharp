// Decompiled with JetBrains decompiler
// Type: CodeHatch.Audio.CrossfadeClip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace CodeHatch.Audio
{
  [Serializable]
  public class CrossfadeClip
  {
    public AudioClip clip;
    public float crossfadeInTime;
    public float crossfadeOutTime;
    public float maxDistance;
    public float minDistance;
    public string name;
    public CrossfadeClip.Type type;
    public float volume;

    public string Name
    {
      get
      {
        if (this.name == string.Empty)
          this.name = this.clip.name;
        return this.name;
      }
    }

    public enum Type
    {
      Once,
      Loop,
    }
  }
}
