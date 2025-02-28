// Decompiled with JetBrains decompiler
// Type: GoreStretch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (MeshRenderer))]
public class GoreStretch : MonoBehaviour
{
  private const float _timeForDistance = 2f;
  public Transform TopTransform;
  public Transform BottomTransform;
  public AnimationCurve StretchToPower;
  public AnimationCurve StretchToAlpha;
  private Material _material;
  private float _stretchDistance;

  public void OnEnable() => this._stretchDistance = 0.0f;

  public void Update()
  {
    if ((Object) this.TopTransform == (Object) null || (Object) this.BottomTransform == (Object) null)
      return;
    if ((Object) this._material == (Object) null)
    {
      this._material = !Application.isPlaying ? this.gameObject.GetComponent<MeshRenderer>().sharedMaterial : this.gameObject.GetComponent<MeshRenderer>().material;
      if ((Object) this._material == (Object) null)
        return;
    }
    float b = Vector3.Distance(this.TopTransform.position, this.BottomTransform.position);
    this._stretchDistance = !Application.isPlaying ? b : Mathf.Max(this._stretchDistance + Time.deltaTime * 2f, b);
    this._material.SetFloat("_AlphaPower", this.StretchToPower.Evaluate(this._stretchDistance));
    this._material.SetFloat("_AlphaMultiplier", this.StretchToAlpha.Evaluate(this._stretchDistance));
  }

  public void OnWillRenderObject()
  {
    if ((Object) this.TopTransform == (Object) null || (Object) this.BottomTransform == (Object) null)
      return;
    if ((Object) this._material == (Object) null)
    {
      this._material = !Application.isPlaying ? this.gameObject.GetComponent<MeshRenderer>().sharedMaterial : this.gameObject.GetComponent<MeshRenderer>().material;
      if ((Object) this._material == (Object) null)
        return;
    }
    this._material.SetMatrix("_TopMatrix", this.TopTransform.localToWorldMatrix);
    this._material.SetMatrix("_BottomMatrix", this.BottomTransform.localToWorldMatrix);
    this.transform.position = (this.TopTransform.position + this.BottomTransform.position) / 2f;
    this.transform.rotation = Quaternion.Slerp(this.TopTransform.rotation, this.BottomTransform.rotation, 0.5f);
  }
}
