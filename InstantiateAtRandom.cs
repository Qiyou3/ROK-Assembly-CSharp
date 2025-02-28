// Decompiled with JetBrains decompiler
// Type: InstantiateAtRandom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class InstantiateAtRandom : MonoBehaviour
{
  public Transform instantiateAt;
  public float randomRange = 10f;
  public GameObject[] objectsToInstantiate;
  public float minInterval = 300f;
  public float maxInterval = 2000f;
  public KeyCode keyCode;
  private float timer = 300f;

  public void Start()
  {
    if (this.objectsToInstantiate == null)
    {
      this.LogError<InstantiateAtRandom>("objectsToInstantiate is null; disabling.");
      this.enabled = false;
    }
    else if (this.objectsToInstantiate.Length == 0)
    {
      this.LogError<InstantiateAtRandom>("objectsToInstantiate is empty; disabling.");
      this.enabled = false;
    }
    else
    {
      if ((Object) this.instantiateAt == (Object) null)
      {
        this.LogInfo<InstantiateAtRandom>("instantiateAt is null; setting to myself.");
        this.instantiateAt = this.transform;
      }
      this.timer = Random.Range(this.minInterval, this.maxInterval);
    }
  }

  public void Update()
  {
    if (this.objectsToInstantiate == null)
    {
      this.LogError<InstantiateAtRandom>("objectsToInstantiate is null; disabling.");
      this.enabled = false;
    }
    else if (this.objectsToInstantiate.Length == 0)
    {
      this.LogError<InstantiateAtRandom>("objectsToInstantiate is empty; disabling.");
      this.enabled = false;
    }
    else
    {
      if ((Object) this.instantiateAt == (Object) null)
      {
        this.LogInfo<InstantiateAtRandom>("instantiateAt is null; setting to myself.");
        this.instantiateAt = this.transform;
      }
      this.timer -= Time.deltaTime;
      if (Input.GetKeyDown(this.keyCode))
        this.timer = 0.0f;
      if ((double) this.timer > 0.0)
        return;
      this.Spawn();
      this.timer = Random.Range(this.minInterval, this.maxInterval);
    }
  }

  private void Spawn()
  {
    Vector3 vector3 = (Random.insideUnitSphere * this.randomRange) with
    {
      y = 0.0f
    };
    GameObject original = this.objectsToInstantiate[Random.Range(0, this.objectsToInstantiate.Length)];
    Object.Instantiate((Object) original, this.transform.position + vector3, original.transform.rotation);
  }
}
