// Decompiled with JetBrains decompiler
// Type: ImageEffectManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
public class ImageEffectManager : MonoBehaviour
{
  public List<ChildImageEffect> childImageEffects;

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (this.childImageEffects.Count > 0)
    {
      for (int index = 0; index < this.childImageEffects.Count; ++index)
      {
        ChildImageEffect childImageEffect = this.childImageEffects[index];
        if (childImageEffect.enabled && childImageEffect.gameObject.activeInHierarchy)
          childImageEffect.OnEffect(source);
      }
      Graphics.Blit((Texture) source, destination);
    }
    else
      Graphics.Blit((Texture) source, destination);
  }
}
