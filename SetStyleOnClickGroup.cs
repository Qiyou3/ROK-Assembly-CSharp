// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.SetStyleOnClickGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  public class SetStyleOnClickGroup : MonoBehaviour
  {
    private SetStyleOnClick _observed;

    public void OnButtonClick(SetStyleOnClick button)
    {
      if ((Object) this._observed != (Object) null && (Object) this._observed.Button != (Object) null && (Object) this._observed != (Object) button)
        this._observed.Button.SetStyle(this._observed.DefaultStyle);
      this._observed = button;
    }
  }
}
