// Decompiled with JetBrains decompiler
// Type: Steamworks.CallbackIdentityAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
  internal class CallbackIdentityAttribute : Attribute
  {
    public CallbackIdentityAttribute(int callbackNum) => this.Identity = callbackNum;

    public int Identity { get; set; }
  }
}
