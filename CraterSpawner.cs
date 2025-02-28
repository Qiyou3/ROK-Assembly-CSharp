// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.CraterSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using FullInspector;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class CraterSpawner : BaseBehavior
  {
    [UnityEngine.Tooltip("Radius of the crater at its widest.")]
    public float radius = 150f;
    [UnityEngine.Tooltip("Length in radii of the crater.")]
    public float length = 300f;
    [UnityEngine.Tooltip("Centre point of the crater.")]
    public Transform craterCentre;

    public void OnDrawGizmos()
    {
      if (!((Object) this.craterCentre != (Object) null))
        return;
      float radius = this.radius / 2f;
      Gizmos.DrawWireSphere(this.craterCentre.position, radius);
      Vector3 vector3 = this.craterCentre.position - this.craterCentre.forward.normalized * this.length * this.radius;
      Gizmos.DrawWireSphere(vector3, radius / 5f);
      Gizmos.DrawLine(this.craterCentre.position, vector3);
      Gizmos.DrawLine(this.craterCentre.position + Vector3.up * radius, vector3 + Vector3.up * (radius / 5f));
      Gizmos.DrawLine(this.craterCentre.position - Vector3.up * radius, vector3 - Vector3.up * (radius / 5f));
      Gizmos.DrawLine(this.craterCentre.position + Vector3.left * radius, vector3 + Vector3.left * (radius / 5f));
      Gizmos.DrawLine(this.craterCentre.position - Vector3.left * radius, vector3 - Vector3.left * (radius / 5f));
    }
  }
}
