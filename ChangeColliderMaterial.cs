// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.ChangeColliderMaterial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Behaviours;
using CodeHatch.Networking.Events;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class ChangeColliderMaterial : IEventAction
  {
    public Collider[] Colliders;
    public PhysicMaterial Material;

    public void Execute(MonoBehaviour caller, BaseEvent e)
    {
      for (int index = 0; index < this.Colliders.Length; ++index)
      {
        Collider collider = this.Colliders[index];
        if ((Object) collider == (Object) null)
          caller.LogInfo<MonoBehaviour>("collider[{0}] == null", (object) index);
        else
          collider.material = this.Material;
      }
    }
  }
}
