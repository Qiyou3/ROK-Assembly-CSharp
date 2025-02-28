// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.BipedDepenetrationModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class BipedDepenetrationModifier : EntityBehaviour
  {
    public float TorsoMaxDepenetrationVelocity = 4f;
    public float LegsMaxDepenetrationVelocity = 4f;
    private BipedBody _body;
    private bool _started;

    public void Start()
    {
      this._started = true;
      this._body = this.Entity.Get<BipedBody>();
      if (!((Object) this._body != (Object) null))
        return;
      this._body.Torso.maxDepenetrationVelocity = this.TorsoMaxDepenetrationVelocity;
      this._body.Legs.maxDepenetrationVelocity = this.LegsMaxDepenetrationVelocity;
    }

    public void OnEnable()
    {
      if (!this._started)
        return;
      if ((Object) this._body == (Object) null)
        this._body = this._body = this.Entity.Get<BipedBody>();
      if ((Object) this._body != (Object) null)
      {
        this._body.Torso.maxDepenetrationVelocity = this.TorsoMaxDepenetrationVelocity;
        this._body.Legs.maxDepenetrationVelocity = this.LegsMaxDepenetrationVelocity;
      }
      this.StartCoroutine(this.Wait());
    }

    [DebuggerHidden]
    private IEnumerator Wait()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new BipedDepenetrationModifier.\u003CWait\u003Ec__IteratorE4()
      {
        \u003C\u003Ef__this = this
      };
    }

    public void OnValidate()
    {
      if (!this._started || !((Object) this._body != (Object) null))
        return;
      this._body.Torso.maxDepenetrationVelocity = this.TorsoMaxDepenetrationVelocity;
      this._body.Legs.maxDepenetrationVelocity = this.LegsMaxDepenetrationVelocity;
    }
  }
}
