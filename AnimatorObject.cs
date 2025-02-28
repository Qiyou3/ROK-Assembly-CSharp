// Decompiled with JetBrains decompiler
// Type: AnimatorObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public struct AnimatorObject
{
  [SerializeField]
  private string _name;
  private int _id;

  private AnimatorObject(string name)
  {
    this._name = name;
    this._id = Animator.StringToHash(name);
  }

  public string Name
  {
    get => this._name;
    set
    {
      if (this._name == value)
        return;
      this._id = Animator.StringToHash(value);
      this._name = value;
    }
  }

  public static implicit operator int(AnimatorObject animatorObject)
  {
    if (animatorObject._id == 0)
      animatorObject._id = Animator.StringToHash(animatorObject._name);
    return animatorObject._id;
  }

  public static implicit operator AnimatorObject(string name) => new AnimatorObject(name);
}
