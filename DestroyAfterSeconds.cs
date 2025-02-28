// Decompiled with JetBrains decompiler
// Type: DestroyAfterSeconds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DestroyAfterSeconds : MonoBehaviour
{
  public void DestroyAfter(float time)
  {
    this.GetComponent<Animation>().Play("SmokeDisappear");
    this.Invoke("SelfDestroy", time);
  }

  public void SelfDestroy() => Object.Destroy((Object) this.gameObject);
}
