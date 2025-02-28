// Decompiled with JetBrains decompiler
// Type: AnchorObjectToScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AnchorObjectToScreen : MonoBehaviour
{
  public Camera camera;
  public Vector3 position;
  public Quaternion rotation;
  public Vector3 scale;

  public void Awake()
  {
    if (!((Object) this.camera == (Object) null))
      return;
    this.camera = Camera.main;
  }

  public void Update()
  {
    this.transform.position = this.camera.ViewportToWorldPoint(this.position);
    this.transform.localRotation = this.rotation;
    this.transform.localScale = this.scale;
  }
}
