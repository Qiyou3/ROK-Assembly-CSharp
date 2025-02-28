// Decompiled with JetBrains decompiler
// Type: DistortionTriggerArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DistortionTriggerArea : MonoBehaviour
{
  [W_Header("Liquidum Distortion Area", "When the Main Camera enter on this area, activate the \"Distortion Trigger Effect\" ", 14, "GreenColor")]
  public string InspectorSpace1;
  private Liquidum LiquidumScript;
  private LiquidumDrainEmitter DrainParent;
  private Liquidum_Cascade CascadeParent;
  private bool OnEnterEnabled = true;
  [W_ToolTip("If true, this trigger area is enabled only when the parent (Cascade or Drain) is active.")]
  public bool IsParentDependent;

  private void Start()
  {
    this.LiquidumScript = Liquidum.LiquidumScript;
    if ((bool) (Object) this.transform.parent.GetComponent<LiquidumDrainEmitter>())
      this.DrainParent = this.transform.parent.GetComponent<LiquidumDrainEmitter>();
    if (!(bool) (Object) this.transform.parent.GetComponent<Liquidum_Cascade>())
      return;
    this.CascadeParent = this.transform.parent.GetComponent<Liquidum_Cascade>();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (!(bool) (Object) other.gameObject.GetComponent<Liquidum>() || !this.OnEnterEnabled)
      return;
    this.LiquidumScript.FadeInTriggerDistortionNow = true;
  }

  private void OnTriggerExit(Collider other)
  {
    if (!(bool) (Object) other.gameObject.GetComponentInChildren<Liquidum>())
      return;
    this.LiquidumScript.FadeOutTriggerDistortionNow = true;
  }

  private void Update()
  {
    if (!this.IsParentDependent)
      return;
    if ((bool) (Object) this.DrainParent)
      this.OnEnterEnabled = this.DrainParent.ThisIsActive;
    if (!(bool) (Object) this.CascadeParent)
      return;
    this.OnEnterEnabled = this.CascadeParent.ThisIsActive;
  }
}
