// Decompiled with JetBrains decompiler
// Type: DestroyWithDistance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
public class DestroyWithDistance : MonoBehaviour
{
  public Transform observer;
  public float destroyDistance = 60f;
  public float checkRate = 1f;

  public Transform Observer
  {
    get
    {
      if ((Object) this.observer == (Object) null)
      {
        this.LogWarning<DestroyWithDistance>(ErrorUtil.NullRefWarning(this.gameObject.GetFullName() + ".observer"));
        this.observer = Camera.main.transform;
        if ((Object) this.observer == (Object) null)
          this.LogError<DestroyWithDistance>("{0}.observer == null", (object) this.gameObject.GetFullName());
      }
      return this.observer;
    }
  }

  public void Start()
  {
    this.InvokeRepeating("CheckDistance", Random.value * this.checkRate, this.checkRate);
    this.destroyDistance *= this.destroyDistance;
  }

  public void CheckDistance()
  {
    if ((double) (this.transform.position - this.Observer.position).sqrMagnitude <= (double) this.destroyDistance)
      return;
    Object.Destroy((Object) this.gameObject);
    this.enabled = false;
  }
}
