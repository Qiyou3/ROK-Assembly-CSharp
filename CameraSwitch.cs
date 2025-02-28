// Decompiled with JetBrains decompiler
// Type: CameraSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Configuration;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Label("Camera Settings")]
public class CameraSwitch : MonoBehaviour, IConfigurable
{
  private List<CameraSwitch.SwitchPointState> switchPointStates = new List<CameraSwitch.SwitchPointState>();
  public Camera[] cameras;
  public CameraSwitchPoint currentSwitchPoint;
  public CameraSwitch.SwitchPointData currentData;
  public bool reparentCamera = true;
  public float ratioToApplyParentTransform = 0.5f;
  private float _ratio;
  private float previousRatio = -1f;
  public CameraSwitchPoint previousSwitchPoint;
  public bool respondToParentDestroys = true;
  public static float fovMultiplier = 1f;
  [Range(40f, 105f)]
  [Configurable]
  [DefaultValue(80f)]
  [Label("Field of View")]
  public static float readableFOV;

  public void Awake() => this.InitializeConfigurable();

  public void OnParentWillDestroy()
  {
    if (!this.respondToParentDestroys)
      return;
    CameraSwitch.SwitchPointState switchPointState1 = (CameraSwitch.SwitchPointState) null;
    foreach (CameraSwitch.SwitchPointState switchPointState2 in this.switchPointStates)
    {
      if ((Object) switchPointState2.switchPoint == (Object) this.currentSwitchPoint)
      {
        switchPointState1 = switchPointState2;
        break;
      }
    }
    if (switchPointState1 != null)
      switchPointState1.allowParent = false;
    this.transform.parent = (Transform) null;
  }

  public void LateUpdate()
  {
    if (this.cameras == null || this.cameras.Length == 0)
      this.cameras = this.GetComponentsInChildren<Camera>();
    if ((Object) this.currentSwitchPoint == (Object) null)
      return;
    if ((Object) this.currentSwitchPoint != (Object) this.previousSwitchPoint)
      this.previousRatio = -1f;
    CameraSwitch.SwitchPointState switchPointState1 = (CameraSwitch.SwitchPointState) null;
    foreach (CameraSwitch.SwitchPointState switchPointState2 in this.switchPointStates)
    {
      if ((Object) switchPointState2.switchPoint == (Object) this.currentSwitchPoint)
      {
        switchPointState1 = switchPointState2;
        break;
      }
    }
    if (switchPointState1 == null)
    {
      switchPointState1 = new CameraSwitch.SwitchPointState(this.currentSwitchPoint);
      this.switchPointStates.Add(switchPointState1);
    }
    if (Application.isPlaying)
    {
      float num1 = 1f - Mathf.Pow(0.5f, Time.deltaTime / this.currentSwitchPoint.interpolationTime);
      float num2 = 0.0f;
      foreach (CameraSwitch.SwitchPointState switchPointState3 in this.switchPointStates)
      {
        if (switchPointState3 == switchPointState1)
          switchPointState3.data.weight += (1f - switchPointState3.data.weight) * num1;
        else
          switchPointState3.data.weight += (0.0f - switchPointState3.data.weight) * num1;
        num2 += switchPointState3.data.weight;
      }
      this._ratio = switchPointState1.data.weight / num2;
      if (this.reparentCamera && (double) this._ratio >= (double) this.ratioToApplyParentTransform)
        switchPointState1.Reparent(this.transform);
      if ((double) this.previousRatio >= (double) this._ratio)
      {
        switchPointState1.Apply(this.transform, (IEnumerable<Camera>) this.cameras);
        if (this.switchPointStates.Count > 1)
        {
          this.switchPointStates.Clear();
          this.switchPointStates.Add(switchPointState1);
        }
      }
      else
      {
        this.currentData.Zero();
        foreach (CameraSwitch.SwitchPointState switchPointState4 in this.switchPointStates)
        {
          switchPointState4.UpdateData();
          this.currentData.Add(switchPointState4.data);
        }
        this.currentData.ApplyData(this.transform, (IEnumerable<Camera>) this.cameras);
      }
      this.previousRatio = this._ratio;
      this.previousSwitchPoint = this.currentSwitchPoint;
    }
    else
    {
      switchPointState1.Apply(this.transform, (IEnumerable<Camera>) this.cameras);
      if (this.switchPointStates.Count <= 1)
        return;
      this.switchPointStates.Clear();
      this.switchPointStates.Add(switchPointState1);
    }
  }

  public void ApplyConfiguration() => CameraSwitch.fovMultiplier = CameraSwitch.readableFOV / 80f;

