// Decompiled with JetBrains decompiler
// Type: Helper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public static class Helper
{
  public static GameObject FindInChildren(this GameObject go, string name)
  {
    IEnumerable<GameObject> source = ((IEnumerable<Transform>) go.GetComponentsInChildren<Transform>()).Where<Transform>((Func<Transform, bool>) (x => x.gameObject.name == name)).Select<Transform, GameObject>((Func<Transform, GameObject>) (x => x.gameObject));
    return source == null ? (GameObject) null : source.FirstOrDefault<GameObject>();
  }
}
