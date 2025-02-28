// Decompiled with JetBrains decompiler
// Type: EditorSwitch`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public abstract class EditorSwitch<T> : MonoBehaviour
{
  public bool On;
  private bool _prevOn;
  public T[] Targets;

  public void Update()
  {
    this.Update_Internal();
    this._prevOn = this.On;
  }

  protected abstract void Update_Internal();

  protected bool JustTurnedOn => !this._prevOn && this.On;

  protected bool JustTurnedOff => this._prevOn && !this.On;
}
