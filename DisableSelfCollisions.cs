// Decompiled with JetBrains decompiler
// Type: DisableSelfCollisions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DisableSelfCollisions : MonoBehaviour
{
  public void Start() => this.OnEnable();

  public void OnEnable() => this.Disable();

  [ContextMenu("Disable")]
  public void Disable()
  {
    Collider[] componentsInChildren = this.GetComponentsInChildren<Collider>(true);
    for (int index1 = 0; index1 < componentsInChildren.Length; ++index1)
    {
      if (!((Object) componentsInChildren[index1] == (Object) null) && componentsInChildren[index1].enabled && componentsInChildren[index1].gameObject.activeInHierarchy)
      {
        for (int index2 = index1 + 1; index2 < componentsInChildren.Length; ++index2)
        {
          if (!((Object) componentsInChildren[index2] == (Object) null) && componentsInChildren[index2].enabled && componentsInChildren[index2].gameObject.activeInHierarchy)
            Physics.IgnoreCollision(componentsInChildren[index1], componentsInChildren[index2]);
        }
      }
    }
  }
}
