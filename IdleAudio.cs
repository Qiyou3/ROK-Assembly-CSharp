// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.IdleAudio
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class IdleAudio : MonoBehaviour
  {
    public AudioSource voice;
    public AudioClip[] idleSounds;
    public float interval = 5f;
    public float intervalVariation = 2f;
    private float _timer;

    public void Update() => this.UpdateMoaning();

    private void UpdateMoaning()
    {
      if ((Object) this.voice == (Object) null)
      {
        this.LogInfo<IdleAudio>("Requires voice AudioSource");
        this.enabled = false;
      }
      else
      {
        this._timer -= Time.deltaTime;
        if ((double) this._timer > 0.0)
          return;
        this.voice.clip = this.idleSounds.GetRandom<AudioClip>();
        this.voice.Play();
        this._timer = this.interval * Mathf.Pow(this.intervalVariation, Random.Range(-1f, 1f));
      }
    }
  }
}
