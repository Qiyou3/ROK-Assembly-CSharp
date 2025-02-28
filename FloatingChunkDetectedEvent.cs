// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Collapsing.FloatingChunkDetectedEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Networking;
using CodeHatch.Networking.Events;
using System;

#nullable disable
namespace CodeHatch.Blocks.Collapsing
{
  public class FloatingChunkDetectedEvent : NetworkEvent
  {
    public FloatingChunkDetectedEvent()
      : this((FloatingChunk) null)
    {
    }

    public FloatingChunkDetectedEvent(FloatingChunk floatingChunk)
      : this(floatingChunk, (Action<BaseEvent>) null)
    {
    }

    public FloatingChunkDetectedEvent(FloatingChunk floatingChunk, Action<BaseEvent> cancelCallback)
      : base(cancelCallback)
    {
      this.FloatingChunk = floatingChunk;
    }

    public FloatingChunk FloatingChunk { get; set; }

    public override void Write(IStream stream)
    {
      base.Write(stream);
      this.FloatingChunk.Serialize(stream);
    }

    public override void Read(IStream stream)
    {
      base.Read(stream);
      this.FloatingChunk = new FloatingChunk();
      this.FloatingChunk.Deserialize(stream);
    }
  }
}
