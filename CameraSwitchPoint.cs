// Decompiled with JetBrains decompiler
// Type: CameraSwitchPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CameraSwitchPoint : MonoBehaviour
{
  public static List<CameraSwitchPoint> allPoints = new List<CameraSwitchPoint>();
  public static List<CameraSwitchPoint> enabledPoints = new List<CameraSwitchPoint>();
  public bool allowParent = true;
  public float fov;
  public float interpolationTime = 0.1f;

  public void Awake()
  {
    CameraSwitchPoint.allPoints.Add(this);
    if (!this.enabled)
      return;
    CameraSwitchPoint.enabledPoints.AddDistinct<CameraSwitchPoint>(this);
  }

  public void OnDestroy()
  {
    CameraSwitchPoint.allPoints.Remove(this);
    if (!this.enabled)
      return;
    CameraSwitchPoint.enabledPoints.RemoveIfContains<CameraSwitchPoint>(this);
  }

  public void OnEnable() => CameraSwitchPoint.enabledPoints.AddDistinct<CameraSwitchPoint>(this);

  public void OnDisable()
  {
    CameraSwitchPoint.enabledPoints.RemoveIfContains<CameraSwitchPoint>(this);
  }
}
