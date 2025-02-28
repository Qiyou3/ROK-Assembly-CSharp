// Decompiled with JetBrains decompiler
// Type: DamageIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Players;
using System;
using UnityEngine;

#nullable disable
public class DamageIndicator : MonoBehaviour
{
  public DamageIndicator.Type type;
  public float damage;
  public float damageForAlphaRange = 10f;
  public AnimationCurve damageForAlpha;
  public Transform damageDirection;
  public Vector3 damageDirectionVec;
  public Vector3 relativePoint;
  public float minBias = 0.5f;
  public float maxBias = 0.5f;
  public float fadeSubtractRate = 5f;
  public float fadeMultiplyRate = 0.75f;

  public void Start()
  {
    EventManager.Subscribe<PlayerRespawnEvent>(new EventSubscriber<PlayerRespawnEvent>(this.OnPlayerRespawn));
  }

  public void OnDestroy()
  {
    EventManager.Unsubscribe<PlayerRespawnEvent>(new EventSubscriber<PlayerRespawnEvent>(this.OnPlayerRespawn));
  }

  private void OnPlayerRespawn(PlayerRespawnEvent e)
  {
    if (!e.Player.IsLocal)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void Update()
  {
    Camera main = Camera.main;
    if ((bool) (UnityEngine.Object) main)
      this.transform.position = main.transform.position;
    this.damage *= Mathf.Pow(this.fadeMultiplyRate, Time.deltaTime);
    this.damage -= this.fadeSubtractRate * Time.deltaTime;
    Renderer component = this.GetComponent<Renderer>();
    if ((UnityEngine.Object) this.GetComponent<Renderer>() != (UnityEngine.Object) null)
    {
      Material material = component.material;
      if ((UnityEngine.Object) material != (UnityEngine.Object) null)
      {
        Color color = material.GetColor("_TintColor") with
        {
          a = this.damageForAlpha.Evaluate(this.damage / this.damageForAlphaRange)
        };
        material.SetColor("_TintColor", color);
      }
    }
    if ((double) this.damage >= 0.0)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void OnWillRenderObject()
  {
    Camera current = Camera.current;
    Vector3 forward = Vector3.forward with
    {
      z = 1f,
      y = Mathf.Tan((float) ((double) current.fieldOfView * (Math.PI / 180.0) / 2.0))
    };
    forward.x = forward.y * current.aspect;
    Vector3 from1 = current.transform.TransformDirection(new Vector3(forward.x, forward.y, forward.z));
    Vector3 from2 = current.transform.TransformDirection(new Vector3(-forward.x, forward.y, forward.z));
    Vector3 from3 = current.transform.TransformDirection(new Vector3(forward.x, -forward.y, forward.z));
    Vector3 from4 = current.transform.TransformDirection(new Vector3(-forward.x, -forward.y, forward.z));
    Vector3 vector3_1;
    if ((UnityEngine.Object) this.damageDirection != (UnityEngine.Object) null)
    {
      switch (this.type)
      {
        case DamageIndicator.Type.Directional:
          this.damageDirectionVec = this.damageDirection.forward;
          vector3_1 = -this.damageDirectionVec;
          break;
        case DamageIndicator.Type.Point:
          this.damageDirectionVec = this.damageDirection.TransformPoint(this.relativePoint);
          vector3_1 = -(this.damageDirectionVec - current.transform.position).normalized;
          break;
        default:
          this.damageDirectionVec = this.damageDirection.forward;
          vector3_1 = -this.damageDirection.forward;
          break;
      }
    }
    else
    {
      switch (this.type)
      {
        case DamageIndicator.Type.Directional:
          vector3_1 = -this.damageDirectionVec;
          break;
        case DamageIndicator.Type.Point:
          vector3_1 = -(this.damageDirectionVec - current.transform.position).normalized;
          break;
        default:
          vector3_1 = -this.damageDirectionVec;
          break;
      }
    }
    Vector3 vector3_2 = Vector3.RotateTowards(current.transform.forward, vector3_1, (float) ((double) Mathf.Max(current.fieldOfView, current.fieldOfView * current.aspect) / 2.0 * (Math.PI / 180.0)), float.PositiveInfinity);
    Vector3 viewportPoint = current.WorldToViewportPoint(current.transform.position + vector3_2);
    viewportPoint.x = Mathf.Clamp01(viewportPoint.x);
    viewportPoint.y = Mathf.Clamp01(viewportPoint.y);
    Vector3 direction1 = current.ViewportPointToRay(viewportPoint).direction;
    Vector3 vector3_3 = Vector3.RotateTowards(current.transform.forward, -vector3_1, (float) ((double) Mathf.Max(current.fieldOfView, current.fieldOfView * current.aspect) / 2.0 * (Math.PI / 180.0)), float.PositiveInfinity);
    viewportPoint = current.WorldToViewportPoint(current.transform.position + vector3_3);
    viewportPoint.x = Mathf.Clamp01(viewportPoint.x);
    viewportPoint.y = Mathf.Clamp01(viewportPoint.y);
    Vector3 direction2 = current.ViewportPointToRay(viewportPoint).direction;
    float num1 = Mathf.Min(180f, Vector3.Angle(direction1, vector3_1), Vector3.Angle(from1, vector3_1), Vector3.Angle(from2, vector3_1), Vector3.Angle(from3, vector3_1), Vector3.Angle(from4, vector3_1));
    float num2 = Mathf.Max(0.0f, Vector3.Angle(direction2, vector3_1), Vector3.Angle(from1, vector3_1), Vector3.Angle(from2, vector3_1), Vector3.Angle(from3, vector3_1), Vector3.Angle(from4, vector3_1));
    float num3 = num1 + (0.0f - num1) * this.minBias;
    float num4 = num2 + (180f - num2) * this.maxBias;
    float num5 = num3 / 180f;
    float x1 = (float) (1.0 / ((double) (num4 / 180f) - (double) num5));
    float x2 = -(num5 * x1);
    this.GetComponent<Renderer>().material.mainTextureScale = new Vector2(x1, this.GetComponent<Renderer>().material.mainTextureScale.y);
    this.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(x2, this.GetComponent<Renderer>().material.mainTextureOffset.y);
    this.transform.SetForward(vector3_1);
    this.transform.position = current.transform.position;
  }

  public enum Type
  {
    Directional,
    Point,
  }
}
