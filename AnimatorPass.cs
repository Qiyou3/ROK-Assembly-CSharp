// Decompiled with JetBrains decompiler
// Type: AnimatorPass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class AnimatorPass
{
  public const int QueueBody = 1000;
  public const int QueueAction = 2000;
  public const int QueueDetail = 3000;
  public System.Action Action;
  public int Queue;

  public AnimatorBehaviour Behaviour => (AnimatorBehaviour) this.Action.Target;
}
