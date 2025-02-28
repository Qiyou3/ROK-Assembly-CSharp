// Decompiled with JetBrains decompiler
// Type: FogConeScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FogConeScript : MonoBehaviour
{
  [HideInInspector]
  public Color StartColor;
  [HideInInspector]
  public bool fadeI;
  [HideInInspector]
  public bool fadeO;
  [HideInInspector]
  private Color TargetColor;
  private Liquidum LiquidumScript;

  private void Start()
  {
    this.LiquidumScript = Liquidum.LiquidumScript;
    this.GetComponent<Renderer>().material.SetColor("_Color", this.LiquidumScript.RainFogColor);
  }

  private void FadeIN()
  {
    this.fadeO = false;
    this.TargetColor = Color.Lerp(this.GetComponent<Renderer>().material.GetColor("_Color"), this.StartColor, 2f * Time.deltaTime);
    if ((double) this.TargetColor.a < (double) this.StartColor.a - 0.0099999997764825821)
      return;
    this.fadeI = false;
  }

  private void FadeOUT()
  {
    this.fadeI = false;
    this.TargetColor = Color.Lerp(this.GetComponent<Renderer>().material.GetColor("_Color"), new Color(0.0f, 0.0f, 0.0f, 0.0f), 2f * Time.deltaTime);
    if ((double) this.TargetColor.a >= 0.0099999997764825821)
      return;
    this.fadeO = false;
  }

  private void Update()
  {
    if (this.fadeI)
      this.FadeIN();
    if (this.fadeO)
      this.FadeOUT();
    if (this.fadeI || this.fadeO)
      this.GetComponent<Renderer>().material.SetColor("_Color", this.TargetColor);
    else if (!this.LiquidumScript.UnderOcclusion && this.LiquidumScript.RainEmit)
      this.GetComponent<Renderer>().material.SetColor("_Color", this.LiquidumScript.RainFogColor);
    if (this.LiquidumScript.RainEmit)
      return;
    this.fadeI = false;
    this.fadeO = false;
    this.GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(this.GetComponent<Renderer>().material.GetColor("_Color"), new Color(0.0f, 0.0f, 0.0f, 0.0f), 3f * Time.deltaTime));
  }
}
