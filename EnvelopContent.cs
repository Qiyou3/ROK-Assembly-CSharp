// Decompiled with JetBrains decompiler
// Type: EnvelopContent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (UIWidget))]
[AddComponentMenu("NGUI/Examples/Envelop Content")]
public class EnvelopContent : MonoBehaviour
{
  public Transform targetRoot;
  public int padLeft;
  public int padRight;
  public int padBottom;
  public int padTop;
  private bool mStarted;

  public void Start()
  {
    this.mStarted = true;
    this.Execute();
  }

  public void OnEnable()
  {
    if (!this.mStarted)
      return;
    this.Execute();
  }

  [ContextMenu("Execute")]
  public void Execute()
  {
    if ((Object) this.targetRoot == (Object) this.transform)
      this.LogError<EnvelopContent>("Target Root object cannot be the same object that has Envelop Content. Make it a sibling instead.", (object) this);
    else if (NGUITools.IsChild(this.targetRoot, this.transform))
    {
      this.LogError<EnvelopContent>("Target Root object should not be a parent of Envelop Content. Make it a sibling instead.", (object) this);
    }
    else
    {
      Bounds relativeWidgetBounds = NGUIMath.CalculateRelativeWidgetBounds(this.transform.parent, this.targetRoot, false);
      float x = relativeWidgetBounds.min.x + (float) this.padLeft;
      float y = relativeWidgetBounds.min.y + (float) this.padBottom;
      float num1 = relativeWidgetBounds.max.x + (float) this.padRight;
      float num2 = relativeWidgetBounds.max.y + (float) this.padTop;
      this.GetComponent<UIWidget>().SetRect(x, y, num1 - x, num2 - y);
      this.BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
    }
  }
}
