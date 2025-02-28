// Decompiled with JetBrains decompiler
// Type: CameraSolver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CameraSolver : MonoBehaviour
{
  public Camera cameraObject;
  public Transform[] ObjectsOfInterest;
  private Vector3[] objectBounds;

  public void Start()
  {
    if (!(bool) (Object) this.cameraObject)
      this.GetComponentInChildren<Camera>();
    if (!(bool) (Object) this.cameraObject)
      this.enabled = false;
    else
      this.objectBounds = new Vector3[8];
  }

  public void FixedUpdate()
  {
    Vector3 position = Vector3.zero;
    Vector3 vector3_1 = Vector3.zero;
    bool flag = true;
    foreach (Transform transform in this.ObjectsOfInterest)
    {
      Vector3 vector3_2 = this.cameraObject.transform.InverseTransformPoint(transform.position);
      if (flag)
      {
        position = vector3_2;
        vector3_1 = vector3_2;
      }
      else
      {
        position.x = Mathf.Min(position.x, vector3_2.x);
        position.y = Mathf.Min(position.y, vector3_2.y);
        position.z = Mathf.Min(position.z, vector3_2.z);
        vector3_1.x = Mathf.Max(vector3_1.x, vector3_2.x);
        vector3_1.y = Mathf.Max(vector3_1.y, vector3_2.y);
        vector3_1.z = Mathf.Max(vector3_1.z, vector3_2.z);
      }
    }
    position = this.cameraObject.transform.TransformPoint(position);
    vector3_1 = this.cameraObject.transform.TransformPoint(position);
    this.objectBounds[0].x = position.x;
    this.objectBounds[1].x = vector3_1.x;
    this.objectBounds[2].x = position.x;
    this.objectBounds[3].x = vector3_1.x;
    this.objectBounds[4].x = position.x;
    this.objectBounds[5].x = vector3_1.x;
    this.objectBounds[6].x = position.x;
    this.objectBounds[7].x = vector3_1.x;
    this.objectBounds[0].y = position.y;
    this.objectBounds[1].y = position.y;
    this.objectBounds[2].y = vector3_1.y;
    this.objectBounds[3].y = vector3_1.y;
    this.objectBounds[4].y = position.y;
    this.objectBounds[5].y = position.y;
    this.objectBounds[6].y = vector3_1.y;
    this.objectBounds[7].y = vector3_1.y;
    this.objectBounds[0].z = position.z;
    this.objectBounds[1].z = position.z;
    this.objectBounds[2].z = position.z;
    this.objectBounds[3].z = position.z;
    this.objectBounds[4].z = vector3_1.z;
    this.objectBounds[5].z = vector3_1.z;
    this.objectBounds[6].z = vector3_1.z;
    this.objectBounds[7].z = vector3_1.z;
  }
}
