// Decompiled with JetBrains decompiler
// Type: ThirdParty.Extensions.NGUI.StupidWidgets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Core;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace ThirdParty.Extensions.NGUI
{
  public class StupidWidgets : MonoBehaviour
  {
    public UIWidget[] Widgets = new UIWidget[0];
    private static WaitForEndOfFrame _wait = new WaitForEndOfFrame();
    private static List<StupidWidgets> _instances = new List<StupidWidgets>();

    public void OnEnable()
    {
      if (StupidWidgets._instances.Count == 0)
        Coroutiner.StartStaticCoroutine(this.LateEnable());
      if (StupidWidgets._instances.Contains(this))
        return;
      StupidWidgets._instances.Add(this);
    }

    [DebuggerHidden]
    public IEnumerator LateEnable()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      StupidWidgets.\u003CLateEnable\u003Ec__Iterator1E4 enableCIterator1E4 = new StupidWidgets.\u003CLateEnable\u003Ec__Iterator1E4();
      return (IEnumerator) enableCIterator1E4;
    }

    private void Refresh()
    {
      for (int index = 0; index < this.Widgets.Length; ++index)
      {
        if ((Object) this.Widgets[index] != (Object) null)
        {
          this.Widgets[index].RemoveFromPanel();
          this.Widgets[index].CreatePanel();
        }
      }
    }
  }
}
