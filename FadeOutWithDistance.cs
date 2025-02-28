// Decompiled with JetBrains decompiler
// Type: FadeOutWithDistance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FadeOutWithDistance : MonoBehaviour
{
  public Transform m_camera;
  public float distanceBeginFade = 1000f;
  public float distanceEndFade = 2000f;
  private Shader originalShader;
  public string transparentShaderName;
  private float originalAlpha;
  public bool yDistanceOnly;
  private Material _material;

  public Transform camera
  {
    get
    {
      if ((Object) this.m_camera != (Object) null)
      {
        if ((Object) this.m_camera.GetComponent<Camera>() == (Object) null)
          this.m_camera = (Transform) null;
        else if (!this.m_camera.GetComponent<Camera>().enabled || !this.m_camera.gameObject.activeSelf)
          this.m_camera = (Transform) null;
      }
      if ((Object) this.m_camera == (Object) null)
      {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("MainCamera"))
        {
          if (gameObject.activeSelf)
          {
            Camera component = gameObject.GetComponent<Camera>();
            if (!((Object) component == (Object) null) && component.enabled)
            {
              this.m_camera = component.transform;
              break;
            }
          }
        }
      }
      if ((Object) this.m_camera == (Object) null)
        this.LogError<FadeOutWithDistance>("m_camera == null");
      return this.m_camera;
    }
    set => this.m_camera = value;
  }

  public void Start()
  {
    this._material = this.GetComponent<Renderer>().material;
    this.originalShader = this._material.shader;
    this.originalAlpha = this._material.color.a;
  }

  public void OnDestroy()
  {
    if (!((Object) this._material != (Object) null))
      return;
    Object.Destroy((Object) this._material);
    this._material = (Material) null;
  }

  public void Update()
  {
    float num = !this.yDistanceOnly ? Vector3.Distance(this.transform.position, this.camera.position) : Mathf.Abs(this.transform.position.y - this.camera.position.y);
    if ((double) num < (double) this.distanceBeginFade)
    {
      this._material.shader = this.originalShader;
      this._material.color = this._material.color with
      {
        a = this.originalAlpha
      };
    }
    else
    {
      this._material.shader = Shader.Find(this.transparentShaderName);
      this._material.color = this._material.color with
      {
        a = this.originalAlpha * Mathf.InverseLerp(this.distanceBeginFade, this.distanceEndFade, num)
      };
    }
    if ((double) num > (double) this.distanceEndFade)
      this.GetComponent<Renderer>().enabled = false;
    else
      this.GetComponent<Renderer>().enabled = true;
  }
}
