// Decompiled with JetBrains decompiler
// Type: EmitterController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EmitterController : MonoBehaviour
{
  [W_Header("Liquidum Emitter Controller Configuration", "With this button you can turn On/Off Drain and Cascade", 14, "GreenColor")]
  public string InspectorSpace1;
  public KeyCode OnKey;
  public KeyCode OffKey;
  public GameObject[] Emitters;

  private void Start() => this.GetComponent<Renderer>().material.color = Color.green;

  private void OnTriggerStay(Collider other)
  {
    if (!(bool) (Object) other.gameObject.GetComponentInChildren<Liquidum>())
      return;
    if (Input.GetKeyDown(this.OnKey))
    {
      for (int index = 0; index < this.Emitters.Length; ++index)
      {
        if ((bool) (Object) this.Emitters[index])
        {
          LiquidumDrainEmitter component1 = this.Emitters[index].transform.GetComponent<LiquidumDrainEmitter>();
          Liquidum_Cascade component2 = this.Emitters[index].transform.GetComponent<Liquidum_Cascade>();
          if ((bool) (Object) component1 && component1.ThisIsActive)
            component1.ThisIsActive = false;
          if ((bool) (Object) component2 && component2.ThisIsActive)
            component2.ThisIsActive = false;
          this.GetComponent<Renderer>().material.color = Color.black;
        }
      }
    }
    if (!Input.GetKeyDown(this.OffKey))
      return;
    for (int index = 0; index < this.Emitters.Length; ++index)
    {
      if ((bool) (Object) this.Emitters[index])
      {
        LiquidumDrainEmitter component3 = this.Emitters[index].transform.GetComponent<LiquidumDrainEmitter>();
        Liquidum_Cascade component4 = this.Emitters[index].transform.GetComponent<Liquidum_Cascade>();
        if ((bool) (Object) component3 && !component3.ThisIsActive)
          component3.ThisIsActive = true;
        if ((bool) (Object) component4 && !component4.ThisIsActive)
          component4.ThisIsActive = true;
      }
      this.GetComponent<Renderer>().material.color = Color.green;
    }
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.blue;
    for (int index = 0; index < this.Emitters.Length; ++index)
    {
      if ((bool) (Object) this.Emitters[index])
        Gizmos.DrawLine(this.transform.position, this.Emitters[index].transform.position);
    }
  }

  private void Update()
  {
  }
}
