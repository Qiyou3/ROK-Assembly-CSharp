// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.AttackController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;

#nullable disable
namespace CodeHatch.AI
{
  public class AttackController : AIBehaviour
  {
    public string attackSound;
    public float attackTimeoutMin = 0.5f;
    public float attackTimeoutMax = 1f;
    private AttackBase[] _attacks;
    private bool _started;

    public AttackBase[] Attacks
    {
      get
      {
        if (this._attacks == null)
          this._attacks = this.Entity.TryGetArray<AttackBase>();
        return this._attacks;
      }
    }

    public void Start()
    {
      this._started = true;
      this.StartCoroutine(this.AttackingCoroutine());
    }

    public void OnEnable()
    {
      if (!this._started)
        return;
      this.Start();
    }

    [DebuggerHidden]
    public IEnumerator AttackingCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AttackController.\u003CAttackingCoroutine\u003Ec__IteratorC2()
      {
        \u003C\u003Ef__this = this
      };
    }
  }
}
