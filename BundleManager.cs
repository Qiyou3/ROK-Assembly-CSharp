// Decompiled with JetBrains decompiler
// Type: BundleManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

#nullable disable
public class BundleManager : MonoBehaviour
{
  private static BundleManager m_instance;
  private static Bundle[] _AllBundles;
  public string downloadUrl;
  public string localUrlRelative;
  private readonly List<Bundle> m_preStartBundles = new List<Bundle>();
  private bool m_started;

  public static BundleManager Instance
  {
    get
    {
      if ((UnityEngine.Object) BundleManager.m_instance == (UnityEngine.Object) null)
      {
        BundleManager.m_instance = UnityEngine.Object.FindObjectOfType(typeof (BundleManager)) as BundleManager;
        if ((UnityEngine.Object) BundleManager.m_instance == (UnityEngine.Object) null)
        {
          BundleManager.m_instance = new GameObject("_BundleManager").AddComponent<BundleManager>();
          BundleManager.m_instance.gameObject.hideFlags = HideFlags.HideAndDontSave;
        }
      }
      return BundleManager.m_instance;
    }
  }

  private static Bundle[] AllBundles
  {
    get
    {
      if (BundleManager._AllBundles == null)
        BundleManager._AllBundles = UnityEngine.Object.FindObjectsOfType<Bundle>();
      if (BundleManager._AllBundles == null)
        Logger.Error<BundleManager>("The bundles are null.");
      return BundleManager._AllBundles;
    }
  }

  public string LocalPathAbsolute => Application.persistentDataPath + this.localUrlRelative;

  public void Awake()
  {
    if ((UnityEngine.Object) BundleManager.m_instance == (UnityEngine.Object) null)
      BundleManager.m_instance = this;
    else if ((UnityEngine.Object) BundleManager.m_instance != (UnityEngine.Object) this)
    {
      if (this.gameObject.name == "_BundleManager")
      {
        this.LogError<BundleManager>("There cannot exist more than one BundleManager. Destroying this gameObject as it is named \"_BundleManager\".");
        this.gameObject.SetActive(false);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
        this.enabled = false;
        return;
      }
      this.LogError<BundleManager>("There cannot exist more than one BundleManager. Destroying this component but not the gameObject as it is not named \"_BundleManager\" (It was not necessarily created solely for the sake of holding a BundleManager).");
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
      this.enabled = false;
      return;
    }
    this.LogWarning<BundleManager>("BundleManager should be placed on an object that will never destroy itself. Marking as DontDestroyOnLoad().");
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
    if (Directory.Exists(this.LocalPathAbsolute))
      return;
    Directory.CreateDirectory(this.LocalPathAbsolute);
  }

  public void RegisterBundle(Bundle bundle)
  {
    if (this.m_started)
      this.StartBundle(bundle);
    else
      this.m_preStartBundles.Add(bundle);
  }

  public void Start()
  {
    this.m_started = true;
    foreach (Bundle preStartBundle in this.m_preStartBundles)
      this.StartBundle(preStartBundle);
  }

  private void StartBundle(Bundle bundle) => this.StartCoroutine(this.StartBundleCoroutine(bundle));

