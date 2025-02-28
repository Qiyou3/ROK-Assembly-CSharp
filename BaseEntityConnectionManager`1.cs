// Decompiled with JetBrains decompiler
// Type: BaseEntityConnectionManager`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using System;

#nullable disable
public abstract class BaseEntityConnectionManager<T> : BaseEntityConnectionManager where T : BaseEntityConnectionManager
{
  public static void Connect(Entity a, Entity b, bool connect)
  {
    try
    {
      if ((UnityEngine.Object) a == (UnityEngine.Object) null || (UnityEngine.Object) b == (UnityEngine.Object) null)
        Logger.ErrorFormat<BaseEntityConnectionManager>("On Entity Connect a passed entity is null A is null: {0} B is null: {1}", (object) ((UnityEngine.Object) a == (UnityEngine.Object) null), (object) ((UnityEngine.Object) b == (UnityEngine.Object) null));
      else
        BaseEntityConnectionManager.ConnectManagers((BaseEntityConnectionManager) a.GetOrCreate<T>(), (BaseEntityConnectionManager) b.GetOrCreate<T>(), connect);
    }
    catch (Exception ex)
    {
      Logger.Exception(ex);
    }
  }
}
