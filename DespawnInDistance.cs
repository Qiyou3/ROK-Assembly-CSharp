// Decompiled with JetBrains decompiler
// Type: DespawnInDistance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public class DespawnInDistance : MonoBehaviour
{
  public float distance = 1000f;
  public float pollTime = 1f;

  public void Start() => this.StartCoroutine(this.CheckDistance());

  [DebuggerHidden]
  private IEnumerator CheckDistance()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new DespawnInDistance.\u003CCheckDistance\u003Ec__IteratorFD()
    {
      \u003C\u003Ef__this = this
    };
  }
}
