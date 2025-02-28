// Decompiled with JetBrains decompiler
// Type: ActRotatorExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ActRotatorExample : MonoBehaviour
{
  [Range(1f, 100f)]
  public float speed = 5f;

  private void Update() => this.transform.Rotate(0.0f, this.speed * Time.deltaTime, 0.0f);
}
