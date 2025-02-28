// Decompiled with JetBrains decompiler
// Type: FuelTank
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FuelTank : MonoBehaviour
{
  private Rigidbody fuelTank;
  private BoxCollider boxCollider;
  private Drivetrain drivetrain;
  private Transform myTransform;
  public float _tankCapacity = 50f;
  public float _currentFuel = 50f;
  public float _tankWeight = 10f;
  public float _fuelDensity = 0.73722f;

  public float tankWeight
  {
    get => this._tankWeight;
    set
    {
      if ((double) value <= 0.0)
        this._tankWeight = 1f / 1000f;
      else
        this._tankWeight = value;
    }
  }

  public float tankCapacity
  {
    get => this._tankCapacity;
    set
    {
      this._tankCapacity = (double) value >= 0.0 ? value : 0.0f;
      if ((double) this._currentFuel <= (double) this._tankCapacity)
        return;
      this._currentFuel = this._tankCapacity;
    }
  }

  public float currentFuel
  {
    get => this._currentFuel;
    set
    {
      if ((double) value < 0.0)
        this._currentFuel = 0.0f;
      else if ((double) value > (double) this.tankCapacity)
        this._currentFuel = this.tankCapacity;
      else
        this._currentFuel = value;
    }
  }

  public float fuelDensity
  {
    get => this._fuelDensity;
    set
    {
      if ((double) value < 0.0)
        this._fuelDensity = 0.0f;
      else
        this._fuelDensity = value;
    }
  }

  public void Start()
  {
    this.myTransform = this.transform;
    Transform parent = this.myTransform.parent;
    while ((Object) parent.GetComponent<Drivetrain>() == (Object) null)
      parent = parent.parent;
    this.drivetrain = parent.GetComponent<Drivetrain>();
    this.tankCapacity = this.tankCapacity;
    this.currentFuel = this.currentFuel;
    this.fuelDensity = this.fuelDensity;
    this.myTransform.gameObject.layer = this.drivetrain.transform.gameObject.layer;
    this.fuelTank = this.myTransform.gameObject.GetComponent<Rigidbody>();
    if ((Object) this.fuelTank == (Object) null)
      this.fuelTank = this.myTransform.gameObject.AddComponent<Rigidbody>();
    this.boxCollider = this.myTransform.gameObject.GetComponent<BoxCollider>();
    if ((Object) this.boxCollider == (Object) null)
    {
      this.boxCollider = this.myTransform.gameObject.AddComponent<BoxCollider>();
      this.boxCollider.size = new Vector3(0.7f, 0.25f, 0.6f);
    }
    this.fuelTank.drag = 0.0f;
    this.fuelTank.angularDrag = 0.0f;
    this.fuelTank.useGravity = true;
    this.fuelTank.isKinematic = false;
    ConfigurableJoint configurableJoint = this.fuelTank.GetComponent<ConfigurableJoint>();
    if ((Object) configurableJoint == (Object) null)
    {
      configurableJoint = this.fuelTank.gameObject.AddComponent<ConfigurableJoint>();
      configurableJoint.xMotion = ConfigurableJointMotion.Locked;
      configurableJoint.yMotion = ConfigurableJointMotion.Locked;
      configurableJoint.zMotion = ConfigurableJointMotion.Locked;
      configurableJoint.angularXMotion = ConfigurableJointMotion.Locked;
      configurableJoint.angularYMotion = ConfigurableJointMotion.Locked;
      configurableJoint.angularZMotion = ConfigurableJointMotion.Locked;
    }
    configurableJoint.connectedBody = this.drivetrain.transform.GetComponent<Rigidbody>();
  }

  public void FixedUpdate()
  {
    if ((double) this.tankCapacity > 0.0 && (double) this.currentFuel >= 0.0 && (double) this.drivetrain.rpm >= 20.0)
    {
      this.currentFuel -= (float) ((double) this.drivetrain.istantConsumption * (double) Time.deltaTime * (1.0 / (double) this.drivetrain.fuelTanks.Length));
      this.currentFuel = Mathf.Clamp(this.currentFuel, 0.0f, this.currentFuel);
    }
    this.fuelTank.mass = this.currentFuel * this.fuelDensity + this.tankWeight;
  }
}
