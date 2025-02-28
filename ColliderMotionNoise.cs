// Decompiled with JetBrains decompiler
// Type: ColliderMotionNoise
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
public class ColliderMotionNoise : MonoBehaviour
{
  public AudioSource audioSource;
  public AudioListener listener;
  private Vector3 previousLocalPosition;
  [HideInInspector]
  public float originalVolume;
  [HideInInspector]
  public float originalPitch;
  public float volumePower = 1f;
  public float pitchPower = 1f;
  public float volumeMultiplier = 1f;
  public float pitchMultiplier = 1f;
  public bool highQualityNearestPointSearch = true;
  public float moveTime = 0.5f;

  private void GetClosestPosition(bool instant)
  {
    float num = !instant ? 1f - Mathf.Pow(0.5f, Time.deltaTime / this.moveTime) : 1f;
    if (this.highQualityNearestPointSearch)
      this.audioSource.transform.position += (this.GetComponent<Collider>().GetClosest(this.listener.transform.position) - this.audioSource.transform.position) * num;
    else
      this.audioSource.transform.position += (this.GetComponent<Collider>().ClosestPointOnBounds(this.listener.transform.position) - this.audioSource.transform.position) * num;
  }

  public void Start()
  {
    this.originalVolume = this.audioSource.volume;
    this.originalPitch = this.audioSource.pitch;
    this.audioSource.volume = 0.0f;
    if ((Object) this.listener == (Object) null)
      this.listener = Object.FindObjectOfType(typeof (AudioListener)) as AudioListener;
    if ((Object) this.listener == (Object) null)
    {
      this.LogError<ColliderMotionNoise>("The scene does not seem to contain an AudioListener.");
      this.enabled = false;
    }
    else
    {
      this.GetClosestPosition(true);
      this.previousLocalPosition = this.listener.transform.InverseTransformPoint(this.audioSource.transform.position);
    }
  }

  public void LateUpdate()
  {
    this.GetClosestPosition(false);
    Vector3 vector3 = this.listener.transform.InverseTransformPoint(this.audioSource.transform.position);
    float magnitude1 = ((this.previousLocalPosition - vector3) / Time.deltaTime + vector3).magnitude;
    float magnitude2 = vector3.magnitude;
    float f = (double) magnitude1 != (double) magnitude2 ? ((double) magnitude1 <= (double) magnitude2 ? magnitude2 / magnitude1 : magnitude1 / magnitude2) : 1f;
    this.LogInfo<ColliderMotionNoise>("{0}", (object) f);
    this.audioSource.volume = (Mathf.Pow(f, this.volumePower) - 1f) * this.volumeMultiplier;
    this.audioSource.pitch = (Mathf.Pow(f, this.pitchPower) - 1f) * this.pitchMultiplier;
    this.previousLocalPosition = vector3;
  }
}
