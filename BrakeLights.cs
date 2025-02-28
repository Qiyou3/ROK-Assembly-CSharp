// Decompiled with JetBrains decompiler
// Type: BrakeLights
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BrakeLights : MonoBehaviour
{
  [HideInInspector]
  public CarController carController;
  public Material brakeLights;
  private float startValue;
  private float intensityValue;

  public void Awake()
  {
    this.carController = this.transform.GetComponent<CarController>();
    if (!(bool) (Object) this.brakeLights)
      return;
    this.startValue = this.brakeLights.GetFloat("_Intensity");
  }

  public void FixedUpdate()
  {
    if (!(bool) (Object) this.brakeLights)
      return;
    if (this.carController.brakeKey)
    {
      if ((double) this.intensityValue >= (double) this.startValue + 1.0)
        return;
      this.intensityValue += Time.deltaTime / 0.1f;
      this.brakeLights.SetFloat("_Intensity", this.intensityValue);
    }
    else
    {
      if ((double) this.intensityValue <= (double) this.startValue)
        return;
      this.intensityValue -= Time.deltaTime / 0.1f;
      this.brakeLights.SetFloat("_Intensity", this.intensityValue);
    }
  }
}
