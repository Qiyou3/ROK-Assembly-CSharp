// Decompiled with JetBrains decompiler
// Type: CarCameras
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (CarCamerasController))]
public class CarCameras : MonoBehaviour
{
  public CarCameras.Cameras mycamera;
  public Transform target;
  public float distance = 10f;
  public float height = 2.5f;
  public float yawAngle;
  public float pitchAngle = -2.5f;
  public float rotationDamping = 3f;
  public float heightDamping = 2f;
  [HideInInspector]
  public bool dampFixedCamera;
  [HideInInspector]
  public bool mouseOrbitFixedCamera;
  [HideInInspector]
  public bool driverView;
  private float xSpeed = 10f;
  private float ySpeed = 10f;
  private float yMinLimit = -20f;
  private float yMaxLimit = 80f;
  [HideInInspector]
  public float distanceMin = 4f;
  [HideInInspector]
  public float distanceMax = 30f;
  private float mouseDamping = 10f;
  [HideInInspector]
  public float x;
  [HideInInspector]
  public float y;
  private float currentYawRotationAngle;
  private float wantedYawRotationAngle;
  private float currentHeight;
  private float wantedHeight;
  private float deltaPitchAngle;
  private Vector3 deltaMovement;
  private Vector3 oldVelocity;
  private Vector3 deltaVelocity;
  private Vector3 velocity;
  private Vector3 accel;
  private float centrifugalAccel;
  private CarDynamics cardynamics;
  [HideInInspector]
  public Transform myTransform;
  [HideInInspector]
  public Transform mtarget;
  private Quaternion rotation;

  public void Start()
  {
    this.myTransform = this.transform;
    this.mtarget = this.target;
    if (!(bool) (Object) this.mtarget)
      return;
    this.cardynamics = this.mtarget.GetComponent<CarDynamics>();
  }

  public void LateUpdate()
  {
    if (!(bool) (Object) this.target)
      return;
    if (this.mycamera == CarCameras.Cameras.MouseOrbit)
    {
      this.x += Input.GetAxis("Mouse X") * this.xSpeed;
      this.y -= Input.GetAxis("Mouse Y") * this.ySpeed;
      this.y = CarCameras.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
      this.rotation = Quaternion.Slerp(this.myTransform.rotation, Quaternion.Euler(this.y, this.x, 0.0f), Time.deltaTime * this.mouseDamping);
      this.distance -= Input.GetAxis("Mouse ScrollWheel") * 5f;
      this.distance = Mathf.Clamp(this.distance, this.distanceMin, this.distanceMax);
      Vector3 vector3 = this.rotation * new Vector3(0.0f, 0.0f, -this.distance) + this.target.position;
      this.myTransform.rotation = this.rotation;
      this.myTransform.position = vector3;
    }
    else if (this.mycamera == CarCameras.Cameras.Map)
    {
      this.myTransform.position = new Vector3(this.target.position.x, this.myTransform.position.y, this.target.position.z);
      this.myTransform.eulerAngles = new Vector3(this.myTransform.eulerAngles.x, this.target.eulerAngles.y, this.myTransform.eulerAngles.z);
    }
    else if (this.mycamera == CarCameras.Cameras.SmoothLookAt)
    {
      this.wantedYawRotationAngle = this.target.eulerAngles.y + this.yawAngle;
      this.currentYawRotationAngle = this.myTransform.eulerAngles.y;
      this.wantedHeight = this.target.position.y + this.height;
      this.currentHeight = this.myTransform.position.y;
      this.currentYawRotationAngle = Mathf.LerpAngle(this.currentYawRotationAngle, this.wantedYawRotationAngle, this.rotationDamping * Time.deltaTime);
      this.currentHeight = Mathf.Lerp(this.currentHeight, this.wantedHeight, this.heightDamping * Time.deltaTime);
      Quaternion quaternion = Quaternion.Euler(0.0f, this.currentYawRotationAngle, 0.0f);
      this.myTransform.position = this.target.position;
      this.myTransform.position -= quaternion * Vector3.forward * this.distance;
      this.myTransform.position = new Vector3(this.myTransform.position.x, this.currentHeight, this.myTransform.position.z);
      this.myTransform.LookAt(new Vector3(this.target.position.x, this.target.position.y + this.height + this.pitchAngle, this.target.position.z));
    }
    else
    {
      if (this.mycamera != CarCameras.Cameras.FixedTo)
        return;
      if (this.mouseOrbitFixedCamera)
      {
        this.x += Input.GetAxis("Mouse X") * this.xSpeed;
        this.y -= Input.GetAxis("Mouse Y") * this.ySpeed;
        this.y = CarCameras.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
      }
      else
        this.x = this.y = 0.0f;
      this.rotation = Quaternion.Slerp(this.myTransform.rotation, Quaternion.Euler(this.y + this.target.eulerAngles.x + this.pitchAngle + this.deltaPitchAngle, this.x + this.target.eulerAngles.y + this.yawAngle, this.target.eulerAngles.z), Time.time);
      this.myTransform.rotation = this.rotation;
      if (this.dampFixedCamera)
      {
        this.myTransform.position = new Vector3(this.target.position.x, this.target.position.y + this.height, this.target.position.z) - this.myTransform.forward * this.distance - (this.deltaMovement.z * this.target.forward + this.deltaMovement.y * this.target.up + this.deltaMovement.x * this.target.right);
      }
      else
      {
        this.myTransform.eulerAngles = new Vector3(this.target.eulerAngles.x + this.pitchAngle, this.target.eulerAngles.y + this.yawAngle, this.target.eulerAngles.z);
        this.myTransform.position = new Vector3(this.target.position.x, this.target.position.y + this.height, this.target.position.z) - this.myTransform.forward * this.distance;
      }
    }
  }

  public void FixedUpdate()
  {
    if (!this.dampFixedCamera)
      return;
    this.oldVelocity = this.velocity;
    this.velocity = this.mtarget.InverseTransformDirection(this.mtarget.GetComponent<Rigidbody>().velocity);
    this.deltaVelocity = this.velocity - this.oldVelocity;
    if ((Object) this.cardynamics != (Object) null)
      this.centrifugalAccel = this.cardynamics.GetCentrifugalAccel();
    this.accel = this.deltaVelocity / Time.deltaTime + this.centrifugalAccel * Vector3.right;
    this.deltaMovement = this.accel * Time.deltaTime * Time.deltaTime * 5f;
    this.deltaMovement.z = Mathf.Clamp(this.deltaMovement.z, -0.2f, 0.2f);
    this.deltaMovement.y = Mathf.Clamp(this.deltaMovement.y, -0.1f, 0.1f);
    this.deltaMovement.x = Mathf.Clamp(this.deltaMovement.x, -0.01f, 0.01f);
    this.deltaPitchAngle = (float) ((double) this.deltaMovement.y * 20.0 - (double) this.deltaMovement.z * 20.0);
    this.deltaPitchAngle = Mathf.Clamp(this.deltaPitchAngle, -5f, 5f);
  }

  private static float ClampAngle(float yawAngle, float min, float max)
  {
    if ((double) yawAngle < -360.0)
      yawAngle += 360f;
    else if ((double) yawAngle > 360.0)
      yawAngle -= 360f;
    return Mathf.Clamp(yawAngle, min, max);
  }

  public enum Cameras
  {
    SmoothLookAt,
    MouseOrbit,
    FixedTo,
    Map,
  }
}
