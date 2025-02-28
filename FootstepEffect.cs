// Decompiled with JetBrains decompiler
// Type: FootstepEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Thrones.Audio;
using UnityEngine;

#nullable disable
public class FootstepEffect : ScriptableObject
{
  public GameObject groundEffect;
  public GameObject footEffect;
  public MaterialAudioBindings[] bindings;

  public bool Valid(MaterialAudioBindings material)
  {
    return this.bindings.Contains<MaterialAudioBindings>(material);
  }
}
