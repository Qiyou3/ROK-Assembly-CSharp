// Decompiled with JetBrains decompiler
// Type: EnableForLogin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Cheats;
using CodeHatch.Common;
using UnityEngine;

#nullable disable
public class EnableForLogin : MonoBehaviour
{
  public User m_userInfo;
  public GameObject[] gameObjectsEnableLoggedIn;
  public GameObject[] gameObjectsDisableLoggedIn;
  public Cheat playAsGuest = Cheat.GetCheat("playasguest");
  public bool enableForLoggingIn;

  public User userInfo
  {
    get
    {
      if ((Object) this.m_userInfo == (Object) null)
      {
        this.LogWarning<EnableForLogin>(ErrorUtil.NullRefSearchWarning(this.gameObject.GetFullName() + "EnableForLogin.m_userInfo"));
        this.m_userInfo = Object.FindObjectOfType(typeof (User)) as User;
        if ((Object) this.m_userInfo == (Object) null)
          this.LogError<EnableForLogin>("m_userInfo == null");
      }
      return this.m_userInfo;
    }
  }

  public void Update()
  {
  }
}
