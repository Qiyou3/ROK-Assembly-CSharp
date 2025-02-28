// Decompiled with JetBrains decompiler
// Type: ChangeBlindVariable`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ChangeBlindVariable<T> : ChangeBlindVariable
{
  private int _lastHead;
  private T[] _history;

  public ChangeBlindVariable(float maxFreezeTime = 3, float updateInterval = 30)
    : base(maxFreezeTime, updateInterval)
  {
    this._history = new T[Mathf.Max(Mathf.CeilToInt(maxFreezeTime * updateInterval), 1)];
  }

  public T Value
  {
    get
    {
      if ((Object) ChangeBlindController.Instance != (Object) null)
        this.SkipTime(ChangeBlindController.Instance.SkipTime);
      return this._history[this.Tail];
    }
    set
    {
      if (!this._init)
      {
        for (int index = 0; index < this._history.Length; ++index)
          this._history[index] = value;
        this._init = true;
      }
      else
      {
        int num = this.WrapIndex(this.Head + 1);
        this._history[this.WrapIndex(this._lastHead + 1)] = value;
        for (int index = this.WrapIndex(this._lastHead + 1); index != num; index = this.WrapIndex(index + 1))
          this._history[index] = value;
      }
      this._lastHead = this.Head;
      this._lastUpdateTime = Time.time;
    }
  }

  private int Head => this.IndexAtTime(Time.time);

  private int Tail => this.IndexAtTime(this.LagTime);

  private int IndexAtTime(float time)
  {
    return (int) ((double) time * (double) this._updateInterval) % this._history.Length;
  }

  private int WrapIndex(int index) => index % this._history.Length;
}
