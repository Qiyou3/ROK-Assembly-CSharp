// Decompiled with JetBrains decompiler
// Type: ImageEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("")]
public class ImageEffects
{
  public static void RenderDistortion(
    Material material,
    RenderTexture source,
    RenderTexture destination,
    float angle,
    Vector2 center,
    Vector2 radius)
  {
    if ((double) source.texelSize.y < 0.0)
    {
      center.y = 1f - center.y;
      angle = -angle;
    }
    Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, angle), Vector3.one);
    material.SetMatrix("_RotationMatrix", matrix);
    material.SetVector("_CenterRadius", new Vector4(center.x, center.y, radius.x, radius.y));
    material.SetFloat("_Angle", angle * ((float) Math.PI / 180f));
    Graphics.Blit((Texture) source, destination, material);
  }

  [Obsolete("Use Graphics.Blit(source,dest) instead")]
  public static void Blit(RenderTexture source, RenderTexture dest)
  {
    Graphics.Blit((Texture) source, dest);
  }

  [Obsolete("Use Graphics.Blit(source, destination, material) instead")]
  public static void BlitWithMaterial(Material material, RenderTexture source, RenderTexture dest)
  {
    Graphics.Blit((Texture) source, dest, material);
  }
}
