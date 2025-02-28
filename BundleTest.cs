// Decompiled with JetBrains decompiler
// Type: BundleTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Renderer))]
public class BundleTest : MonoBehaviour
{
  public Bundle bundle;
  public Bundle gameObjBundle;

  [DebuggerHidden]
  public IEnumerator Start()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new BundleTest.\u003CStart\u003Ec__Iterator56()
    {
      \u003C\u003Ef__this = this
    };
  }
}
