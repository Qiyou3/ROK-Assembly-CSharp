// Decompiled with JetBrains decompiler
// Type: EdgeDetectEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Image Effects/Edge Detection (Color)")]
[ExecuteInEditMode]
public class EdgeDetectEffect : ImageEffectBase
{
  public float threshold = 0.2f;

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.material.SetFloat("_Treshold", this.threshold * this.threshold);
    Graphics.Blit((Texture) source, destination, this.material);
  }
}
