// Decompiled with JetBrains decompiler
// Type: DynUIPopupListTS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class DynUIPopupListTS : UIPopupList, IPositionedPopupList
{
  public void AddItem(string displayText, IPositionedPopupListItem_EventHandler eventHandler)
  {
    DynUIPopupListTS.UIPositionedPopupListItem data = new DynUIPopupListTS.UIPositionedPopupListItem()
    {
      DisplayText = displayText,
      EventHandler = eventHandler
    };
    this.AddItem(displayText, (object) data);
  }

  public void ClearItems() => this.Clear();

  public void Update()
  {
    if (this.items.IndexOf(this.value) < 0 || !(this.data is DynUIPopupListTS.UIPositionedPopupListItem data))
      return;
    for (int index = 0; index < this.items.Count; ++index)
    {
      if (this.items[index] == this.value)
      {
        this.items[index] = data.DisplayText;
        break;
      }
    }
  }

  public class UIPositionedPopupListItem
  {
    public string DisplayText;
    public IPositionedPopupListItem_EventHandler EventHandler;
  }
}
