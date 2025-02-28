// Decompiled with JetBrains decompiler
// Type: CameraUIObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CameraUIObject : MonoBehaviour
{
  public Transform pivot;

  public void Start()
  {
    this.GetComponent<Renderer>().GetComponent<MeshFilter>().mesh.bounds = new Bounds(Vector3.zero, new Vector3(10f, 10f, 10f));
  }

  public void LateUpdate()
  {
    if (!(bool) (Object) Camera.main)
      return;
    this.pivot.position = Camera.main.transform.position;
  }

  public void OnPreCull() => this.pivot.position = Camera.current.transform.position;

  public void OnPreRender() => this.pivot.position = Camera.current.transform.position;

  public void OnWillRenderObject() => this.pivot.position = Camera.current.transform.position;
}
