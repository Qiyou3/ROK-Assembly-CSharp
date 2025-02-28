// Decompiled with JetBrains decompiler
// Type: UnityTest.IAssertionComponentConfigurator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace UnityTest
{
  public interface IAssertionComponentConfigurator
  {
    int UpdateCheckStartOnFrame { set; }

    int UpdateCheckRepeatFrequency { set; }

    bool UpdateCheckRepeat { set; }

    float TimeCheckStartAfter { set; }

    float TimeCheckRepeatFrequency { set; }

    bool TimeCheckRepeat { set; }

    AssertionComponent Component { get; }
  }
}
