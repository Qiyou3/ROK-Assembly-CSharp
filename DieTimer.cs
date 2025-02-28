// Decompiled with JetBrains decompiler
// Type: DieTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DieTimer : MonoBehaviour
{
  public float SecondsToDie = 10f;
  private float m_fTimer;

  private void Start() => this.m_fTimer = 0.0f;

  private void Update()
  {
    this.m_fTimer += Time.deltaTime;
    if ((double) this.m_fTimer <= (double) this.SecondsToDie)
      return;
    Object.Destroy((Object) this.gameObject);
  }
}
