// Decompiled with JetBrains decompiler
// Type: GlobalFog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("Image Effects/Global Fog")]
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
public class GlobalFog : PostEffectsBase
{
  public GlobalFog.FogMode fogMode;
  private float CAMERA_NEAR = 0.5f;
  private float CAMERA_FAR = 50f;
  private float CAMERA_FOV = 60f;
  private float CAMERA_ASPECT_RATIO = 1.333333f;
  public float startDistance = 200f;
  public float globalDensity = 1f;
  public float heightScale = 100f;
  public float height;
  public Color globalFogColor = Color.grey;
  public Shader fogShader;
  private Material fogMaterial;

  public void OnDisable()
  {
    if (!(bool) (UnityEngine.Object) this.fogMaterial)
      return;
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.fogMaterial);
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.fogMaterial = this.CheckShaderAndCreateMaterial(this.fogShader, this.fogMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  [ImageEffectOpaque]
  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      this.CAMERA_NEAR = this.GetComponent<Camera>().nearClipPlane;
      this.CAMERA_FAR = this.GetComponent<Camera>().farClipPlane;
      this.CAMERA_FOV = this.GetComponent<Camera>().fieldOfView;
      this.CAMERA_ASPECT_RATIO = this.GetComponent<Camera>().aspect;
      Matrix4x4 identity = Matrix4x4.identity;
      float num1 = this.CAMERA_FOV * 0.5f;
      Vector3 vector3_1 = this.GetComponent<Camera>().transform.right * this.CAMERA_NEAR * Mathf.Tan(num1 * ((float) Math.PI / 180f)) * this.CAMERA_ASPECT_RATIO;
      Vector3 vector3_2 = this.GetComponent<Camera>().transform.up * this.CAMERA_NEAR * Mathf.Tan(num1 * ((float) Math.PI / 180f));
      Vector3 vector3_3 = this.GetComponent<Camera>().transform.forward * this.CAMERA_NEAR - vector3_1 + vector3_2;
      float num2 = vector3_3.magnitude * this.CAMERA_FAR / this.CAMERA_NEAR;
      vector3_3.Normalize();
      Vector3 v1 = vector3_3 * num2;
      Vector3 vector3_4 = this.GetComponent<Camera>().transform.forward * this.CAMERA_NEAR + vector3_1 + vector3_2;
      vector3_4.Normalize();
      Vector3 v2 = vector3_4 * num2;
      Vector3 vector3_5 = this.GetComponent<Camera>().transform.forward * this.CAMERA_NEAR + vector3_1 - vector3_2;
      vector3_5.Normalize();
      Vector3 v3 = vector3_5 * num2;
      Vector3 vector3_6 = this.GetComponent<Camera>().transform.forward * this.CAMERA_NEAR - vector3_1 - vector3_2;
      vector3_6.Normalize();
      Vector3 v4 = vector3_6 * num2;
      identity.SetRow(0, (Vector4) v1);
      identity.SetRow(1, (Vector4) v2);
      identity.SetRow(2, (Vector4) v3);
      identity.SetRow(3, (Vector4) v4);
      this.fogMaterial.SetMatrix("_FrustumCornersWS", identity);
      this.fogMaterial.SetVector("_CameraWS", (Vector4) this.GetComponent<Camera>().transform.position);
      this.fogMaterial.SetVector("_StartDistance", new Vector4(1f / this.startDistance, num2 - this.startDistance));
      this.fogMaterial.SetVector("_Y", new Vector4(this.height, 1f / this.heightScale));
      this.fogMaterial.SetFloat("_GlobalDensity", this.globalDensity * 0.01f);
      this.fogMaterial.SetColor("_FogColor", this.globalFogColor);
      Graphics.Blit((Texture) source, destination, this.fogMaterial, (int) this.fogMode);
    }
  }

  public static void CustomGraphicsBlit(
    RenderTexture source,
    RenderTexture dest,
    Material fxMaterial,
    int passNr)
  {
    RenderTexture.active = dest;
    fxMaterial.SetTexture("_MainTex", (Texture) source);
    GL.PushMatrix();
    GL.LoadOrtho();
    fxMaterial.SetPass(passNr);
    GL.Begin(7);
    GL.MultiTexCoord2(0, 0.0f, 0.0f);
    GL.Vertex3(0.0f, 0.0f, 3f);
    GL.MultiTexCoord2(0, 1f, 0.0f);
    GL.Vertex3(1f, 0.0f, 2f);
    GL.MultiTexCoord2(0, 1f, 1f);
    GL.Vertex3(1f, 1f, 1f);
    GL.MultiTexCoord2(0, 0.0f, 1f);
    GL.Vertex3(0.0f, 1f, 0.0f);
    GL.End();
    GL.PopMatrix();
  }

  public enum FogMode
  {
    AbsoluteYAndDistance,
    AbsoluteY,
    Distance,
    RelativeYAndDistance,
  }
}
