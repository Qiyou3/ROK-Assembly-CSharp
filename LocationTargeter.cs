// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.LocationTargeter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class LocationTargeter : AIBehaviour
  {
    public float refreshInterval = 1f;
    public Vector3 distancePenalty = new Vector3(1f, 10f, 1f);
    public Transform measurePoint;
    private bool _started;
    private Targetable _previousTarget;

    private Transform MeasurePoint
    {
      get
      {
        if ((Object) this.measurePoint == (Object) null)
          this.measurePoint = this.Entity.GetOrCreate<CharacterDefinition>().GetTransform(CharacterDefinition.Part.EyesCenter);
        return this.measurePoint;
      }
    }

    public void Start()
    {
      this._started = true;
      this.StartCoroutineWithExceptionHandling(new CoroutineUtil.CoroutineDelegate(this.TargetingRoutine));
    }

    public void OnEnable()
    {
      if (!this._started)
        return;
      this.Start();
    }

    public void Update()
    {
      if ((Object) this._previousTarget != (Object) this.CurrentTarget)
      {
        if ((Object) this.CurrentTarget != (Object) null)
          this.RecalculateLocation();
        else
          this.RemoveCurrentLocation();
      }
      this._previousTarget = this.CurrentTarget;
    }

    [DebuggerHidden]
    public IEnumerator TargetingRoutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new LocationTargeter.\u003CTargetingRoutine\u003Ec__IteratorC8()
      {
        \u003C\u003Ef__this = this
      };
    }

    private void RecalculateLocation()
    {
      Collider closestCollider;
      Vector3 position = this.CurrentTarget.ClosestPointOnColliders(this.MeasurePoint.position, this.distancePenalty, out closestCollider);
      Transform parent = !((Object) closestCollider != (Object) null) ? this.CurrentTarget.Entity.MainTransform : closestCollider.transform;
      this.SetCurrentLocation(parent.InverseTransformPoint(position), parent);
    }
  }
}
