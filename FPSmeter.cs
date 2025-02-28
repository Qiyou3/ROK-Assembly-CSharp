// Decompiled with JetBrains decompiler
// Type: FPSmeter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FPSmeter : MonoBehaviour
{
  public float updateInterval = 0.5f;
  private float lastInterval;
  private int frames;
  public static float fps;
  public bool showFPS;

  private void Start()
  {
    this.lastInterval = Time.realtimeSinceStartup;
    this.frames = 0;
  }

  private void OnGUI()
  {
    if (!this.showFPS)
      return;
    GUI.Label(new Rect(10f, 10f, 100f, 20f), string.Empty + (object) (float) ((double) Mathf.Round(FPSmeter.fps * 100f) / 100.0));
  }

  private void Update()
  {
    ++this.frames;
    float realtimeSinceStartup = Time.realtimeSinceStartup;
    if ((double) realtimeSinceStartup <= (double) this.lastInterval + (double) this.updateInterval)
      return;
    FPSmeter.fps = (float) this.frames / (realtimeSinceStartup - this.lastInterval);
    this.frames = 0;
    this.lastInterval = realtimeSinceStartup;
  }
}
