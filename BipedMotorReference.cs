// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.BipedMotorReference
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class BipedMotorReference : EntityBehaviour
  {
    private const float _minMoveDistance = 0.1f;
    public CapsuleCollider colliderToMatch;
    public float SlopeLimit = 90f;
    public float StepOffset = 0.5f;
    public float Shrink;
    private CharacterController _reference;
    private GameObject _referenceGameObject;
    private Transform _transformToMatch;
    private Transform _referenceTransform;
    private Collider[] _childColliders;
    private GameObject[] _childColliderGameObjects;
    private int[] _childColliderLayers;

    public void Awake()
    {
      this._referenceGameObject = new GameObject("[Reference Character Controller]");
      this._referenceGameObject.layer = this.colliderToMatch.gameObject.layer;
      this._referenceTransform = this._referenceGameObject.transform;
      this._referenceTransform.parent = this.transform;
      this._reference = this._referenceGameObject.AddComponent<CharacterController>();
      this._reference.radius = this.colliderToMatch.radius - this.Shrink;
      this._reference.height = this.colliderToMatch.height - this.Shrink * 2f;
      this._reference.center = this.colliderToMatch.center;
      this._reference.slopeLimit = this.SlopeLimit;
      this._reference.stepOffset = this.StepOffset;
    }

    public void Start()
    {
      if (!this.Entity.IsLocallyOwned)
      {
        Object.Destroy((Object) this._referenceGameObject);
        Object.Destroy((Object) this);
        this.enabled = false;
      }
      else
      {
        this._childColliders = this.GetComponentsInChildren<Collider>();
        this._childColliderGameObjects = new GameObject[this._childColliders.Length];
        this._childColliderLayers = new int[this._childColliders.Length];
        for (int index = 0; index < this._childColliders.Length; ++index)
        {
          this._childColliderGameObjects[index] = this._childColliders[index].gameObject;
          this._childColliderLayers[index] = this._childColliderGameObjects[index].layer;
        }
        this._transformToMatch = this.colliderToMatch.transform;
      }
    }

    public void Move(ref Vector3 offset)
    {
      if (!this.enabled || (double) offset.magnitude <= 0.10000000149011612)
        return;
      Vector3 position = this._transformToMatch.position;
      this._referenceTransform.position = position;
      this._referenceTransform.rotation = this._transformToMatch.rotation;
      this._reference.enabled = true;
      for (int index = 0; index < this._childColliders.Length; ++index)
        ColliderUtil.TryIgnoreCollision((Collider) this._reference, this._childColliders[index]);
      int num = (int) this._reference.Move(offset);
      this._reference.enabled = false;
      offset = this._referenceTransform.position - position;
    }
  }
}
