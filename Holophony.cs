// Decompiled with JetBrains decompiler
// Type: Holophony
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Holophony : MonoBehaviour
{
  public AudioSource right;
  public AudioSource left;
  public Transform rightEar;
  public Transform leftEar;
  public float speedOfSound = 343f;
  private float startTime;
  private float progressLeft;
  private float progressRight;
  private float alignMultLeftSmooth;
  private float alignMultRightSmooth;
  public float volumeAtUnitDistance = 10f;

  public void Start() => this.enabled = true;

  [ContextMenu("Play")]
  public void Play()
  {
    this.enabled = true;
    this.startTime = Time.fixedTime;
    this.left.Stop();
    this.right.Stop();
    this.progressRight = 0.0f;
    this.progressLeft = 0.0f;
  }

  public void FixedUpdate()
  {
    float num1 = (Time.fixedTime - this.startTime) * this.speedOfSound;
    float num2 = Vector3.Distance(this.transform.position, this.rightEar.position);
    float num3 = Vector3.Distance(this.transform.position, this.leftEar.position);
    float progressLeft = this.progressLeft;
    float progressRight = this.progressRight;
    this.progressRight = (num1 - num2) / this.speedOfSound;
    this.progressLeft = (num1 - num3) / this.speedOfSound;
    if ((double) this.progressLeft > 0.0 && (double) progressLeft <= 0.0 && !this.left.isPlaying)
      this.left.Play((ulong) ((double) this.progressLeft * 44100.0));
    float a = this.volumeAtUnitDistance / num3 / num3;
    float b = this.volumeAtUnitDistance / num2 / num2;
    float num4 = Mathf.Max(a, b);
    if ((double) num4 > 1.0)
    {
      a /= num4;
      b /= num4;
    }
    this.left.volume = a;
    this.right.volume = b;
    if (this.left.isPlaying)
    {
      float num5 = (this.progressLeft - progressLeft) / Time.fixedDeltaTime;
      float num6 = progressLeft - this.left.time;
      this.alignMultLeftSmooth += (float) (((double) (num6 - Mathf.Round(num6 / this.left.clip.length) * this.left.clip.length) - (double) this.alignMultLeftSmooth) * 0.10000000149011612);
      this.left.pitch = num5 * (1f + this.alignMultLeftSmooth);
    }
    if ((double) this.progressRight > 0.0 && (double) progressRight <= 0.0 && !this.right.isPlaying)
      this.right.Play((ulong) ((double) this.progressRight * 44100.0));
    if (!this.right.isPlaying)
      return;
    float num7 = (this.progressRight - progressRight) / Time.fixedDeltaTime;
    float num8 = progressRight - this.right.time;
    this.alignMultRightSmooth += (float) (((double) (num8 - Mathf.Round(num8 / this.right.clip.length) * this.right.clip.length) - (double) this.alignMultRightSmooth) * 0.10000000149011612);
    this.right.pitch = num7 * (1f + this.alignMultRightSmooth);
  }
}
