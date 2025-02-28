// Decompiled with JetBrains decompiler
// Type: Bundle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public class Bundle : MonoBehaviour
{
  public static float waitTimeSecondsBeforeUnload = 5f;
  [NonSerialized]
  internal Bundle.State state;
  public Bundle.StreamType streamType = Bundle.StreamType.LocalAssetBundle;
  public Bundle.Mode downloadMode;
  public Bundle.Mode loadMode = Bundle.Mode.OnRequest;
  public string assetBundleFilename;
  public string objectName;
  public string objectTypeName;
  public UnityEngine.Object directReference;
  [NonSerialized]
  internal AssetBundle assetBundle;
  [NonSerialized]
  internal UnityEngine.Object assetBundleObject;
  [NonSerialized]
  internal UnityEngine.Object resource;
  [NonSerialized]
  internal string error;
  [NonSerialized]
  internal bool started;
  [NonSerialized]
  internal WWW downloader;
  [NonSerialized]
  internal AssetBundleCreateRequest request;
  [NonSerialized]
  internal readonly List<BundleReference> bundleReferences = new List<BundleReference>();
  private Coroutine m_checkReferencesCoroutine;
  private float m_unloadAtTime;

  public bool IsAsyncOperationRunning
  {
    get
    {
      return this.state == Bundle.State.Downloading || this.state == Bundle.State.Loading || this.state == Bundle.State.Unloading;
    }
  }

  public string AssetBundleFullDownloadUrl
  {
    get => BundleManager.Instance.downloadUrl + this.assetBundleFilename;
  }

  public string AssetBundleFullPathAbsolute
  {
    get => BundleManager.Instance.LocalPathAbsolute + this.assetBundleFilename;
  }

  internal System.Type ObjectType
  {
    get
    {
      if (string.IsNullOrEmpty(this.objectTypeName))
        return (System.Type) null;
      System.Type type = System.Type.GetType("UnityEngine." + this.objectTypeName + ", UnityEngine");
      if (type == null)
      {
        type = System.Type.GetType(this.objectTypeName);
        if (type == null)
          this.LogError<Bundle>("Type specified \"{0}\" could not be found in the UnityEngine assembly.", (object) this.objectTypeName);
      }
      return type;
    }
  }

  public UnityEngine.Object StreamedObject
  {
    get
    {
      if (this.state != Bundle.State.Loaded)
        return (UnityEngine.Object) null;
      switch (this.streamType)
      {
        case Bundle.StreamType.DirectReference:
          return this.directReference;
        case Bundle.StreamType.DownloadableAssetBundle:
        case Bundle.StreamType.LocalAssetBundle:
          return this.assetBundleObject;
        case Bundle.StreamType.Resource:
          return this.resource;
        default:
          return (UnityEngine.Object) null;
      }
    }
  }

  public void Awake()
  {
    this.state = Bundle.State.Unavailable;
    if (this.directReference != (UnityEngine.Object) null && this.streamType != Bundle.StreamType.DirectReference)
    {
      this.LogError<Bundle>("this.directReference should be set to null when not using the DirectReference stream type, to avoid loading/holding the memory of the object referenced.");
      this.directReference = (UnityEngine.Object) null;
    }
    else if (this.directReference == (UnityEngine.Object) null && this.streamType == Bundle.StreamType.DirectReference)
    {
      this.LogError<Bundle>("Direct Reference cannot be null while using DirectReference stream type.");
      this.state = Bundle.State.Error;
    }
    BundleManager.Instance.RegisterBundle(this);
  }

  public float DownloadProgress => this.downloader == null ? 0.0f : this.downloader.progress;

  public float LoadProgress => this.request == null ? 0.0f : this.request.progress;

  public void AddReference(BundleReference bundleReference)
  {
    if (this.bundleReferences.Contains(bundleReference))
      return;
    this.bundleReferences.Add(bundleReference);
  }

  public void RemoveReference(BundleReference bundleReference)
  {
    this.bundleReferences.Remove(bundleReference);
    this.m_unloadAtTime = Time.time + Bundle.waitTimeSecondsBeforeUnload;
    if (this.m_checkReferencesCoroutine != null)
      return;
    this.m_checkReferencesCoroutine = this.StartCoroutine(this.CheckForNoReferencesAfterSeconds());
  }

  [DebuggerHidden]
  public IEnumerator CheckForNoReferencesAfterSeconds()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new Bundle.\u003CCheckForNoReferencesAfterSeconds\u003Ec__Iterator4E()
    {
      \u003C\u003Ef__this = this
    };
  }

  public int BundleReferenceCount => this.bundleReferences.Count;

  public int BundleReferenceCountAccurate
  {
    get
    {
      this.RemoveNullsInReferences();
      if (this.bundleReferences == null)
        return 0;
      int referenceCountAccurate = 0;
      foreach (UnityEngine.Object bundleReference in this.bundleReferences)
      {
        if (bundleReference != (UnityEngine.Object) null)
          ++referenceCountAccurate;
      }
      return referenceCountAccurate;
    }
  }

  private void RemoveNullsInReferences()
  {
    for (int index1 = 0; index1 < this.bundleReferences.Count; ++index1)
    {
      if ((UnityEngine.Object) this.bundleReferences[index1] == (UnityEngine.Object) null)
      {
        for (int index2 = this.bundleReferences.Count - 1; index2 >= 0; --index2)
        {
          if ((UnityEngine.Object) this.bundleReferences[index2] == (UnityEngine.Object) null)
          {
            this.bundleReferences.RemoveAt(index2);
          }
          else
          {
            this.bundleReferences[index1] = this.bundleReferences[index2];
            this.bundleReferences.RemoveAt(index2);
          }
        }
      }
    }
  }

  public enum State
  {
    Unavailable,
    Downloading,
    Downloaded,
    Loading,
    Loaded,
    Unloading,
    Unloaded,
    Error,
  }

  public enum StreamType
  {
    DirectReference,
    DownloadableAssetBundle,
    LocalAssetBundle,
    Resource,
  }

  public enum Mode
  {
    OnStart,
    OnRequest,
  }
}
