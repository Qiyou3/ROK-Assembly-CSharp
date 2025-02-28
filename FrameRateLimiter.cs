// Decompiled with JetBrains decompiler
// Type: FrameRateLimiter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;
using System.Threading;
using UnityEngine;

#nullable disable
public class FrameRateLimiter : MonoBehaviour
{
  public int MinMillisecondPerFrame = 60;
  private readonly Stopwatch _stopwatch = new Stopwatch();

  public void Update()
  {
    if (this._stopwatch.IsRunning)
    {
      this._stopwatch.Stop();
      int millisecondsTimeout = this.MinMillisecondPerFrame - (int) this._stopwatch.ElapsedMilliseconds;
      if (millisecondsTimeout > 0)
        Thread.Sleep(millisecondsTimeout);
    }
    this._stopwatch.Reset();
    this._stopwatch.Start();
  }
}
