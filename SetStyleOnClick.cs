// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.SetStyleOnClick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.UserInterface.Menus.Buttons;
using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  public class SetStyleOnClick : MonoBehaviour
  {
    public StyledMenuButton Button;
    public string DefaultStyle;
    public string ClickedStyle;
    public bool CanToggle;
    public SetStyleOnClickGroup Observer;

    public void OnClick()
    {
      if ((Object) this.Button != (Object) null)
      {
        if (this.CanToggle)
        {
          if (this.Button.Style == this.ClickedStyle)
            this.Button.SetStyle(this.DefaultStyle);
          else
            this.Button.SetStyle(this.ClickedStyle);
        }
        else
          this.Button.SetStyle(this.ClickedStyle);
      }
      if (!((Object) this.Observer != (Object) null))
        return;
      this.Observer.OnButtonClick(this);
    }
  }
}
