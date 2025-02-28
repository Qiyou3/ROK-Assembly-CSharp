// Decompiled with JetBrains decompiler
// Type: CompassTick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CompassTick : MonoBehaviour
{
  public Transform camera;
  public Transform target;
  private float angle;
  private Vector3 offset;
  private float outwards;
  private float upwards;
  public bool upright = true;

  public void Start()
  {
    if (!(bool) (Object) this.target)
    {
      this.angle = this.transform.localRotation.eulerAngles.y;
      this.offset = Quaternion.AngleAxis(this.angle, Vector3.up) * Vector3.forward;
      this.offset.Normalize();
    }
    this.outwards = Vector3.Magnitude(this.transform.localPosition with
    {
      y = 0.0f
    });
    this.upwards = this.transform.localPosition.y;
  }

  public void Update()
  {
    if ((bool) (Object) this.target)
    {
      this.offset = this.target.position - this.camera.position;
      this.offset.Normalize();
    }
    this.transform.position = this.transform.parent.position + this.offset + Vector3.up * Vector3.Dot(-this.offset, this.transform.parent.up) / Vector3.Dot(Vector3.up, this.transform.parent.up);
    this.transform.localPosition = this.transform.localPosition.normalized * this.outwards + Vector3.up * this.upwards;
    this.transform.rotation = Quaternion.LookRotation(this.transform.position - this.transform.parent.position, !this.upright ? Vector3.up : this.transform.parent.up);
  }
}
