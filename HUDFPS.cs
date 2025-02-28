// Decompiled with JetBrains decompiler
// Type: HUDFPS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HUDFPS : MonoBehaviour
{
  public float updateInterval = 0.5f;
  private float accum;
  private int frames;
  private float timeleft;

  private void Start()
  {
    if (!(bool) (Object) this.GetComponent<GUIText>())
    {
      this.LogInfo<HUDFPS>("UtilityFramesPerSecond needs a GUIText component!");
      this.enabled = false;
    }
    else
      this.timeleft = this.updateInterval;
  }

  private void Update()
  {
    this.timeleft -= Time.deltaTime;
    this.accum += Time.timeScale / Time.deltaTime;
    ++this.frames;
    if ((double) this.timeleft > 0.0)
      return;
    float num = this.accum / (float) this.frames;
    string str = string.Format("{0:F2} FPS", (object) num);
    this.GetComponent<GUIText>().text = str;
    if ((double) num < 30.0)
      this.GetComponent<GUIText>().material.color = Color.yellow;
    else if ((double) num < 10.0)
      this.GetComponent<GUIText>().material.color = Color.red;
    else
      this.GetComponent<GUIText>().material.color = Color.green;
    this.timeleft = this.updateInterval;
    this.accum = 0.0f;
    this.frames = 0;
  }
}
