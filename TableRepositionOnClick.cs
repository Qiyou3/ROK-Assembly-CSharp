// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.TableRepositionOnClick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  public class TableRepositionOnClick : MonoBehaviour
  {
    public UITable Table;

    private void OnClick()
    {
      if (!((Object) this.Table != (Object) null))
        return;
      this.Table.repositionNow = true;
    }
  }
}
