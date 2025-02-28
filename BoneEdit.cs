// Decompiled with JetBrains decompiler
// Type: BoneEdit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class BoneEdit : MonoBehaviour
{
  private Transform _transform;
  private Transform _parent;
  public Color Color;

  public void Awake()
  {
    this._transform = this.transform;
    this._parent = this._transform.parent;
  }

  public void OnDrawGizmos()
  {
    if ((Object) this._parent == (Object) null)
      return;
    Gizmos.color = this.Color;
    Gizmos.DrawLine(this._transform.position, this._parent.position);
  }
}
