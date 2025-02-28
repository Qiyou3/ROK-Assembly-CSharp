// Decompiled with JetBrains decompiler
// Type: BundleLoadInplace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class BundleLoadInplace : MonoBehaviour
{
  public string bundleName;
  public Bundle bundle;
  public BundleLoadInplace.Method method;
  public BundleLoadInplace.LoadWhen loadWhen;
  [NonSerialized]
  internal bool m_loading;
  [NonSerialized]
  internal bool m_done;

  public void Awake()
  {
    if (this.loadWhen != BundleLoadInplace.LoadWhen.Awake && this.loadWhen != BundleLoadInplace.LoadWhen.All)
      return;
    BundleManager.Instance.LoadInplace(this);
  }

  public void Start()
  {
    if (this.loadWhen != BundleLoadInplace.LoadWhen.Start && this.loadWhen != BundleLoadInplace.LoadWhen.StartOnEnable && this.loadWhen != BundleLoadInplace.LoadWhen.All)
      return;
    BundleManager.Instance.LoadInplace(this);
  }

  public void OnEnable()
  {
    if (this.loadWhen != BundleLoadInplace.LoadWhen.OnEnable && this.loadWhen != BundleLoadInplace.LoadWhen.StartOnEnable && this.loadWhen != BundleLoadInplace.LoadWhen.All)
      return;
    BundleManager.Instance.LoadInplace(this);
  }

  public enum Method
  {
    ReplaceGameObject,
    LoadAsChild,
  }

  public enum LoadWhen
  {
    Awake,
    Start,
    OnEnable,
    StartOnEnable,
    All,
  }
}
