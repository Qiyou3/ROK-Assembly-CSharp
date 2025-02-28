// Decompiled with JetBrains decompiler
// Type: InstantiateWithDistance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
public class InstantiateWithDistance : MonoBehaviour
{
  public GameObject objectToInstantiate;
  public Transform observer;
  public float instantiateDistance = 20f;
  public float destroyDistance = 100f;
  public float checkRate = 10f;

  public Transform Observer
  {
    get
    {
      if ((Object) this.observer == (Object) null)
      {
        this.LogWarning<InstantiateWithDistance>(ErrorUtil.NullRefWarning(this.gameObject.GetFullName() + ".observer"));
        this.observer = Camera.main.transform;
        if ((Object) this.observer == (Object) null)
          this.LogError<InstantiateWithDistance>("{0}.observer == null", (object) this.gameObject.GetFullName());
      }
      return this.observer;
    }
  }

  public void Start()
  {
    this.InvokeRepeating("CheckDistance", Random.value * this.checkRate, this.checkRate);
    this.instantiateDistance *= this.instantiateDistance;
    this.destroyDistance *= this.destroyDistance;
  }

  public void CheckDistance()
  {
    float sqrMagnitude = (this.transform.position - this.Observer.position).sqrMagnitude;
    if ((double) sqrMagnitude < (double) this.instantiateDistance)
    {
      Object.Instantiate((Object) this.objectToInstantiate, this.transform.position, this.transform.rotation);
      Object.Destroy((Object) this.gameObject);
      this.enabled = false;
    }
    else
    {
      if ((double) sqrMagnitude <= (double) this.destroyDistance)
        return;
      Object.Destroy((Object) this.gameObject);
      this.enabled = false;
    }
  }
}
