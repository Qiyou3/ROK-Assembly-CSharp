﻿// Decompiled with JetBrains decompiler
// Type: AddRandomTorque
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AddRandomTorque : MonoBehaviour
{
  public float torqueAmount = 10f;

  public void OnEnable()
  {
    if ((Object) this.GetComponent<Rigidbody>() == (Object) null)
      return;
    this.GetComponent<Rigidbody>().AddTorque((Vector3) (Random.insideUnitCircle * this.torqueAmount));
  }
}
