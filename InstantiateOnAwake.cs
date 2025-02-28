// Decompiled with JetBrains decompiler
// Type: InstantiateOnAwake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class InstantiateOnAwake : MonoBehaviour
{
  public bool asChildren = true;
  public bool checkForDestroyIfAlreadyExists = true;
  public bool resetPosition = true;
  public bool resetRotation = true;
  public bool removeCloneFromName = true;
  public GameObject[] gameObjects;
  public Vector3 localPosition;
  public Vector3 localRotationEuler;

  public void Awake()
  {
    if (this.checkForDestroyIfAlreadyExists)
    {
      DestroyIfAlreadyExists component = this.GetComponent<DestroyIfAlreadyExists>();
      if ((Object) component != (Object) null && component.WillDestroy)
      {
        this.LogInfo<InstantiateOnAwake>("Not instantiating objects because this object is marked by DestroyIfAlreadyExists to destroy. Assumes you do not want multiple instantiations.");
        Object.Destroy((Object) this);
        this.enabled = false;
        return;
      }
    }
    Quaternion rotation = Quaternion.Euler(this.localRotationEuler);
    foreach (GameObject gameObject1 in this.gameObjects)
    {
      if ((Object) gameObject1 != (Object) null)
      {
        if (!this.resetPosition)
          this.localPosition = gameObject1.transform.position;
        if (!this.resetRotation)
          rotation = gameObject1.transform.rotation;
        GameObject gameObject2 = (GameObject) Object.Instantiate((Object) gameObject1, this.localPosition, rotation);
        if (this.asChildren)
          gameObject2.transform.parent = this.transform;
        if (this.removeCloneFromName)
          gameObject2.name = gameObject2.name.Replace("(Clone)", string.Empty);
      }
    }
  }
}
