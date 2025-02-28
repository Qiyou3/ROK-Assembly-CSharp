// Decompiled with JetBrains decompiler
// Type: GameObjectPath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class GameObjectPath
{
  public static string GetPath(this Transform current)
  {
    return (Object) current.parent == (Object) null ? "/" + current.name : current.parent.GetPath() + "/" + current.name;
  }

  public static string GetPath(this Component component)
  {
    return component.transform.GetPath() + "/" + component.GetType().ToString();
  }
}
