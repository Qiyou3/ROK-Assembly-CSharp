// Decompiled with JetBrains decompiler
// Type: UnityTest.CheckMethod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace UnityTest
{
  [Flags]
  public enum CheckMethod
  {
    AfterPeriodOfTime = 1,
    Start = 2,
    Update = 4,
    FixedUpdate = 8,
    LateUpdate = 16, // 0x00000010
    OnDestroy = 32, // 0x00000020
    OnEnable = 64, // 0x00000040
    OnDisable = 128, // 0x00000080
    OnControllerColliderHit = 256, // 0x00000100
    OnParticleCollision = 512, // 0x00000200
    OnJointBreak = 1024, // 0x00000400
    OnBecameInvisible = 2048, // 0x00000800
    OnBecameVisible = 4096, // 0x00001000
    OnTriggerEnter = 8192, // 0x00002000
    OnTriggerExit = 16384, // 0x00004000
    OnTriggerStay = 32768, // 0x00008000
    OnCollisionEnter = 65536, // 0x00010000
    OnCollisionExit = 131072, // 0x00020000
    OnCollisionStay = 262144, // 0x00040000
    OnTriggerEnter2D = 524288, // 0x00080000
    OnTriggerExit2D = 1048576, // 0x00100000
    OnTriggerStay2D = 2097152, // 0x00200000
    OnCollisionEnter2D = 4194304, // 0x00400000
    OnCollisionExit2D = 8388608, // 0x00800000
    OnCollisionStay2D = 16777216, // 0x01000000
  }
}
