// Decompiled with JetBrains decompiler
// Type: demo_scene_control
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class demo_scene_control : MonoBehaviour
{
  public GameObject gr_explosion;
  public GameObject mass_gr_explosion;
  public GameObject d_gr_explosion;
  public GameObject short_gr_explosion;
  public GameObject space_explosion;
  public GameObject short_space_explosion;
  public GameObject circle_explosion;
  public GameObject nuke_explosion;
  public GameObject flash_explosion;
  public GameObject huge_explosion;
  public GameObject smoke_explosion;
  private Transform spawn;
  private Transform n_spawn;
  private Transform dir_spawn;
  private Transform space_spawn;
  private Transform mass_spawn;
  private Transform spawn_smoke;

  private void Start()
  {
    this.spawn = GameObject.Find("spawn").transform;
    this.dir_spawn = GameObject.Find("dir_spawn").transform;
    this.space_spawn = GameObject.Find("space_spawn").transform;
    this.mass_spawn = GameObject.Find("mass_spawn").transform;
    this.spawn_smoke = GameObject.Find("spawn_smoke").transform;
    this.n_spawn = GameObject.Find("spawn_nuke").transform;
  }

  private void Update()
  {
  }

  private void OnGUI()
  {
    if (GUI.Button(new Rect(20f, 20f, 200f, 20f), "Ground Explosion"))
    {
      GameObject gameObject = Object.Instantiate((Object) this.gr_explosion, this.spawn.position, this.spawn.rotation) as GameObject;
      PanWM.shake_value = 1f;
      PanWM.shake_speed = 10f;
    }
    if (GUI.Button(new Rect(20f, 50f, 200f, 20f), "Flash Explosion"))
    {
      GameObject gameObject = Object.Instantiate((Object) this.flash_explosion, this.spawn.position, this.spawn.rotation) as GameObject;
      PanWM.shake_value = 1f;
      PanWM.shake_speed = 10f;
    }
    if (GUI.Button(new Rect(20f, 80f, 200f, 20f), "Massive Ground Explosion"))
    {
      GameObject gameObject = Object.Instantiate((Object) this.mass_gr_explosion, this.mass_spawn.position, this.mass_spawn.rotation) as GameObject;
      PanWM.shake_value = 1.5f;
      PanWM.shake_speed = 10f;
    }
    if (GUI.Button(new Rect(20f, 110f, 200f, 20f), "Directed Ground Explosion"))
    {
      GameObject gameObject = Object.Instantiate((Object) this.d_gr_explosion, this.dir_spawn.position, this.dir_spawn.rotation) as GameObject;
      PanWM.shake_value = 0.7f;
      PanWM.shake_speed = 10f;
    }
    if (GUI.Button(new Rect(20f, 140f, 200f, 20f), "Ground Short Explosion"))
    {
      GameObject gameObject = Object.Instantiate((Object) this.short_gr_explosion, this.spawn.position, this.spawn.rotation) as GameObject;
      PanWM.shake_value = 0.7f;
      PanWM.shake_speed = 10f;
    }
    if (GUI.Button(new Rect(20f, 170f, 200f, 20f), "Space (No Gravity) Explosion"))
    {
      GameObject gameObject = Object.Instantiate((Object) this.space_explosion, this.space_spawn.position, this.space_spawn.rotation) as GameObject;
      PanWM.shake_value = 1f;
      PanWM.shake_speed = 10f;
    }
    if (GUI.Button(new Rect(20f, 200f, 200f, 20f), "Space Short Explosion"))
    {
      GameObject gameObject = Object.Instantiate((Object) this.short_space_explosion, this.space_spawn.position, this.space_spawn.rotation) as GameObject;
      PanWM.shake_value = 0.7f;
      PanWM.shake_speed = 10f;
    }
    if (GUI.Button(new Rect(20f, 230f, 200f, 20f), "Circle Explosion"))
    {
      GameObject gameObject = Object.Instantiate((Object) this.circle_explosion, this.spawn.position, this.spawn.rotation) as GameObject;
      PanWM.shake_value = 0.5f;
      PanWM.shake_speed = 10f;
    }
    if (GUI.Button(new Rect(20f, 260f, 200f, 20f), "Huge Explosion"))
    {
      GameObject gameObject = Object.Instantiate((Object) this.huge_explosion, this.n_spawn.position, this.n_spawn.rotation) as GameObject;
      PanWM.shake_value = 2f;
      PanWM.shake_speed = 5f;
    }
    if (GUI.Button(new Rect(20f, 290f, 200f, 20f), "Smoke Explosion"))
    {
      GameObject gameObject = Object.Instantiate((Object) this.smoke_explosion, this.spawn_smoke.position, this.spawn_smoke.rotation) as GameObject;
      PanWM.shake_value = 2f;
      PanWM.shake_speed = 5f;
    }
    if (!GUI.Button(new Rect(20f, 320f, 200f, 20f), "NUKE"))
      return;
    GameObject gameObject1 = Object.Instantiate((Object) this.nuke_explosion, this.n_spawn.position, this.n_spawn.rotation) as GameObject;
    PanWM.shake_value = 2f;
    PanWM.shake_speed = 5f;
  }
}
