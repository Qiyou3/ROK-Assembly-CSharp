// Decompiled with JetBrains decompiler
// Type: CenterJointAnchors
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CenterJointAnchors : MonoBehaviour
{
  public bool affectChildren = true;

  public void Start()
  {
    if (this.affectChildren)
    {
      foreach (Joint componentsInChild in this.GetComponentsInChildren<Joint>())
        componentsInChild.anchor = componentsInChild.GetComponent<Rigidbody>().centerOfMass;
    }
    else
    {
      foreach (Joint component in this.GetComponents<Joint>())
        component.anchor = component.GetComponent<Rigidbody>().centerOfMass;
    }
  }
}
