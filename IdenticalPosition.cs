// Decompiled with JetBrains decompiler
// Type: IdenticalPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class IdenticalPosition : MonoBehaviour
{
  public Transform Target;

  public void Start()
  {
    if ((Object) this.Target == (Object) null && (Object) Camera.main != (Object) null)
      this.Target = Camera.main.transform;
    if (!((Object) this.Target == (Object) null))
      return;
    this.gameObject.SetActive(false);
  }

  public void Update() => this.transform.position = this.Target.position;

  public void LateUpdate() => this.transform.position = this.Target.position;

  public void OnBecameInvisible() => this.transform.position = this.Target.position;

  public void OnWillRenderObject() => this.transform.position = this.Target.position;
}