  public float GetStateWeight(CameraSwitchPoint switchPoint)
  {
    if ((Object) switchPoint == (Object) null)
      return 0.0f;
    if ((Object) this.currentSwitchPoint == (Object) switchPoint)
      return this._ratio;
    float num1 = 0.0f;
    float num2 = 0.0f;
    for (int index = 0; index < this.switchPointStates.Count; ++index)
    {
      float weight = this.switchPointStates[index].data.weight;
      num1 += weight;
      if ((Object) this.switchPointStates[index].switchPoint == (Object) switchPoint)
        num2 = weight;
    }
    return (double) num2 <= 0.0 || (double) num1 <= 0.0 ? 0.0f : num2 / num1;
  }

  public struct SwitchPointData
  {
    public Vector3 position;
    public Vector3 rotationForward;
    public Vector3 rotationUp;
    public float fov;
    public float weight;

    public SwitchPointData(Transform transform, float fov, float weight)
    {
      this.position = transform.position * weight;
      this.rotationForward = transform.rotation * new Vector3(0.0f, 0.0f, weight);
      this.rotationUp = transform.rotation * new Vector3(0.0f, weight, 0.0f);
      this.fov = fov * weight;
      this.weight = weight;
    }

    public SwitchPointData(Transform transform, float fov)
    {
      this.position = transform.position;
      this.rotationForward = transform.rotation * new Vector3(0.0f, 0.0f, 1f);
      this.rotationUp = transform.rotation * new Vector3(0.0f, 1f, 0.0f);
      this.fov = fov;
      this.weight = 1f;
    }

    public SwitchPointData(Vector3 position, Quaternion rotation, float fov, float weight)
    {
      this.position = position * weight;
      this.rotationForward = rotation * new Vector3(0.0f, 0.0f, weight);
      this.rotationUp = rotation * new Vector3(0.0f, weight, 0.0f);
      this.fov = fov * weight;
      this.weight = weight;
    }

    public SwitchPointData(Vector3 position, Quaternion rotation, float fov)
    {
      this.position = position;
      this.rotationForward = rotation * new Vector3(0.0f, 0.0f, 1f);
      this.rotationUp = rotation * new Vector3(0.0f, 1f, 0.0f);
      this.fov = fov;
      this.weight = 1f;
    }

    public Quaternion rotation
    {
      get => Quaternion.LookRotation(this.rotationForward, this.rotationUp);
      set
      {
        this.rotationForward = value * Vector3.forward;
        this.rotationUp = value * Vector3.up;
      }
    }

    public void Add(CameraSwitch.SwitchPointData other) => this.Add(other, other.weight);

    public void Add(CameraSwitch.SwitchPointData other, float weight)
    {
      this.position += other.position * weight;
      this.rotationForward += other.rotationForward * weight;
      this.rotationUp += other.rotationUp * weight;
      this.fov += other.fov * weight;
      weight += weight;
    }

    public void Deweight()
    {
      if ((double) this.weight != 0.0)
      {
        this.position /= this.weight;
        this.rotationForward = this.rotationForward.normalized;
        this.rotationUp = this.rotationUp.normalized;
        this.fov /= this.weight;
      }
      this.weight = 0.0f;
    }

    public void Zero()
    {
      this.position = Vector3.zero;
      this.rotationForward = Vector3.zero;
      this.rotationUp = Vector3.zero;
      this.fov = 0.0f;
      this.weight = 0.0f;
    }

    public void ApplyData(Transform transform, IEnumerable<Camera> cameras)
    {
      this.Deweight();
      transform.rotation = this.rotation;
      transform.position = this.position;
      foreach (Camera camera in cameras)
        camera.fieldOfView = this.fov * CameraSwitch.fovMultiplier;
    }

    public void ApplyData(IEnumerable<Transform> transforms, IEnumerable<Camera> cameras)
    {
      this.Deweight();
      foreach (Transform transform in transforms)
      {
        transform.rotation = this.rotation;
        transform.position = this.position;
      }
      foreach (Camera camera in cameras)
        camera.fieldOfView = this.fov * CameraSwitch.fovMultiplier;
    }

    public void ApplyData(IEnumerable<Camera> cameras)
    {
      this.Deweight();
      foreach (Camera camera in cameras)
      {
        camera.fieldOfView = this.fov * CameraSwitch.fovMultiplier;
        camera.transform.rotation = this.rotation;
        camera.transform.position = this.position;
      }
    }
  }

  public class SwitchPointState
  {
    public bool m_allowParent = true;
    public CameraSwitchPoint switchPoint;
    public CameraSwitch.SwitchPointData data;

    public SwitchPointState(CameraSwitchPoint switchPoint, CameraSwitch.SwitchPointData data)
    {
      this.switchPoint = switchPoint;
      this.data = data;
    }

