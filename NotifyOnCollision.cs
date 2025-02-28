// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.NotifyOnCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class NotifyOnCollision : MonoBehaviour
  {
    public Action<Collision> collisionStay;
    public Action<Collision> collisionEnter;
    public Action<Collision> collisionExit;

    public void OnCollisionStay(Collision collision)
    {
      if (this.collisionStay == null)
        return;
      this.collisionStay(collision);
    }

    public void OnCollisionEnter(Collision collision)
    {
      if (this.collisionEnter == null)
        return;
      this.collisionEnter(collision);
    }

    public void OnCollisionExit(Collision collision)
    {
      if (this.collisionExit == null)
        return;
      this.collisionExit(collision);
    }
  }
}
