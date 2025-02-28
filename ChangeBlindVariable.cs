// Decompiled with JetBrains decompiler
// Type: ChangeBlindVariable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class ChangeBlindVariable
{
  protected float _updateInterval;
  protected float _maxFreezeTime;
  protected float _lastGetTime;
  protected float _lagTime;
  protected float _lastUpdateTime;
  protected bool _init;

  public ChangeBlindVariable(float maxFreezeTime = 5, float updateInterval = 30)
  {
    this._maxFreezeTime = maxFreezeTime;
    this._updateInterval = updateInterval;
  }

  protected float LagTime
  {
    get
    {
      this._lagTime = Mathf.Clamp(this._lagTime, (float) ((double) Time.time - (double) this._maxFreezeTime + 2.0 / (double) this._updateInterval), this._lastUpdateTime);
      return this._lagTime;
    }
  }

  public void SkipTime(float skipTime) => this._lagTime += skipTime;
}
