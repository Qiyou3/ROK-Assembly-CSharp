// Decompiled with JetBrains decompiler
// Type: AudioFader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AudioFader
{
  private float _fadeOutTotalTime = -1f;
  private double _fadeOutStartTime = -1.0;
  private float _fadeInTotalTime = -1f;
  private double _fadeInStartTime = -1.0;

  public double time { get; set; }

  public bool isFadingOutComplete
  {
    get
    {
      return this._fadeOutStartTime > 0.0 ? (double) this._fadeOutTotalTime >= 0.0 && this.time >= this._fadeOutStartTime + (double) this._fadeOutTotalTime : (double) this._fadeOutTotalTime >= 0.0 && this.time >= (double) this._fadeOutTotalTime;
    }
  }

  public bool isFadingOut
  {
    get
    {
      return this._fadeOutStartTime > 0.0 ? (double) this._fadeOutTotalTime >= 0.0 && this.time >= this._fadeOutStartTime && this.time < this._fadeOutStartTime + (double) this._fadeOutTotalTime : (double) this._fadeOutTotalTime >= 0.0 && this.time < (double) this._fadeOutTotalTime;
    }
  }

  public bool isFadingOutOrScheduled => (double) this._fadeOutTotalTime >= 0.0;

  public bool isFadingIn
  {
    get
    {
      return this._fadeInStartTime > 0.0 ? (double) this._fadeInTotalTime > 0.0 && this.time >= this._fadeInStartTime && this.time - this._fadeInStartTime < (double) this._fadeInTotalTime : (double) this._fadeInTotalTime > 0.0 && this.time < (double) this._fadeInTotalTime;
    }
  }

  public void Set0()
  {
    this.time = 0.0;
    this._fadeOutTotalTime = -1f;
    this._fadeOutStartTime = -1.0;
    this._fadeInTotalTime = -1f;
    this._fadeInStartTime = -1.0;
  }

  public void FadeIn(float fadeInTime, bool stopCurrentFadeOut = false)
  {
    this.FadeIn(fadeInTime, this.time, stopCurrentFadeOut);
  }

  public void FadeIn(float fadeInTime, double startToFadeTime, bool stopCurrentFadeOut = false)
  {
    if (this.isFadingOutOrScheduled && stopCurrentFadeOut)
    {
      float fadeOutValue = this._GetFadeOutValue();
      this._fadeOutTotalTime = -1f;
      this._fadeOutStartTime = -1.0;
      this._fadeInTotalTime = fadeInTime;
      this._fadeInStartTime = startToFadeTime - (double) fadeInTime * (double) fadeOutValue;
    }
    else
    {
      this._fadeInTotalTime = fadeInTime;
      this._fadeInStartTime = startToFadeTime;
    }
  }

  public void FadeOut(float fadeOutLength, float startToFadeTime)
  {
    if (this.isFadingOutOrScheduled)
    {
      double num1 = this.time + (double) startToFadeTime + (double) fadeOutLength;
      double num2 = this._fadeOutStartTime + (double) this._fadeOutTotalTime;
      if (num2 < num1)
        return;
      double num3 = this.time - this._fadeOutStartTime;
      double num4 = (double) startToFadeTime + (double) fadeOutLength;
      double num5 = num2 - this.time;
      if (num5 == 0.0)
        return;
      double num6 = num3 * num4 / num5;
      this._fadeOutStartTime = this.time - num6;
      this._fadeOutTotalTime = (float) (num4 + num6);
    }
    else
    {
      this._fadeOutTotalTime = fadeOutLength;
      this._fadeOutStartTime = this.time + (double) startToFadeTime;
    }
  }

  public float Get(out bool finishedFadeOut)
  {
    float num = 1f;
    finishedFadeOut = false;
    if (this.isFadingOutOrScheduled)
    {
      num *= this._GetFadeOutValue();
      if ((double) num == 0.0)
      {
        finishedFadeOut = true;
        return 0.0f;
      }
    }
    if (this.isFadingIn)
      num *= this._GetFadeInValue();
    return num;
  }

  private float _GetFadeOutValue()
  {
    return 1f - this._GetFadeValue((float) (this.time - this._fadeOutStartTime), this._fadeOutTotalTime);
  }

  private float _GetFadeInValue()
  {
    return this._GetFadeValue((float) (this.time - this._fadeInStartTime), this._fadeInTotalTime);
  }

  private float _GetFadeValue(float t, float dt)
  {
    if ((double) dt > 0.0)
      return Mathf.Clamp(t / dt, 0.0f, 1f);
    return (double) t > 0.0 ? 1f : 0.0f;
  }
}
