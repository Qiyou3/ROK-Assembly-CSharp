// Decompiled with JetBrains decompiler
// Type: DisableObjects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DisableObjects : MonoBehaviour
{
  public GameObject theObject;
  private Renderer[] renders;

  public void Start()
  {
    Component[] componentsInChildren = this.theObject.transform.GetComponentsInChildren(typeof (Renderer));
    this.renders = new Renderer[componentsInChildren.Length];
    for (int index = 0; index < componentsInChildren.Length; ++index)
      this.renders[index] = componentsInChildren[index] as Renderer;
    if (this.renders != null)
      return;
    this.renders = new Renderer[0];
  }

  public void OnTriggerEnter()
  {
    foreach (Renderer render in this.renders)
      render.enabled = false;
  }

  public void OnTriggerExit()
  {
    foreach (Renderer render in this.renders)
      render.enabled = true;
  }
}
