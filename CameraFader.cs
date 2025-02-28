// Decompiled with JetBrains decompiler
// Type: CameraFader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public class CameraFader : MonoBehaviour
{
  public Texture2D fadeOutTexture;
  [Range(0.1f, 5f)]
  public float fadeTime = 2.5f;
  private int drawDepth = -1000;
  private float alpha = 1f;
  private float fadeDir = -1f;

  private void Start()
  {
    this.alpha = 1f;
    this.fadeIn();
  }

  private void OnGUI()
  {
    this.alpha += this.fadeDir / this.fadeTime * Time.deltaTime;
    this.alpha = Mathf.Clamp01(this.alpha);
    GUI.color = GUI.color with { a = this.alpha };
    GUI.depth = this.drawDepth;
    GUI.DrawTexture(new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height), (Texture) this.fadeOutTexture);
  }

  public void fadeIn() => this.fadeDir = -1f;

  public void fadeOut() => this.fadeDir = 1f;

  public void fadeOutAndLoadScene(string levelName)
  {
    this.fadeDir = 1f;
    this.StartCoroutine(this.WaitAndLoadLevel(levelName));
  }

  [DebuggerHidden]
  private IEnumerator WaitAndLoadLevel(string levelName)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new CameraFader.\u003CWaitAndLoadLevel\u003Ec__Iterator1F2()
    {
      levelName = levelName,
      \u003C\u0024\u003ElevelName = levelName,
      \u003C\u003Ef__this = this
    };
  }
}
