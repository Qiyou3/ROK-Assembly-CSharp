// Decompiled with JetBrains decompiler
// Type: InstantiateOnKey
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class InstantiateOnKey : MonoBehaviour
{
  public Transform instantiateAt;
  public GameObject objectToInstantiate;
  public KeyCode keycode;

  public void Start()
  {
    if ((Object) this.objectToInstantiate == (Object) null)
    {
      this.LogError<InstantiateOnKey>("objectToInstantiate is null; disabling.");
      this.enabled = false;
    }
    else
    {
      if (!((Object) this.instantiateAt == (Object) null))
        return;
      this.LogInfo<InstantiateOnKey>("instantiateAt is null; setting to myself.");
      this.instantiateAt = this.transform;
    }
  }

  public void Update()
  {
    if ((Object) this.objectToInstantiate == (Object) null)
    {
      this.LogError<InstantiateOnKey>("objectToInstantiate is null; disabling.");
      this.enabled = false;
    }
    else
    {
      if ((Object) this.instantiateAt == (Object) null)
      {
        this.LogInfo<InstantiateOnKey>("instantiateAt is null; setting to myself.");
        this.instantiateAt = this.transform;
      }
      if (!Input.GetKeyDown(this.keycode))
        return;
      Object.Instantiate((Object) this.objectToInstantiate, this.transform.position, this.transform.rotation);
    }
  }
}