  [DebuggerHidden]
  public IEnumerator StartBundleCoroutine(Bundle bundle)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new BundleManager.\u003CStartBundleCoroutine\u003Ec__Iterator50()
    {
      bundle = bundle,
      \u003C\u0024\u003Ebundle = bundle,
      \u003C\u003Ef__this = this
    };
  }

  [DebuggerHidden]
  public IEnumerator RequestLoad(Bundle bundle)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new BundleManager.\u003CRequestLoad\u003Ec__Iterator51()
    {
      bundle = bundle,
      \u003C\u0024\u003Ebundle = bundle,
      \u003C\u003Ef__this = this
    };
  }

  [DebuggerHidden]
  public IEnumerator DownloadAssetBundle(Bundle bundle)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new BundleManager.\u003CDownloadAssetBundle\u003Ec__Iterator52()
    {
      bundle = bundle,
      \u003C\u0024\u003Ebundle = bundle,
      \u003C\u003Ef__this = this
    };
  }

  [DebuggerHidden]
  public IEnumerator LoadAssetBundleObject(Bundle bundle)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new BundleManager.\u003CLoadAssetBundleObject\u003Ec__Iterator53()
    {
      bundle = bundle,
      \u003C\u0024\u003Ebundle = bundle,
      \u003C\u003Ef__this = this
    };
  }

  private void LoadResource(Bundle bundle)
  {
    if (bundle.state != Bundle.State.Downloaded)
    {
      this.LogError<BundleManager>("Resource of name \"" + bundle.objectName + "\" and type \"" + (object) bundle.ObjectType + "\" failed to load because: state != State.Downloaded");
    }
    else
    {
      bundle.state = Bundle.State.Loading;
      System.Type objectType = bundle.ObjectType;
      bundle.resource = objectType == null ? Resources.Load(bundle.objectName) : Resources.Load(bundle.objectName, objectType);
      if (bundle.resource == (UnityEngine.Object) null)
      {
        bundle.error = "Resource of name \"" + bundle.objectName + "\" and type \"" + (object) objectType + "\" failed to load because: (resource == null)";
        this.LogError<BundleManager>(bundle.error);
        bundle.state = Bundle.State.Error;
      }
      else
      {
        bundle.state = Bundle.State.Loaded;
        this.OnLoaded(bundle);
      }
    }
  }

  public void OnLoaded(Bundle bundle)
  {
    GameObject streamedObject = bundle.StreamedObject as GameObject;
    if (!((UnityEngine.Object) streamedObject != (UnityEngine.Object) null))
      return;
    BundleReference bundleReference = streamedObject.GetComponent<BundleReference>() ?? streamedObject.AddComponent<BundleReference>();
    if ((UnityEngine.Object) bundleReference != (UnityEngine.Object) null)
      bundleReference.bundle = bundle;
    else
      this.LogError<BundleManager>("bundleReference == null");
  }

  public void Unload(Bundle bundle)
  {
    if (bundle.state != Bundle.State.Loaded)
      this.LogError<BundleManager>("Unload() should only be called when the asset is loaded. It was called in the state \"{0}\".", (object) bundle.state);
    else if (bundle.IsAsyncOperationRunning)
    {
      this.LogWarning<BundleManager>("Unload() called while bundle had a coroutine already working. ({0})", (object) bundle.state);
    }
    else
    {
      switch (bundle.streamType)
      {
        case Bundle.StreamType.DirectReference:
          this.LogWarning<BundleManager>("Resource or Asset of name \"" + bundle.objectName + "\" and type \"" + (object) bundle.ObjectType + "\" could not unloaded because: Directly referenced objects cannot be unloaded from memory.");
          bundle.state = Bundle.State.Loaded;
          break;
        case Bundle.StreamType.DownloadableAssetBundle:
        case Bundle.StreamType.LocalAssetBundle:
          if ((UnityEngine.Object) bundle.assetBundle == (UnityEngine.Object) null)
          {
            bundle.error = "Resource or Asset of name \"" + bundle.objectName + "\" and type \"" + (object) bundle.ObjectType + "\" could not unloaded because: assetBundle == null";
            this.LogError<BundleManager>(bundle.error);
            bundle.state = Bundle.State.Error;
            break;
          }
          bundle.assetBundleObject = (UnityEngine.Object) null;
          bundle.assetBundle.Unload(true);
          UnityEngine.Object.Destroy((UnityEngine.Object) bundle.assetBundle);
          bundle.state = Bundle.State.Downloaded;
          break;
        case Bundle.StreamType.Resource:
          Resources.UnloadUnusedAssets();
          bundle.resource = (UnityEngine.Object) null;
          bundle.state = Bundle.State.Downloaded;
          break;
      }
    }
  }

  public void LoadInplace(BundleLoadInplace loadInplace)
  {
    this.StartCoroutine(this.LoadInplaceCoroutine(loadInplace));
  }

  [DebuggerHidden]
  public IEnumerator LoadInplaceCoroutine(BundleLoadInplace loadInplace)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new BundleManager.\u003CLoadInplaceCoroutine\u003Ec__Iterator54()
    {
      loadInplace = loadInplace,
      \u003C\u0024\u003EloadInplace = loadInplace,
      \u003C\u003Ef__this = this
    };
  }

  public void LoadRendererAssets(BundleLoadRendererAssets loadRendererAssets)
  {
    this.StartCoroutine(this.LoadRenderersCoroutine(loadRendererAssets));
  }

  [DebuggerHidden]
  public IEnumerator LoadRenderersCoroutine(BundleLoadRendererAssets loadRendererAssets)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new BundleManager.\u003CLoadRenderersCoroutine\u003Ec__Iterator55()
    {
      loadRendererAssets = loadRendererAssets,
      \u003C\u0024\u003EloadRendererAssets = loadRendererAssets,
      \u003C\u003Ef__this = this
    };
  }
}
