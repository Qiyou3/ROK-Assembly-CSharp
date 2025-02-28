// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.AddDragOnCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class AddDragOnCollision : EntityBehaviour
  {
    public float AngularDrag = 50f;
    public float Drag = 150f;
    private bool setKinimatic = true;

    public void Start()
    {
    }

    public void OnCollisionStay(Collision col)
    {
      if (col.gameObject.layer != LayerMask.NameToLayer("Terrain") && col.gameObject.layer != LayerMask.NameToLayer("Blocks"))
        return;
      EntityRigidbodyManager rigidbodyManager = this.Entity.TryGet<EntityRigidbodyManager>();
      if ((Object) rigidbodyManager == (Object) null)
      {
        if ((Object) (Rigidbody) (GameObjectAttribute<Rigidbody>) this.Entity.Get<MainRigidbody>() == (Object) null)
        {
          Logger.ErrorFormat("Could not find EntityRigidbodyManager or MainRigidbody on {0} to apply drag to.", (object) this.Entity.name);
          return;
        }
        this.Entity.Get<MainRigidbody>().GetComponent<Rigidbody>().angularDrag = this.AngularDrag;
        this.Entity.Get<MainRigidbody>().GetComponent<Rigidbody>().drag = this.Drag;
        this.enabled = false;
      }
      Rigidbody[] rigidbodies = rigidbodyManager.MyRigidbodies.Rigidbodies;
      if (rigidbodies.Length != 1)
        return;
      for (int index = 0; index < rigidbodies.Length; ++index)
      {
        rigidbodies[index].angularDrag = this.AngularDrag;
        rigidbodies[index].drag = this.Drag;
        if (this.setKinimatic)
          rigidbodies[index].isKinematic = true;
        this.enabled = false;
      }
    }
  }
}
