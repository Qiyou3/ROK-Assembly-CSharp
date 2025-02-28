// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.NotifyOnVisibility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class NotifyOnVisibility : MonoBehaviour
  {
    public Action becameVisible;
    public Action becameInvisible;

    public void OnBecameVisible()
    {
      if (this.becameVisible == null)
        return;
      this.becameVisible();
    }

    public void OnBecameInvisible()
    {
      if (this.becameInvisible == null)
        return;
      this.becameInvisible();
    }
  }
}
