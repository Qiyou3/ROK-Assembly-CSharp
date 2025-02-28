// Decompiled with JetBrains decompiler
// Type: CeilingPlacingInstructions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CeilingPlacingInstructions : MonoBehaviour, IPlacerInstructions
{
  public bool ignoreRotationOffset;

  public bool IgnoreRotationOffset
  {
    get => this.ignoreRotationOffset;
    set
    {
    }
  }

  public void Modify(ref Vector3 goalPosition, ref Quaternion goalRotation, Vector3 goalNormal)
  {
    float y = goalRotation.eulerAngles.y;
    goalRotation = Quaternion.LookRotation(new Vector3(1f, 0.0f, 1f), -goalNormal);
    Vector3 eulerAngles = goalRotation.eulerAngles;
    goalRotation = Quaternion.Euler(eulerAngles.x, y, eulerAngles.z);
  }
}
