// Decompiled with JetBrains decompiler
// Type: AttachCameraToCharacter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public class AttachCameraToCharacter : MonoBehaviour
{
  public GameObject overrideAttachTo;
  public Vector3 cameraPosition = new Vector3(-1.8f, -0.5f, -1.8f);
  public Vector3 cameraRotation = new Vector3(0.0f, 12f, 90f);
  private GameObject attachToGO;

  private void Start() => this.StartCoroutine(this.WaitAndAttach());

  [DebuggerHidden]
  private IEnumerator WaitAndAttach()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AttachCameraToCharacter.\u003CWaitAndAttach\u003Ec__Iterator1F1()
    {
      \u003C\u003Ef__this = this
    };
  }
}
