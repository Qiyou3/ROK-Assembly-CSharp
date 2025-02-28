// Decompiled with JetBrains decompiler
// Type: BirdsBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BirdsBehaviour : MonoBehaviour
{
  public Transform birdsPrefab;
  private float birdTimer;
  private ParticleAnimator animator;
  private ParticleEmitter emitter;

  public void Start()
  {
    if (QualitySettings.GetQualityLevel() < 3)
      this.enabled = false;
    else
      this.birdTimer = (float) Random.Range(2, 5);
  }

  public void Update()
  {
    if ((double) this.birdTimer >= (double) Time.time)
      return;
    this.StartBirds();
  }

  private void StartBirds()
  {
    Transform transform = (Transform) Object.Instantiate((Object) this.birdsPrefab, this.transform.position, this.transform.rotation);
    this.animator = transform.GetComponentInChildren(typeof (ParticleAnimator)) as ParticleAnimator;
    this.animator.force = new Vector3(0.0f, Random.Range(-0.3f, 0.3f), 0.0f);
    this.emitter = transform.GetComponentInChildren(typeof (ParticleEmitter)) as ParticleEmitter;
    this.emitter.emit = true;
    this.birdTimer = Time.time + (float) Random.Range(5, 20);
  }
}
