// Decompiled with JetBrains decompiler
// Type: ConnectionStatistics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Networking;
using uLink;

#nullable disable
public class ConnectionStatistics
{
  public ConnectionStatistics(Connection connection)
  {
    NetworkStatistics statistics = ((NetworkPlayer) connection).statistics;
    if (statistics == null)
      return;
    this.BytesReceived = (long) statistics.bytesReceived;
    this.BytesReceivedPerSecond = statistics.bytesReceivedPerSecond;
    this.AverageBytesReceived = (double) this.BytesReceived / Network.time;
    this.BytesSent = (long) statistics.bytesSent;
    this.BytesSentPerSecond = statistics.bytesSentPerSecond;
    this.AverageBytesSent = (double) this.BytesSent / Network.time;
    this.MessageDuplicatesRejected = statistics.messageDuplicatesRejected;
    this.MessageSequencesRejected = statistics.messageSequencesRejected;
    this.MessagesSent = statistics.messagesSent;
    this.MessagesReceived = statistics.messagesReceived;
    this.MessagesResent = statistics.messagesResent;
    this.MessagesStored = statistics.messagesStored;
    this.MessagesUnsent = statistics.messagesUnsent;
    this.MessagesWithheld = statistics.messagesWithheld;
    this.PacketsReceived = statistics.packetsReceived;
    this.PacketsSent = statistics.packetsSent;
    this.AverageMessageReceiveSize = (double) (this.BytesReceived / this.MessagesReceived);
    this.AverageMessageSentSize = (double) (this.BytesSent / this.MessagesSent);
  }

  public long BytesReceived { get; private set; }

  public double BytesReceivedPerSecond { get; private set; }

  public double AverageBytesReceived { get; private set; }

  public long BytesSent { get; private set; }

  public double BytesSentPerSecond { get; private set; }

  public double AverageBytesSent { get; private set; }

  public long MessageDuplicatesRejected { get; private set; }

  public long MessageSequencesRejected { get; private set; }

  public long MessagesSent { get; private set; }

  public long MessagesReceived { get; private set; }

  public long MessagesResent { get; private set; }

  public long MessagesStored { get; private set; }

  public long MessagesUnsent { get; private set; }

  public long MessagesWithheld { get; private set; }

  public long PacketsReceived { get; private set; }

  public long PacketsSent { get; private set; }

  public double AverageMessageReceiveSize { get; private set; }

  public double AverageMessageSentSize { get; private set; }
}
