﻿// Decompiled with JetBrains decompiler
// Type: IdenticalRotationRunInEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class IdenticalRotationRunInEditor : MonoBehaviour
{
  public Transform target;

  public void LateUpdate() => this.transform.rotation = this.target.rotation;
}
