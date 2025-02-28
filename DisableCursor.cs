// Decompiled with JetBrains decompiler
// Type: DisableCursor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Networking;
using UnityEngine;

#nullable disable
public class DisableCursor : MonoBehaviour
{
  public static DisableCursor disableCursorObj;
  public static int framesCursorHidden;

  public void Start()
  {
    if ((Object) DisableCursor.disableCursorObj == (Object) null)
    {
      DisableCursor.disableCursorObj = this;
      Object.DontDestroyOnLoad((Object) this.gameObject);
    }
    else
    {
      this.LogInfo<DisableCursor>("Only one DisableCursor object is allowed per level. Destroying gameObject.");
      Object.Destroy((Object) this);
      this.enabled = false;
    }
  }

  public void Update()
  {
    if (Player.IsLocalDedi)
      return;
    if (Cursor.visible)
    {
      DisableCursor.framesCursorHidden = 0;
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.None;
    }
    else
      ++DisableCursor.framesCursorHidden;
    if (DisableCursor.framesCursorHidden <= 1)
      return;
    Cursor.lockState = CursorLockMode.Locked;
  }

  public static void ConfineCursorOneFrame()
  {
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Confined;
    DisableCursor.framesCursorHidden = 0;
  }

  public static void EnableCursorOneFrame()
  {
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
    DisableCursor.framesCursorHidden = 0;
  }

  public static void UnlockCursorOneFrame()
  {
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.None;
    DisableCursor.framesCursorHidden = 0;
  }
}
