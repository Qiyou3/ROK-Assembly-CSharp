// Decompiled with JetBrains decompiler
// Type: DetectNearbyObjects`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Common.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof (SphereCollider))]
public class DetectNearbyObjects<T> : MonoBehaviour where T : class
{
  public float Range = 10f;
  private float _prevRange = 10f;
  private SphereCollider _sphereCollider;
  private bool _performFullScanNextUpdate = true;
  [NoEdit]
  public List<T> NearbyObjects = new List<T>();
  private List<DetectNearbyObjects<T>.ObjectColliders> _objectColliders;

  public event DetectNearbyObjects<T>.ObjectChangeHandler OnObjectAdded;

  public event DetectNearbyObjects<T>.ObjectChangeHandler OnObjectRemoved;

  public event System.Action OnObjectAddedSimple;

  public event System.Action OnObjectRemovedSimple;

  public void Awake()
  {
    this._objectColliders = new List<DetectNearbyObjects<T>.ObjectColliders>();
    this._sphereCollider = this.GetComponent<SphereCollider>();
    this.GetComponent<Rigidbody>().useGravity = false;
    this.GetComponent<Rigidbody>().isKinematic = true;
    this.UpdateColliderForRange();
  }

  public void Update()
  {
    if (this._performFullScanNextUpdate)
    {
      this.FullScan();
      this._performFullScanNextUpdate = false;
    }
    if ((double) this.Range == (double) this._prevRange)
      return;
    this._prevRange = this.Range;
    this.UpdateColliderForRange();
  }

  public void OnDisable()
  {
    this.RemoveAllObjects();
    this._performFullScanNextUpdate = true;
  }

  public void OnDestroy()
  {
    this.RemoveAllObjects();
    this.OnObjectAdded = (DetectNearbyObjects<T>.ObjectChangeHandler) null;
    this.OnObjectRemoved = (DetectNearbyObjects<T>.ObjectChangeHandler) null;
    this.OnObjectAddedSimple = (System.Action) null;
    this.OnObjectRemovedSimple = (System.Action) null;
  }

  private void UpdateColliderForRange() => this._sphereCollider.radius = this.Range;

  public virtual void OnTriggerEnter(Collider other)
  {
    T @object = other.gameObject.FirstImplementingAncestor<T>();
    if ((object) @object == null)
      return;
    this.AddObject(@object, other);
  }

  public virtual void OnTriggerExit(Collider other)
  {
    T @object = other.gameObject.FirstImplementingAncestor<T>();
    if ((object) @object != null)
      this.RemoveObject(@object, other);
    if (!this.NearbyObjects.Contains(@object))
      return;
    this.StartCoroutine(this.SingleScan(@object));
  }

  private void FullScan()
  {
    foreach (Collider collider in Physics.OverlapSphere(this.transform.position, this.Range))
    {
      T @object = collider.gameObject.FirstImplementingAncestor<T>();
      if ((object) @object != null)
        this.AddObject(@object, collider);
    }
  }

  private void AddObject(T @object, Collider collider)
  {
    bool flag = false;
    for (int index = this._objectColliders.Count - 1; index >= 0; --index)
    {
      if ((object) this._objectColliders[index].Object == (object) @object)
      {
        this._objectColliders[index].Colliders.AddDistinct<Collider>(collider);
        flag = true;
        break;
      }
    }
    if (flag)
      return;
    DetectNearbyObjects<T>.ObjectColliders objectColliders = new DetectNearbyObjects<T>.ObjectColliders(@object);
    objectColliders.Colliders.AddDistinct<Collider>(collider);
    this._objectColliders.Add(objectColliders);
    this.AddObject(@object);
  }

  private void AddObject(T @object)
  {
    Component child = (object) @object as Component;
    if ((UnityEngine.Object) child != (UnityEngine.Object) null)
      child.GetOrAddComponent<NotifyOnDestroy>().OnDestroyEvent += new NotifyOnDestroy.OnDestroyHandler(this.ResourceDestroyedWhileWithinTrigger);
    this.NearbyObjects.Add(@object);
    if (this.OnObjectAdded != null)
      this.OnObjectAdded(@object);
    if (this.OnObjectAddedSimple == null)
      return;
    this.OnObjectAddedSimple();
  }

  private void RemoveObject(T @object, Collider collider)
  {
    bool flag = false;
    for (int index = this._objectColliders.Count - 1; index >= 0; --index)
    {
      if ((object) this._objectColliders[index].Object == (object) @object)
      {
        this._objectColliders[index].Colliders.Remove(collider);
        if (this._objectColliders[index].Colliders.Count == 0)
        {
          this._objectColliders.RemoveAt(index);
          flag = true;
          break;
        }
        break;
      }
    }
    if (!flag)
      return;
    this.RemoveObject(@object);
  }

  private void RemoveObject(T @object)
  {
    Component component1 = (object) @object as Component;
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      NotifyOnDestroy component2 = component1.gameObject.GetComponent<NotifyOnDestroy>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        component2.OnDestroyEvent -= new NotifyOnDestroy.OnDestroyHandler(this.ResourceDestroyedWhileWithinTrigger);
    }
    this.NearbyObjects.Remove(@object);
    if (this.OnObjectRemoved != null)
      this.OnObjectRemoved(@object);
    if (this.OnObjectRemovedSimple == null)
      return;
    this.OnObjectRemovedSimple();
  }

  private void RemoveAllObjects()
  {
    for (int index = this.NearbyObjects.Count - 1; index >= 0; --index)
      this.RemoveObject(this.NearbyObjects[index]);
    this._objectColliders.Clear();
    if (this.NearbyObjects.Count == 0)
      return;
    this.LogError<DetectNearbyObjects<T>>("Failed to remove all objects");
    this.NearbyObjects.Clear();
  }

  private void ResourceDestroyedWhileWithinTrigger(GameObject gameObjectDestroyed)
  {
    this.RemoveObject(gameObjectDestroyed.GetImplementor<T>());
  }

  [DebuggerHidden]
  private IEnumerator SingleScan(T @object)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new DetectNearbyObjects<T>.\u003CSingleScan\u003Ec__Iterator7E()
    {
      @object = @object,
      \u003C\u0024\u003Eobject = @object,
      \u003C\u003Ef__this = this
    };
  }

  private class ObjectColliders
  {
    public ObjectColliders(T @object)
    {
      this.Object = @object;
      this.Colliders = new List<Collider>();
    }

    public T Object { get; private set; }

    public List<Collider> Colliders { get; private set; }
  }

  public delegate void ObjectChangeHandler(T resource);
}
