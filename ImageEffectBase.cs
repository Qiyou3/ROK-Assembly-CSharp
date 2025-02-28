// Decompiled with JetBrains decompiler
// Type: ImageEffectBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (Camera))]
[AddComponentMenu("")]
public class ImageEffectBase : MonoBehaviour
{
  public Shader shader;
  private Material m_Material;

  public virtual void Start()
  {
    if (!SystemInfo.supportsImageEffects)
    {
      this.enabled = false;
    }
    else
    {
      if ((bool) (Object) this.shader && this.shader.isSupported)
        return;
      this.enabled = false;
    }
  }

  protected Material material
  {
    get
    {
      if ((Object) this.m_Material == (Object) null)
      {
        this.m_Material = new Material(this.shader);
        this.m_Material.hideFlags = HideFlags.HideAndDontSave;
      }
      return this.m_Material;
    }
  }

  public virtual void OnDisable()
  {
    if (!(bool) (Object) this.m_Material)
      return;
    Object.DestroyImmediate((Object) this.m_Material);
  }
}
