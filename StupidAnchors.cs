// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.StupidAnchors
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  [RequireComponent(typeof (UIWidget))]
  public class StupidAnchors : MonoBehaviour
  {
    public int LeftPosition;
    public int RightPosition;
    public int BottomPosition;
    public int TopPosition;
    public Vector3 Position = Vector3.zero;

    public void OnEnable() => this.StartCoroutine(this.LateEnable());

    public void OnDisable() => this.StopAllCoroutines();

    [DebuggerHidden]
    public IEnumerator LateEnable()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new StupidAnchors.\u003CLateEnable\u003Ec__Iterator1E2()
      {
        \u003C\u003Ef__this = this
      };
    }

    public void OnValidate()
    {
      UIWidget component = this.GetComponent<UIWidget>();
      this.LeftPosition = component.leftAnchor.absolute;
      this.RightPosition = component.rightAnchor.absolute;
      this.BottomPosition = component.bottomAnchor.absolute;
      this.TopPosition = component.topAnchor.absolute;
      this.Position = this.transform.localPosition;
    }
  }
}
