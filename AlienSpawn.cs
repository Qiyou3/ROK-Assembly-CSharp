// Decompiled with JetBrains decompiler
// Type: AlienSpawn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AlienSpawn : MonoBehaviour
{
  private float timer;
  private bool dropping;
  public bool fullyDeveloped;

  public void Update()
  {
    this.timer += Time.deltaTime;
    if (this.dropping)
    {
      this.transform.position = this.transform.position + Vector3.down * this.timer * Time.deltaTime * 9.81f;
      if ((double) this.timer <= 1.0)
        return;
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      this.transform.localScale = new Vector3(1f, 1f, 1f) * Mathf.Min(this.timer, 5f) / 5f * 0.798f;
      if ((double) this.timer <= 5.0)
        return;
      this.fullyDeveloped = true;
    }
  }

  public void Drop()
  {
    this.dropping = true;
    this.timer = 0.0f;
  }
}
