// Decompiled with JetBrains decompiler
// Type: HologramGUISimple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class HologramGUISimple : ImageEffectBase
{
  public Camera m_guiCamera;
  public RenderTexture guiRenderTexture;
  public Camera m_worldGuiCamera;
  public RenderTexture worldGuiRenderTexture;

  public Camera guiCamera
  {
    get
    {
      if ((Object) this.m_guiCamera == (Object) null)
        this.m_guiCamera = Tags.FindComponentWithTag<Camera>("GUI Camera");
      return this.m_guiCamera;
    }
  }

  public Camera worldGuiCamera
  {
    get
    {
      if ((Object) this.m_worldGuiCamera == (Object) null)
        this.m_worldGuiCamera = Tags.FindComponentWithTag<Camera>("World GUI Camera");
      return this.m_worldGuiCamera;
    }
  }

  public void Awake()
  {
    if ((Object) this.guiRenderTexture != (Object) null)
      Object.DestroyImmediate((Object) this.guiRenderTexture);
    if (!((Object) this.worldGuiRenderTexture != (Object) null))
      return;
    Object.DestroyImmediate((Object) this.worldGuiRenderTexture);
  }

  public void OnDestroy()
  {
    this.guiRenderTexture = !((Object) this.guiCamera != (Object) null) ? (RenderTexture) null : this.guiCamera.targetTexture;
    if ((Object) this.guiRenderTexture != (Object) null)
    {
      this.guiCamera.targetTexture = (RenderTexture) null;
      Object.DestroyImmediate((Object) this.guiRenderTexture);
    }
    this.worldGuiRenderTexture = !((Object) this.worldGuiCamera != (Object) null) ? (RenderTexture) null : this.worldGuiCamera.targetTexture;
    if (!((Object) this.worldGuiRenderTexture != (Object) null))
      return;
    this.worldGuiCamera.targetTexture = (RenderTexture) null;
    Object.DestroyImmediate((Object) this.worldGuiRenderTexture);
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((Object) this.m_guiCamera == (Object) null)
    {
      this.m_guiCamera = Tags.FindComponentWithTag<Camera>("GUI Camera");
      if ((Object) this.m_guiCamera == (Object) null)
      {
        Graphics.Blit((Texture) source, destination);
        return;
      }
    }
    this.guiRenderTexture = this.guiCamera.targetTexture;
    if (((Object) this.guiRenderTexture == (Object) null || this.guiRenderTexture.width != source.width || this.guiRenderTexture.height != source.height) && (Object) this.guiRenderTexture != (Object) RenderTexture.active)
    {
      Object.DestroyImmediate((Object) this.guiRenderTexture);
      this.guiRenderTexture = new RenderTexture(source.width, source.height, 0, source.format);
      this.guiRenderTexture.name = "GUI Render Texture (For GUI Effects)";
      this.guiCamera.targetTexture = this.guiRenderTexture;
      this.material.SetTexture("_GuiTex", (Texture) this.guiRenderTexture);
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      this.guiCamera.aspect = (float) this.guiRenderTexture.width / (float) this.guiRenderTexture.height;
      if ((Object) this.worldGuiCamera != (Object) null)
      {
        this.worldGuiRenderTexture = this.worldGuiCamera.targetTexture;
        if (((Object) this.worldGuiRenderTexture == (Object) null || this.worldGuiRenderTexture.width != source.width || this.worldGuiRenderTexture.height != source.height) && (Object) this.worldGuiRenderTexture != (Object) RenderTexture.active)
        {
          Object.DestroyImmediate((Object) this.worldGuiRenderTexture);
          this.worldGuiRenderTexture = new RenderTexture(source.width, source.height, 0, source.format);
          this.worldGuiRenderTexture.name = "World GUI Render Texture (For GUI Effects)";
          this.worldGuiCamera.targetTexture = this.worldGuiRenderTexture;
          this.worldGuiCamera.aspect = (float) this.worldGuiRenderTexture.width / (float) this.worldGuiRenderTexture.height;
          Graphics.Blit((Texture) source, destination);
          return;
        }
        this.material.SetTexture("_WorldGuiTex", (Texture) this.worldGuiRenderTexture);
        Graphics.Blit((Texture) this.guiRenderTexture, this.guiRenderTexture, this.material, 0);
      }
      Graphics.Blit((Texture) source, destination, this.material, 0);
    }
  }
}
