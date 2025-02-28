// Decompiled with JetBrains decompiler
// Type: GunAim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
public class GunAim : MonoBehaviour
{
  public Transform traceDirection;
  public Transform camera;
  public Transform cameraCrosshair;
  public Transform gunCrosshair;
  public Transform rotateToAim;
  public Transform gunPivot;

  public void Update()
  {
    Ray ray = new Ray()
    {
      direction = this.traceDirection.forward
    };
    ray.origin = this.camera.position + Vector3UtilEx.Parallel(this.transform.position - this.camera.position, ray.direction);
    this.cameraCrosshair.rotation = Quaternion.LookRotation(ray.direction);
    RaycastHit hitInfo;
    this.rotateToAim.rotation = !Physics.Raycast(ray, out hitInfo) ? this.cameraCrosshair.rotation : Quaternion.LookRotation(hitInfo.point - this.gunPivot.position);
    if (Physics.Raycast(new Ray()
    {
      origin = this.transform.position,
      direction = this.transform.forward
    }, out hitInfo))
      this.gunCrosshair.rotation = Quaternion.LookRotation(hitInfo.point - this.camera.position);
    else
      this.gunCrosshair.rotation = this.transform.rotation;
  }
}
