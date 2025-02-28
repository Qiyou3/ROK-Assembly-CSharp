// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.SetStyleOnEnable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.UserInterface.Menus.Buttons;
using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  public class SetStyleOnEnable : MonoBehaviour
  {
    public StyledMenuButton Button;
    public string Style;

    public void OnEnable()
    {
      if (!((Object) this.Button != (Object) null))
        return;
      this.Button.SetStyle(this.Style);
    }
  }
}
