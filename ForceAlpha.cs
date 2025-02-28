// Decompiled with JetBrains decompiler
// Type: ForceAlpha
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ForceAlpha : MonoBehaviour
{
  private float alpha = 1f;
  private Material mat;

  public void Start()
  {
    this.mat = new Material("Shader \"Hidden/Clear Alpha\" {Properties { _Alpha(\"Alpha\", Float)=1.0 } SubShader {    Pass {        ZTest Always Cull Off ZWrite Off        ColorMask A        SetTexture [_Dummy] {            constantColor(0,0,0,[_Alpha]) combine constant }    }}}");
  }

  public void OnPostRender()
  {
    GL.PushMatrix();
    GL.LoadOrtho();
    this.mat.SetFloat("_Alpha", this.alpha);
    this.mat.SetPass(0);
    GL.Begin(7);
    GL.Vertex3(0.0f, 0.0f, 0.1f);
    GL.Vertex3(1f, 0.0f, 0.1f);
    GL.Vertex3(1f, 1f, 0.1f);
    GL.Vertex3(0.0f, 1f, 0.1f);
    GL.End();
    GL.PopMatrix();
  }
}
