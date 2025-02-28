// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.TitleCaseCaps
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  public class TitleCaseCaps : MonoBehaviour
  {
    public Transform Center;
    public UILabel UpperCase;
    public UILabel LowerCase;

    [ContextMenu("Execute")]
    public void OnEnable()
    {
      if (!((Object) this.Center != (Object) null) || !((Object) this.UpperCase != (Object) null) || !((Object) this.LowerCase != (Object) null))
        return;
      this.LowerCase.cachedTransform.position = new Vector3(this.Center.position.x, this.LowerCase.cachedTransform.position.y, this.LowerCase.cachedTransform.position.z);
      this.LowerCase.cachedTransform.localPosition += new Vector3((float) (((double) this.UpperCase.width * 0.77499997615814209 - (double) this.LowerCase.width * 0.77499997615814209) / 2.0), 0.0f, 0.0f);
    }
  }
}
