// Decompiled with JetBrains decompiler
// Type: EnableParticlesAtDistance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public class EnableParticlesAtDistance : MonoBehaviour
{
  private ParticleSystem _particleSystem;
  [SerializeField]
  private Transform _followTransform;
  private static float UPDATE_RATE = 1f;
  public float EnableDistance = 100f;
  private bool _emissionEnabled;

  public ParticleSystem ParticleSystem
  {
    get
    {
      if ((Object) this._particleSystem == (Object) null)
        this._particleSystem = this.GetComponent<ParticleSystem>();
      return this._particleSystem;
    }
  }

  public Transform FollowTransform
  {
    get
    {
      if ((Object) this._followTransform == (Object) null)
      {
        if ((Object) Camera.main == (Object) null)
          return (Transform) null;
        this._followTransform = Camera.main.transform;
      }
      return this._followTransform;
    }
  }

  public void OnEnable() => this.StartCoroutine(this.UpdateCheck());

  [DebuggerHidden]
  public IEnumerator UpdateCheck()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new EnableParticlesAtDistance.\u003CUpdateCheck\u003Ec__Iterator32()
    {
      \u003C\u003Ef__this = this
    };
  }
}
