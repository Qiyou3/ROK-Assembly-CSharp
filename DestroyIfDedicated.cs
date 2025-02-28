// Decompiled with JetBrains decompiler
// Type: DestroyIfDedicated
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Networking;
using UnityEngine;

#nullable disable
public class DestroyIfDedicated : MonoBehaviour
{
  public Object[] destroyIfDedicated;
  public Object[] destroyIfNonDedicated;
  public bool enableIfDedicated = true;
  public bool enableIfNonDedicated;

  public void Awake()
  {
    if (Player.IsLocalDedi)
    {
      foreach (Object @object in this.destroyIfDedicated)
        Object.DestroyImmediate(@object);
      if (!this.enableIfDedicated)
        return;
      foreach (Object @object in this.destroyIfNonDedicated)
        @object.SetEnable(true);
    }
    else
    {
      foreach (Object @object in this.destroyIfNonDedicated)
        Object.DestroyImmediate(@object);
      if (!this.enableIfNonDedicated)
        return;
      foreach (Object @object in this.destroyIfDedicated)
        @object.SetEnable(true);
    }
  }
}
