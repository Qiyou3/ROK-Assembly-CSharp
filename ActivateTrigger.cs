// Decompiled with JetBrains decompiler
// Type: ActivateTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ActivateTrigger : MonoBehaviour
{
  public ActivateTrigger.Mode action = ActivateTrigger.Mode.Activate;
  public Object target;
  public GameObject source;
  public int triggerCount = 1;
  public bool repeatTrigger;

  private void DoActivateTrigger()
  {
    --this.triggerCount;
    if (this.triggerCount != 0 && !this.repeatTrigger)
      return;
    Object @object = !(this.target != (Object) null) ? (Object) this.gameObject : this.target;
    Behaviour behaviour = @object as Behaviour;
    GameObject gameObject = @object as GameObject;
    if ((Object) behaviour != (Object) null)
      gameObject = behaviour.gameObject;
    switch (this.action)
    {
      case ActivateTrigger.Mode.Trigger:
        gameObject.BroadcastMessage(nameof (DoActivateTrigger));
        break;
      case ActivateTrigger.Mode.Replace:
        if (!((Object) this.source != (Object) null))
          break;
        Object.Instantiate((Object) this.source, gameObject.transform.position, gameObject.transform.rotation);
        Object.DestroyObject((Object) gameObject);
        break;
      case ActivateTrigger.Mode.Activate:
        gameObject.SetActive(true);
        break;
      case ActivateTrigger.Mode.Enable:
        if (!((Object) behaviour != (Object) null))
          break;
        behaviour.enabled = true;
        break;
      case ActivateTrigger.Mode.Animate:
        gameObject.GetComponent<Animation>().Play();
        break;
      case ActivateTrigger.Mode.Deactivate:
        gameObject.SetActive(false);
        break;
    }
  }

  public void OnTriggerEnter(Collider other) => this.DoActivateTrigger();

  public enum Mode
  {
    Trigger,
    Replace,
    Activate,
    Enable,
    Animate,
    Deactivate,
  }
}
