// Decompiled with JetBrains decompiler
// Type: AdditionalDistortion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AdditionalDistortion : MonoBehaviour
{
  private Liquidum LiquidumScript;
  public bool fadeO;
  public bool fadeI;
  private Color TargetColor;
  private float FieldOfViewCompensation;

  private void Start()
  {
    this.LiquidumScript = Liquidum.LiquidumScript;
    this.TargetColor = this.LiquidumScript.AdditionaDistortionColor;
  }

  public void FadeOut()
  {
    this.fadeO = true;
    this.fadeI = false;
    this.TargetColor = Color.Lerp(this.GetComponent<Renderer>().material.GetColor("_Color"), new Color(0.0f, 0.0f, 0.0f, 0.0f), this.LiquidumScript.AdditionalDistortionFadeOutSpeed * Time.deltaTime);
    if ((double) this.TargetColor.a < 0.0099999997764825821)
      this.fadeO = false;
    if ((double) Time.timeSinceLevelLoad >= 4.0)
      return;
    this.Clear();
  }

  public void Clear()
  {
    this.fadeO = false;
    this.fadeI = false;
    this.GetComponent<Renderer>().material.SetColor("_Color", Color.clear);
  }

  public void FadeIn()
  {
    this.fadeI = true;
    this.fadeO = false;
    this.TargetColor = Color.Lerp(this.GetComponent<Renderer>().material.GetColor("_Color"), this.LiquidumScript.AdditionaDistortionColor, this.LiquidumScript.AdditionalDistortionFadeInSpeed * Time.deltaTime);
    if ((double) this.TargetColor.a < (double) this.LiquidumScript.AdditionaDistortionColor.a - 0.0099999997764825821)
      return;
    this.fadeI = false;
  }

  private void Update()
  {
    float num = (float) ((double) Time.time * (double) this.LiquidumScript.AdditionalDistortionSlipSpeed / 10.0);
    if (this.fadeI || this.fadeO)
      this.GetComponent<Renderer>().material.SetColor("_Color", this.TargetColor);
    else if ((double) this.TargetColor.a < 0.0099999997764825821)
      this.GetComponent<Renderer>().material.SetColor("_Color", new Color(0.0f, 0.0f, 0.0f, 0.0f));
    else
      this.GetComponent<Renderer>().material.SetColor("_Color", this.LiquidumScript.AdditionaDistortionColor);
    this.GetComponent<Renderer>().material.SetFloat("_BumpAmt", this.LiquidumScript.AdditionalDistortionStrength);
    this.GetComponent<Renderer>().materials[0].mainTextureOffset = new Vector2(0.0f, -num);
    this.GetComponent<Renderer>().materials[0].SetTextureOffset("_BumpMap", new Vector2(0.0f, -num));
    if (this.fadeO)
      this.FadeOut();
    if (this.fadeI)
      this.FadeIn();
    if (!this.LiquidumScript.Additional_RainDependence || this.LiquidumScript.UnderOcclusion)
      return;
    this.fadeI = this.LiquidumScript.RainEmit;
    this.fadeO = !this.LiquidumScript.RainEmit;
    if (this.LiquidumScript.EmissionRate >= 50)
      return;
    this.fadeO = true;
  }
}
