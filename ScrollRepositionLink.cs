// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.ScrollRepositionLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  [RequireComponent(typeof (UITable))]
  public class ScrollRepositionLink : MonoBehaviour
  {
    public bool Reset;
    public UITable Table;
    public UIScrollView Target;

    public void Awake() => this.Attach();

    public void OnDestroy() => this.Detach();

    public void OnValidate()
    {
      if ((Object) this.Table == (Object) null)
        this.Table = this.GetComponent<UITable>();
      if (!((Object) this.Target == (Object) null))
        return;
      this.Target = NGUITools.FindInParents<UIScrollView>(this.gameObject);
    }

    public void Attach()
    {
      if (!((Object) this.Table != (Object) null) || !((Object) this.Target != (Object) null))
        return;
      if (this.Reset)
        this.Table.onReposition += new UITable.OnReposition(this.ResetScroll);
      this.Table.onReposition += new UITable.OnReposition(this.Target.UpdateScrollbars);
    }

    public void Detach()
    {
      if (!((Object) this.Table != (Object) null) || !((Object) this.Target != (Object) null))
        return;
      if (this.Reset)
        this.Table.onReposition -= new UITable.OnReposition(this.ResetScroll);
      this.Table.onReposition -= new UITable.OnReposition(this.Target.UpdateScrollbars);
    }

    public void ResetScroll()
    {
      if (!((Object) this.Target != (Object) null))
        return;
      if ((Object) this.Target.verticalScrollBar != (Object) null)
        this.Target.verticalScrollBar.value = 0.0f;
      if (!((Object) this.Target.horizontalScrollBar != (Object) null))
        return;
      this.Target.horizontalScrollBar.value = 0.0f;
    }
  }
}
