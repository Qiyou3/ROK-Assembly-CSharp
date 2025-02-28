// Decompiled with JetBrains decompiler
// Type: EffectDestroyOnParentResize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.EffectsPooling;
using UnityEngine;

#nullable disable
public class EffectDestroyOnParentResize : MonoBehaviour
{
  private Vector3 _Size;
  private Transform _Parent;

  public void Awake() => this._Size = new Vector3();

  public void Update()
  {
    if ((Object) this.transform.parent != (Object) this._Parent || this._Size == new Vector3())
    {
      this._Size = this.transform.lossyScale;
      this._Parent = this.transform.parent;
    }
    else
    {
      if (!(this._Size != this.transform.lossyScale))
        return;
      this.transform.parent = (Transform) null;
      this.transform.localScale = this._Size;
      EffectsPool.Destroy(this.gameObject);
    }
  }
}
