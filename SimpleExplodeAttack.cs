// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.SimpleExplodeAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class SimpleExplodeAttack : AttackBase
  {
    public float explodeDelay;
    public Animation anim;
    public string[] AttackAnimations;

    public override void TriggerAttack(Location locationToAttack)
    {
      this.anim.CrossFade(this.AttackAnimations[Random.Range(0, this.AttackAnimations.Length)], 0.2f);
      this.StartCoroutine(this.Delay());
    }

    [DebuggerHidden]
    private IEnumerator Delay()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new SimpleExplodeAttack.\u003CDelay\u003Ec__IteratorCB()
      {
        \u003C\u003Ef__this = this
      };
    }
  }
}
