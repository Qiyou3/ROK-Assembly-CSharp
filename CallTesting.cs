// Decompiled with JetBrains decompiler
// Type: UnityTest.CallTesting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace UnityTest
{
  public class CallTesting : MonoBehaviour
  {
    public int afterFrames;
    public float afterSeconds;
    public CallTesting.Functions callOnMethod = CallTesting.Functions.Start;
    public CallTesting.Method methodToCall;
    private int m_StartFrame;
    private float m_StartTime;

    private void TryToCallTesting(CallTesting.Functions invokingMethod)
    {
      if (invokingMethod != this.callOnMethod)
        return;
      if (this.methodToCall == CallTesting.Method.Pass)
        IntegrationTest.Pass(this.gameObject);
      else
        IntegrationTest.Fail(this.gameObject);
      this.afterFrames = 0;
      this.afterSeconds = 0.0f;
      this.m_StartTime = float.PositiveInfinity;
      this.m_StartFrame = int.MinValue;
    }

    public void Start()
    {
      this.m_StartTime = Time.time;
      this.m_StartFrame = this.afterFrames;
      this.TryToCallTesting(CallTesting.Functions.Start);
    }

    public void Update()
    {
      this.TryToCallTesting(CallTesting.Functions.Update);
      this.CallAfterSeconds();
      this.CallAfterFrames();
    }

    private void CallAfterFrames()
    {
      if (this.afterFrames <= 0 || this.m_StartFrame + this.afterFrames > Time.frameCount)
        return;
      this.TryToCallTesting(CallTesting.Functions.CallAfterFrames);
    }

    private void CallAfterSeconds()
    {
      if ((double) this.m_StartTime + (double) this.afterSeconds > (double) Time.time)
        return;
      this.TryToCallTesting(CallTesting.Functions.CallAfterSeconds);
    }

    public void OnDisable() => this.TryToCallTesting(CallTesting.Functions.OnDisable);

    public void OnEnable() => this.TryToCallTesting(CallTesting.Functions.OnEnable);

    public void OnDestroy() => this.TryToCallTesting(CallTesting.Functions.OnDestroy);

    public void FixedUpdate() => this.TryToCallTesting(CallTesting.Functions.FixedUpdate);

    public void LateUpdate() => this.TryToCallTesting(CallTesting.Functions.LateUpdate);

    public void OnControllerColliderHit()
    {
      this.TryToCallTesting(CallTesting.Functions.OnControllerColliderHit);
    }

    public void OnParticleCollision()
    {
      this.TryToCallTesting(CallTesting.Functions.OnParticleCollision);
    }

    public void OnJointBreak() => this.TryToCallTesting(CallTesting.Functions.OnJointBreak);

    public void OnBecameInvisible()
    {
      this.TryToCallTesting(CallTesting.Functions.OnBecameInvisible);
    }

    public void OnBecameVisible() => this.TryToCallTesting(CallTesting.Functions.OnBecameVisible);

    public void OnTriggerEnter() => this.TryToCallTesting(CallTesting.Functions.OnTriggerEnter);

    public void OnTriggerExit() => this.TryToCallTesting(CallTesting.Functions.OnTriggerExit);

    public void OnTriggerStay() => this.TryToCallTesting(CallTesting.Functions.OnTriggerStay);

    public void OnCollisionEnter() => this.TryToCallTesting(CallTesting.Functions.OnCollisionEnter);

    public void OnCollisionExit() => this.TryToCallTesting(CallTesting.Functions.OnCollisionExit);

    public void OnCollisionStay() => this.TryToCallTesting(CallTesting.Functions.OnCollisionStay);

    public void OnTriggerEnter2D() => this.TryToCallTesting(CallTesting.Functions.OnTriggerEnter2D);

    public void OnTriggerExit2D() => this.TryToCallTesting(CallTesting.Functions.OnTriggerExit2D);

    public void OnTriggerStay2D() => this.TryToCallTesting(CallTesting.Functions.OnTriggerStay2D);

    public void OnCollisionEnter2D()
    {
      this.TryToCallTesting(CallTesting.Functions.OnCollisionEnter2D);
    }

    public void OnCollisionExit2D()
    {
      this.TryToCallTesting(CallTesting.Functions.OnCollisionExit2D);
    }

    public void OnCollisionStay2D()
    {
      this.TryToCallTesting(CallTesting.Functions.OnCollisionStay2D);
    }

    public enum Functions
    {
      CallAfterSeconds,
      CallAfterFrames,
      Start,
      Update,
      FixedUpdate,
      LateUpdate,
      OnDestroy,
      OnEnable,
      OnDisable,
      OnControllerColliderHit,
      OnParticleCollision,
      OnJointBreak,
      OnBecameInvisible,
      OnBecameVisible,
      OnTriggerEnter,
      OnTriggerExit,
      OnTriggerStay,
      OnCollisionEnter,
      OnCollisionExit,
      OnCollisionStay,
      OnTriggerEnter2D,
      OnTriggerExit2D,
      OnTriggerStay2D,
      OnCollisionEnter2D,
      OnCollisionExit2D,
      OnCollisionStay2D,
    }

    public enum Method
    {
      Pass,
      Fail,
    }
  }
}
