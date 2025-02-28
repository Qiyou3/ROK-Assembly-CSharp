// Decompiled with JetBrains decompiler
// Type: GhostOriginal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GhostOriginal : MonoBehaviour
{
  public GameObject character;
  public Vector3 offset;

  public void Synch()
  {
    foreach (AnimationState animationState1 in this.character.GetComponent<Animation>())
    {
      AnimationState animationState2 = this.GetComponent<Animation>()[animationState1.name];
      if ((TrackedReference) animationState2 == (TrackedReference) null)
      {
        this.GetComponent<Animation>().AddClip(animationState1.clip, animationState1.name);
        animationState2 = this.GetComponent<Animation>()[animationState1.name];
      }
      if (animationState2.enabled != animationState1.enabled)
      {
        animationState2.wrapMode = animationState1.wrapMode;
        animationState2.enabled = animationState1.enabled;
        animationState2.speed = animationState1.speed;
      }
      animationState2.weight = animationState1.weight;
      animationState2.time = animationState1.time;
    }
  }

  public void LateUpdate()
  {
    this.transform.position = this.character.transform.position + this.offset;
    this.transform.rotation = this.character.transform.rotation;
  }
}
