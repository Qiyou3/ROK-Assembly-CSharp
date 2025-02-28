// Decompiled with JetBrains decompiler
// Type: Crosshair
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class Crosshair : EntityBehaviour
{
  public Transform Sprite;
  public Transform Pivot;
  private Renderer _renderer;
  private Material _material;
  public Vector3 Position;
  public Quaternion Rotation;
  public float Scale;

  public Renderer Renderer
  {
    get
    {
      if ((Object) this._renderer == (Object) null)
      {
        if ((Object) this.Sprite == (Object) null)
          return (Renderer) null;
        this._renderer = this.Sprite.GetComponent<Renderer>();
      }
      return this._renderer;
    }
  }

  public Material Material
  {
    get
    {
      if ((Object) this._material == (Object) null)
      {
        Renderer renderer = this.Renderer;
        if ((Object) renderer == (Object) null)
          return (Material) null;
        this._material = renderer.material;
      }
      return this._material;
    }
  }

  public void Enable()
  {
    if ((Object) this.Renderer == (Object) null)
      return;
    this.Renderer.enabled = true;
    this.PrepareObjectForCamera(Camera.main);
  }

  public void Disable()
  {
    if ((Object) this.Renderer == (Object) null)
      return;
    this.Renderer.enabled = false;
  }

  public void LateUpdate() => this.PrepareObjectForCamera(Camera.main);

  public void OnWillRenderObject() => this.PrepareObjectForCamera(Camera.current);

  private void PrepareObjectForCamera(Camera cam)
  {
    if ((Object) cam == (Object) null)
      return;
    Transform transform = cam.transform;
    this.Pivot.position = transform.position;
    this.Pivot.rotation = Quaternion.LookRotation(this.Position - transform.position);
    this.Sprite.rotation = this.Rotation;
    this.Sprite.localPosition = new Vector3(0.0f, 0.0f, 1f);
    this.Sprite.localScale = new Vector3(this.Scale, this.Scale, this.Scale);
  }
}
