// Decompiled with JetBrains decompiler
// Type: ForceLocalRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ForceLocalRotation : MonoBehaviour
{
  public Quaternion LocalRotation;
  private Transform _t;

  public void Start() => this._t = this.transform;

  public void LateUpdate() => this._t.localRotation = this.LocalRotation;
}
