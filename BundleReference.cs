// Decompiled with JetBrains decompiler
// Type: BundleReference
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BundleReference : MonoBehaviour
{
  private bool referenceAdded;
  public Bundle m_bundle;

  public Bundle bundle
  {
    get => this.m_bundle;
    set
    {
      if ((Object) this.m_bundle == (Object) null)
        this.m_bundle = value;
      else if (!this.referenceAdded)
      {
        this.LogWarning<BundleReference>("Changing reference link from \"{0}\" to \"{1}\". Is this intentional?", (object) this.m_bundle, (object) value);
        this.m_bundle = value;
      }
      else
        this.LogError<BundleReference>("Cannot change bundle reference, as it has already been registered with its bundle, and it would break the reference link.");
    }
  }

  public void Awake()
  {
    if (!(bool) (Object) this.bundle)
      return;
    this.bundle.AddReference(this);
    this.referenceAdded = true;
  }

  public void OnDestroy()
  {
    if (!(bool) (Object) this.bundle)
      return;
    this.bundle.RemoveReference(this);
    this.referenceAdded = false;
  }
}
