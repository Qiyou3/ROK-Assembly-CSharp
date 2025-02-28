// Decompiled with JetBrains decompiler
// Type: DeviceTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine;
using UnityEngine;

#nullable disable
public class DeviceTime : MonoBehaviour
{
  public TOD_Sky sky;

  protected void OnEnable()
  {
    if (!(bool) (Object) this.sky)
      this.sky = TOD_Sky.Instance;
    this.sky.Cycle.DateTime = SafeTime.ServerDate;
  }
}
