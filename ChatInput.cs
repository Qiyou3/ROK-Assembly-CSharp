// Decompiled with JetBrains decompiler
// Type: ChatInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Examples/Chat Input")]
[RequireComponent(typeof (UIInput))]
public class ChatInput : MonoBehaviour
{
  public UITextList textList;
  public bool fillWithDummyData;
  private UIInput mInput;

  public void Start()
  {
    this.mInput = this.GetComponent<UIInput>();
    this.mInput.label.maxLineCount = 1;
    if (!this.fillWithDummyData || !((Object) this.textList != (Object) null))
      return;
    for (int index = 0; index < 30; ++index)
      this.textList.Add((index % 2 != 0 ? (object) "[AAAAAA]" : (object) "[FFFFFF]").ToString() + "This is an example paragraph for the text list, testing line " + (object) index + "[-]");
  }

  public void OnSubmit()
  {
    if (!((Object) this.textList != (Object) null))
      return;
    string text = NGUIText.StripSymbols(this.mInput.value);
    if (string.IsNullOrEmpty(text))
      return;
    this.textList.Add(text);
    this.mInput.value = string.Empty;
    this.mInput.isSelected = false;
  }
}
