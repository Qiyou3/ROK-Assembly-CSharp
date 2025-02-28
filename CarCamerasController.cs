// Decompiled with JetBrains decompiler
// Type: CarCamerasController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CarCamerasController : MonoBehaviour
{
  private CarCameras carCameras;
  private float sizex;
  private float sizey;
  private float sizez;
  [HideInInspector]
  public float externalSizex;
  [HideInInspector]
  public float externalSizey;
  [HideInInspector]
  public float externalSizez;
  private int i;
  public string wheelLayer = "Unitycar - Wheels";

  public void Awake() => this.carCameras = this.GetComponent<CarCameras>();

  private float FindMaxBoundingSize(Collider[] colliders)
  {
    this.sizex = 0.0f;
    this.sizey = 0.0f;
    this.sizez = 0.0f;
    if (colliders.Length != 0)
    {
      foreach (Collider collider in colliders)
      {
        if (collider.gameObject.layer != LayerMask.NameToLayer(this.wheelLayer) && (Object) collider.transform.GetComponent<FuelTank>() == (Object) null)
        {
          this.sizex += collider.bounds.size.x;
          this.sizey += collider.bounds.size.y;
          this.sizez += collider.bounds.size.z;
        }
      }
    }
    return Mathf.Max(this.sizex + this.externalSizex, this.sizey + this.externalSizey, this.sizez + this.externalSizez);
  }

  public void SetCamera(int index, Transform target, bool newTarget)
  {
    this.i = index;
    if (newTarget)
      this.carCameras.mtarget = target;
    this.carCameras.dampFixedCamera = false;
    this.carCameras.mouseOrbitFixedCamera = false;
    this.carCameras.driverView = false;
    if (index == 0 && (Object) target != (Object) null)
    {
      this.carCameras.mycamera = CarCameras.Cameras.SmoothLookAt;
      this.carCameras.target = target;
      this.carCameras.rotationDamping = 3f;
      this.carCameras.heightDamping = 100f;
      this.carCameras.distance = this.FindMaxBoundingSize(target.gameObject.GetComponentsInChildren<Collider>()) * 1.5f;
      if ((double) this.carCameras.distance < 4.0)
        this.carCameras.distance = 4f;
      this.carCameras.height = this.carCameras.distance / 2f;
      this.carCameras.pitchAngle = (float) (-(double) this.carCameras.height / 1.5);
      this.carCameras.yawAngle = 0.0f;
    }
    else if (index == 1 && (Object) target != (Object) null)
    {
      this.carCameras.mycamera = CarCameras.Cameras.FixedTo;
      foreach (Transform componentsInChild in this.carCameras.mtarget.gameObject.GetComponentsInChildren<Transform>())
      {
        if (componentsInChild.gameObject.tag == "Fixed_Camera_Driver_View" || componentsInChild.gameObject.name == "Fixed_Camera_Driver_View")
          this.carCameras.target = componentsInChild;
      }
      this.carCameras.distance = 0.0f;
      this.carCameras.height = 0.0f;
      this.carCameras.pitchAngle = 0.0f;
      this.carCameras.yawAngle = 0.0f;
      this.carCameras.dampFixedCamera = true;
      this.carCameras.mouseOrbitFixedCamera = true;
      this.carCameras.x = 0.0f;
      this.carCameras.y = 0.0f;
      this.carCameras.driverView = true;
    }
    else if (index == 2 && (Object) target != (Object) null)
    {
      this.carCameras.mycamera = CarCameras.Cameras.SmoothLookAt;
      this.carCameras.target = target;
      this.carCameras.rotationDamping = 3f;
      this.carCameras.heightDamping = 100f;
      this.carCameras.distance = this.FindMaxBoundingSize(target.gameObject.GetComponentsInChildren<Collider>());
      if ((double) this.carCameras.distance < 4.0)
        this.carCameras.distance = 4f;
      this.carCameras.distance *= 2f;
      this.carCameras.height = this.carCameras.distance / 2f;
      this.carCameras.pitchAngle = (float) (-(double) this.carCameras.height / 2.5);
      this.carCameras.yawAngle = 0.0f;
    }
    else if (index == 3 && (Object) target != (Object) null)
    {
      this.carCameras.mycamera = CarCameras.Cameras.FixedTo;
      foreach (Transform componentsInChild in this.carCameras.mtarget.gameObject.GetComponentsInChildren<Transform>())
      {
        if (componentsInChild.gameObject.tag == "Fixed_Camera_1" || componentsInChild.gameObject.name == "Fixed_Camera_1")
          this.carCameras.target = componentsInChild;
      }
      this.carCameras.distance = 0.0f;
      this.carCameras.height = 0.0f;
      this.carCameras.pitchAngle = 0.0f;
      this.carCameras.yawAngle = 0.0f;
    }
    else if (index == 4 && (Object) target != (Object) null)
    {
      this.carCameras.mycamera = CarCameras.Cameras.FixedTo;
      foreach (Transform componentsInChild in this.carCameras.mtarget.gameObject.GetComponentsInChildren<Transform>())
      {
        if (componentsInChild.gameObject.tag == "Fixed_Camera_2" || componentsInChild.gameObject.name == "Fixed_Camera_2")
          this.carCameras.target = componentsInChild;
      }
      this.carCameras.distance = 0.0f;
      this.carCameras.height = 0.0f;
      this.carCameras.pitchAngle = 0.0f;
      this.carCameras.yawAngle = 0.0f;
    }
    else if (index == 5 && (Object) target != (Object) null)
    {
      this.carCameras.mycamera = CarCameras.Cameras.SmoothLookAt;
      this.carCameras.rotationDamping = 3f;
      this.carCameras.heightDamping = 50f;
      this.carCameras.target = target;
      this.carCameras.distance = this.FindMaxBoundingSize(target.gameObject.GetComponentsInChildren<Collider>());
      if ((double) this.carCameras.distance < 4.0)
        this.carCameras.distance = 4f;
      this.carCameras.height = this.carCameras.distance / 2f;
      this.carCameras.pitchAngle = -this.carCameras.height;
      this.carCameras.yawAngle = 90f;
    }
    else if (index == 6 && (Object) target != (Object) null)
    {
      this.carCameras.mycamera = CarCameras.Cameras.MouseOrbit;
      this.carCameras.target = target;
      if ((double) this.carCameras.distance < (double) this.carCameras.distanceMin || (double) this.carCameras.distance > (double) this.carCameras.distanceMax)
        this.carCameras.distance = 5f;
      this.carCameras.x = this.carCameras.myTransform.eulerAngles.y;
      this.carCameras.y = this.carCameras.myTransform.eulerAngles.x;
    }
    else
    {
      if (index != 7 || !((Object) target != (Object) null))
        return;
      this.carCameras.mycamera = CarCameras.Cameras.Map;
      this.carCameras.target = target;
      this.carCameras.distance = 0.0f;
      this.carCameras.height = 0.0f;
      this.carCameras.pitchAngle = 0.0f;
      this.carCameras.yawAngle = 0.0f;
    }
  }

  public void Start() => this.SetCamera(0, this.carCameras.mtarget, false);

  public void Update()
  {
    if (this.carCameras.mycamera == CarCameras.Cameras.Map || !Input.GetKeyDown(KeyCode.C))
      return;
    ++this.i;
    if (this.i == 7)
      this.i = 0;
    this.SetCamera(this.i, this.carCameras.mtarget, false);
  }
}
