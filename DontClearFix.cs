// Decompiled with JetBrains decompiler
// Type: DontClearFix
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DontClearFix : MonoBehaviour
{
  public Camera camera1;
  public Camera camera2;
  private int cullingMask;
  private bool frame;

  public void Start() => this.cullingMask = this.camera1.cullingMask;

  public void LateUpdate()
  {
    this.frame = !this.frame;
    if (this.frame)
    {
      this.camera1.depth = -100f;
      this.camera2.depth = 100f;
      this.camera1.clearFlags = CameraClearFlags.Depth;
      this.camera1.cullingMask = 0;
      this.camera2.clearFlags = CameraClearFlags.Nothing;
      this.camera2.cullingMask = this.cullingMask;
    }
    else
    {
      this.camera2.depth = -100f;
      this.camera1.depth = 100f;
      this.camera2.clearFlags = CameraClearFlags.Depth;
      this.camera2.cullingMask = 0;
      this.camera1.clearFlags = CameraClearFlags.Nothing;
      this.camera1.cullingMask = this.cullingMask;
    }
  }
}