    public SwitchPointState(CameraSwitchPoint switchPoint)
    {
      this.switchPoint = switchPoint;
      this.data = new CameraSwitch.SwitchPointData();
    }

    public bool used
    {
      get => (Object) this.switchPoint != (Object) null;
      set
      {
        if (!value)
          this.switchPoint = (CameraSwitchPoint) null;
        else
          this.LogError<CameraSwitch.SwitchPointState>("A switch point cannot be activated by setting it's used state to true - you must supply switchPoint with a reference.");
      }
    }

    public bool allowParent
    {
      get => this.m_allowParent && this.switchPoint.allowParent;
      set
      {
        this.m_allowParent = value;
        if (!value || this.switchPoint.allowParent)
          return;
        this.LogInfo<CameraSwitch.SwitchPointState>("Parenting internally is disabled on this switch point - modify switchPoint.allowParent.");
      }
    }

    public void UpdateData()
    {
      if (!((Object) this.switchPoint != (Object) null))
        return;
      if ((Object) this.switchPoint.transform != (Object) null)
      {
        this.data.position = this.switchPoint.transform.position;
        this.data.rotation = this.switchPoint.transform.rotation;
      }
      this.data.fov = this.switchPoint.fov;
    }

    public void Reparent(Transform transform)
    {
      if (!this.allowParent || !((Object) this.switchPoint != (Object) null))
        return;
      transform.parent = this.switchPoint.transform;
    }

    public void Apply(Transform transform, IEnumerable<Camera> cameras)
    {
      if ((Object) this.switchPoint != (Object) null)
      {
        if (this.allowParent)
          transform.parent = this.switchPoint.transform;
        if ((Object) this.switchPoint.transform != (Object) null)
        {
          transform.rotation = this.switchPoint.transform.rotation;
          transform.position = this.switchPoint.transform.position;
        }
        foreach (Camera camera in cameras)
          camera.fieldOfView = this.switchPoint.fov * CameraSwitch.fovMultiplier;
      }
      else
      {
        transform.rotation = this.data.rotation;
        transform.position = this.data.position;
        foreach (Camera camera in cameras)
          camera.fieldOfView = this.data.fov * CameraSwitch.fovMultiplier;
      }
    }

    public void Reparent(IEnumerable<Transform> transforms)
    {
      if ((Object) this.switchPoint == (Object) null || !this.allowParent)
        return;
      foreach (Transform transform in transforms)
        transform.parent = this.switchPoint.transform;
    }

    public void Reparent(IEnumerable<Camera> cameras)
    {
      if ((Object) this.switchPoint == (Object) null || !this.allowParent)
        return;
      foreach (Component camera in cameras)
        camera.transform.parent = this.switchPoint.transform;
    }

    public void Apply(IEnumerable<Transform> transforms, IEnumerable<Camera> cameras)
    {
      if ((Object) this.switchPoint != (Object) null)
      {
        foreach (Transform transform in transforms)
        {
          if (this.allowParent)
            transform.parent = this.switchPoint.transform;
          if ((Object) this.switchPoint.transform != (Object) null)
          {
            transform.rotation = this.switchPoint.transform.rotation;
            transform.position = this.switchPoint.transform.position;
          }
        }
        foreach (Camera camera in cameras)
          camera.fieldOfView = this.switchPoint.fov * CameraSwitch.fovMultiplier;
      }
      else
      {
        foreach (Transform transform in transforms)
        {
          if ((Object) this.switchPoint.transform != (Object) null)
          {
            transform.rotation = this.data.rotation;
            transform.position = this.data.position;
          }
        }
        foreach (Camera camera in cameras)
          camera.fieldOfView = this.data.fov * CameraSwitch.fovMultiplier;
      }
    }

    public void Apply(IEnumerable<Camera> cameras)
    {
      if ((Object) this.switchPoint != (Object) null)
      {
        foreach (Camera camera in cameras)
        {
          if (this.allowParent)
            camera.transform.parent = this.switchPoint.transform;
          if ((Object) this.switchPoint.transform != (Object) null)
          {
            camera.transform.rotation = this.switchPoint.transform.rotation;
            camera.transform.position = this.switchPoint.transform.position;
          }
          camera.fieldOfView = this.switchPoint.fov * CameraSwitch.fovMultiplier;
        }
      }
      else
      {
        foreach (Camera camera in cameras)
        {
          if ((Object) this.switchPoint.transform != (Object) null)
          {
            camera.transform.rotation = this.data.rotation;
            camera.transform.position = this.data.position;
          }
          camera.fieldOfView = this.data.fov * CameraSwitch.fovMultiplier;
        }
      }
    }
  }
}
