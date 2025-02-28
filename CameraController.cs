// Decompiled with JetBrains decompiler
// Type: CameraController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CameraController : MonoBehaviour
{
  public Transform cameraTarget;
  public Light light;
  [Range(0.5f, 5f)]
  public float headDistance = 0.8f;
  [Range(0.5f, 5f)]
  public float bodyDistance = 2f;
  [HideInInspector]
  public float distance = 2f;
  public float xSpeed = 250f;
  public float ySpeed = 120f;
  [Range(-90f, 0.0f)]
  public int yMinLimit = -20;
  [Range(0.0f, 90f)]
  public int yMaxLimit = 80;
  private float x;
  private float y;
  public float smoothTime = 0.3f;
  private float xSmooth;
  private float ySmooth;
  private float xVelocity;
  private float yVelocity;
  private Vector3 posSmooth = Vector3.zero;

  public void OnEnable() => this.light.enabled = true;

  public void OnDisable() => this.light.enabled = false;

  private void Start()
  {
    if ((Object) this.cameraTarget == (Object) null)
      this.LogError<CameraController>("Camera Target is missing! Please link it to UMAzing>Camera Target");
    this.x = this.transform.eulerAngles.y + 180f;
    this.y = 0.0f;
    if (!(bool) (Object) this.GetComponent<Rigidbody>())
      return;
    this.GetComponent<Rigidbody>().freezeRotation = true;
  }

  private void Update()
  {
    if (!(bool) (Object) this.cameraTarget)
      return;
    if (Input.GetMouseButton(1))
    {
      this.x += (float) ((double) Input.GetAxis("Mouse X") * (double) this.xSpeed * 0.019999999552965164);
      this.y -= (float) ((double) Input.GetAxis("Mouse Y") * (double) this.ySpeed * 0.019999999552965164);
      this.y = Mathf.Clamp(this.y, (float) this.yMinLimit, (float) this.yMaxLimit);
    }
    this.xSmooth = Mathf.SmoothDamp(this.xSmooth, this.x, ref this.xVelocity, this.smoothTime);
    this.ySmooth = Mathf.SmoothDamp(this.ySmooth, this.y, ref this.yVelocity, this.smoothTime);
    this.ySmooth = CameraController.ClampAngle(this.ySmooth, (float) this.yMinLimit, (float) this.yMaxLimit);
    Quaternion quaternion = Quaternion.Euler(this.ySmooth, this.xSmooth, 0.0f);
    this.posSmooth = this.cameraTarget.position;
    this.transform.rotation = quaternion;
    this.transform.position = quaternion * new Vector3(0.0f, 0.0f, -this.distance) + this.posSmooth;
  }

  private static float ClampAngle(float angle, float min, float max)
  {
    if ((double) angle < -360.0)
      angle += 360f;
    if ((double) angle > 360.0)
      angle -= 360f;
    return Mathf.Clamp(angle, min, max);
  }
}
