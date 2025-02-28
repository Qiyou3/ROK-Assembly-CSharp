// Decompiled with JetBrains decompiler
// Type: ChangeBlindController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ChangeBlindController : MonoBehaviour
{
  public static ChangeBlindController Instance;
  public AnimationCurve CameraRotationToTimeSkip = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(180f, 10f)
  });
  private Quaternion _lastRotation;
  [HideInInspector]
  public float SkipTime;

  public void Awake() => ChangeBlindController.Instance = this;

  public void Update()
  {
    this.SkipTime = this.CameraRotationToTimeSkip.Evaluate(Quaternion.Angle(this._lastRotation, this.transform.rotation));
    this._lastRotation = this.transform.rotation;
  }
}
