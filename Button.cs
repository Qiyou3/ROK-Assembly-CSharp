// Decompiled with JetBrains decompiler
// Type: Button
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public class Button : MonoBehaviour
{
  public Transform effectToPlay;
  public Vector3 effectPos;
  public float effectAngle;

  private void OnMouseDown() => this.StartCoroutine(this.PlayEffect());

  [DebuggerHidden]
  private IEnumerator PlayEffect()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new Button.\u003CPlayEffect\u003Ec__Iterator1DA()
    {
      \u003C\u003Ef__this = this
    };
  }
}
