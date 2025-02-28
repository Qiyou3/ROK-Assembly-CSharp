// Decompiled with JetBrains decompiler
// Type: CreatureRandomScaler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CreatureRandomScaler : MonoBehaviour
{
  public float minScale;
  public float maxScale;
  public Transform transformToScale;

  public void Start()
  {
    float num = Random.Range(this.minScale, this.maxScale);
    this.transformToScale.localScale = new Vector3(num, num, num);
  }
}
