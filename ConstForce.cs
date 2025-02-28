// Decompiled with JetBrains decompiler
// Type: ConstForce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using PigeonCoopToolkit.Effects.Trails;
using UnityEngine;

#nullable disable
public class ConstForce : MonoBehaviour
{
  public float speed;

  private void Start()
  {
  }

  private void Update()
  {
    foreach (SmokePlume component in this.GetComponents<SmokePlume>())
      component.ConstantForce = this.transform.forward * this.speed;
  }
}
