// Decompiled with JetBrains decompiler
// Type: DownloadTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (UITexture))]
public class DownloadTexture : MonoBehaviour
{
  public string url = "http://www.yourwebsite.com/logo.png";
  private Texture2D mTex;

  [DebuggerHidden]
  private IEnumerator Start()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new DownloadTexture.\u003CStart\u003Ec__Iterator1E6()
    {
      \u003C\u003Ef__this = this
    };
  }

  public void OnDestroy()
  {
    if (!((Object) this.mTex != (Object) null))
      return;
    Object.Destroy((Object) this.mTex);
  }
}
