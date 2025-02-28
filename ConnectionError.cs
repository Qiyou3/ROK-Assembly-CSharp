// Decompiled with JetBrains decompiler
// Type: ConnectionError
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
public enum ConnectionError
{
  InternalDirectConnectFailed = -5, // 0xFFFFFFFB
  EmptyConnectTarget = -4, // 0xFFFFFFFC
  IncorrectParameters = -3, // 0xFFFFFFFD
  CreateSocketOrThreadFailure = -2, // 0xFFFFFFFE
  AlreadyConnectedToAnotherServer = -1, // 0xFFFFFFFF
  NoError = 0,
  ConnectionFailed = 14, // 0x0000000E
  TooManyConnectedPlayers = 17, // 0x00000011
  RSAPublicKeyMismatch = 20, // 0x00000014
  ConnectionBanned = 21, // 0x00000015
  InvalidPassword = 22, // 0x00000016
  DetectedDuplicatePlayerID = 23, // 0x00000017
  NATTargetNotConnected = 61, // 0x0000003D
  NATTargetConnectionLost = 62, // 0x0000003E
  NATPunchthroughFailed = 63, // 0x0000003F
  IncompatibleVersions = 64, // 0x00000040
  ServerAuthenticationTimeout = 65, // 0x00000041
  ConnectionTimeout = 70, // 0x00000046
  LimitedPlayers = 71, // 0x00000047
  IsAuthoritativeServer = 80, // 0x00000050
  ApprovalDenied = 81, // 0x00000051
  ProxyTargetNotConnected = 90, // 0x0000005A
  ProxyTargetNotRegistered = 91, // 0x0000005B
  ProxyServerNotEnabled = 92, // 0x0000005C
  ProxyServerOutOfPorts = 93, // 0x0000005D
  ServerNotReady = 128, // 0x00000080
  AuthenticationFailed = 129, // 0x00000081
  NotInWhitelist = 130, // 0x00000082
  BannedByEAC = 131, // 0x00000083
  DisconnectedByEAC = 132, // 0x00000084
  EACViolation = 133, // 0x00000085
}
