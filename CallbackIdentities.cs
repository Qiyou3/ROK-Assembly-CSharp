// Decompiled with JetBrains decompiler
// Type: Steamworks.CallbackIdentities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  internal class CallbackIdentities
  {
    public static int GetCallbackIdentity(Type callbackStruct)
    {
      object[] customAttributes = callbackStruct.GetCustomAttributes(typeof (CallbackIdentityAttribute), false);
      int index = 0;
      if (index < customAttributes.Length)
        return ((CallbackIdentityAttribute) customAttributes[index]).Identity;
      throw new Exception("Callback number not found for struct " + (object) callbackStruct);
    }
  }
}
