// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.ClickOnStart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  public class ClickOnStart : MonoBehaviour
  {
    public bool LateStart = true;
    public bool UseSelect = true;

    [DebuggerHidden]
    public IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ClickOnStart.\u003CStart\u003Ec__Iterator1E0()
      {
        \u003C\u003Ef__this = this
      };
    }
  }
}
