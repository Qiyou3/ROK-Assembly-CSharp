// Decompiled with JetBrains decompiler
// Type: BaseContact
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BaseContact
{
  public Collider collider;
  public Vector3 lastPosition = Vector3.zero;
  public Vector3 position = Vector3.zero;
  public Vector3 normal = Vector3.zero;
  public Vector3 velocity = Vector3.zero;
  public float traction;
  public bool inContact;
}
