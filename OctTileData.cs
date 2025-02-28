// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Geometry.OctTileData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using SmartAssembly.Attributes;
using System;
using UnityEngine;

#nullable disable
namespace CodeHatch.Blocks.Geometry
{
  [ObfuscateControlFlow]
  [Serializable]
  public class OctTileData
  {
    public int tile;
    public byte atom;
    public int complexIndex;
    public float probability;
    public Vector3 euler;
    public bool mirror;

    internal static byte Flip(byte data, byte axis)
    {
      byte num = 0;
      switch (axis)
      {
        case 0:
          num = (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) num | (uint) (byte) ((int) data >> 1 & 1)) | (uint) (byte) (((int) data >> 3 & 1) << 2)) | (uint) (byte) (((int) data >> 5 & 1) << 4)) | (uint) (byte) (((int) data >> 7 & 1) << 6)) | (uint) (byte) (((int) data & 1) << 1)) | (uint) (byte) (((int) data >> 2 & 1) << 3)) | (uint) (byte) (((int) data >> 4 & 1) << 5)) | (uint) (byte) (((int) data >> 6 & 1) << 7));
          break;
        case 1:
          num = (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) num | (uint) (byte) ((int) data >> 2 & 1)) | (uint) (byte) (((int) data >> 3 & 1) << 1)) | (uint) (byte) (((int) data >> 6 & 1) << 4)) | (uint) (byte) (((int) data >> 7 & 1) << 5)) | (uint) (byte) (((int) data & 1) << 2)) | (uint) (byte) (((int) data >> 1 & 1) << 3)) | (uint) (byte) (((int) data >> 4 & 1) << 6)) | (uint) (byte) (((int) data >> 5 & 1) << 7));
          break;
        case 2:
          num = (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) num | (uint) (byte) ((int) data >> 4 & 1)) | (uint) (byte) (((int) data >> 5 & 1) << 1)) | (uint) (byte) (((int) data >> 6 & 1) << 2)) | (uint) (byte) (((int) data >> 7 & 1) << 3)) | (uint) (byte) (((int) data & 1) << 4)) | (uint) (byte) (((int) data >> 1 & 1) << 5)) | (uint) (byte) (((int) data >> 2 & 1) << 6)) | (uint) (byte) (((int) data >> 3 & 1) << 7));
          break;
      }
      return num;
    }

    internal static byte Rotate90(byte data, int axis)
    {
      byte num = 0;
      switch (axis)
      {
        case 0:
          num = (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) num | (uint) (byte) (((int) data & 1) << 2)) | (uint) (byte) (((int) data >> 2 & 1) << 6)) | (uint) (byte) (((int) data >> 6 & 1) << 4)) | (uint) (byte) ((int) data >> 4 & 1)) | (uint) (byte) (((int) data >> 1 & 1) << 3)) | (uint) (byte) (((int) data >> 3 & 1) << 7)) | (uint) (byte) (((int) data >> 7 & 1) << 5)) | (uint) (byte) (((int) data >> 5 & 1) << 1));
          break;
        case 1:
          num = (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) num | (uint) (byte) (((int) data & 1) << 4)) | (uint) (byte) (((int) data >> 4 & 1) << 5)) | (uint) (byte) (((int) data >> 5 & 1) << 1)) | (uint) (byte) ((int) data >> 1 & 1)) | (uint) (byte) (((int) data >> 2 & 1) << 6)) | (uint) (byte) (((int) data >> 6 & 1) << 7)) | (uint) (byte) (((int) data >> 7 & 1) << 3)) | (uint) (byte) (((int) data >> 3 & 1) << 2));
          break;
        case 2:
          num = (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) (byte) ((uint) num | (uint) (byte) (((int) data & 1) << 1)) | (uint) (byte) (((int) data >> 1 & 1) << 3)) | (uint) (byte) (((int) data >> 3 & 1) << 2)) | (uint) (byte) ((int) data >> 2 & 1)) | (uint) (byte) (((int) data >> 4 & 1) << 5)) | (uint) (byte) (((int) data >> 5 & 1) << 7)) | (uint) (byte) (((int) data >> 7 & 1) << 6)) | (uint) (byte) (((int) data >> 6 & 1) << 4));
          break;
      }
      return num;
    }

    internal static byte InvertFill(
      byte binary,
      byte mask,
      bool enforceConcaveEdges,
      bool enforceConvexEdges,
      bool enforceConcaveCorners,
      bool enforceConvexCorners)
    {
      if (mask != (byte) 1 && mask != (byte) 2 && mask != (byte) 4 && mask != (byte) 8 && mask != (byte) 16 && mask != (byte) 32 && mask != (byte) 64 && mask != (byte) 128)
      {
        Logger.ErrorFormat("End Invertfill: Mask ({0}) must be 2 to the power of 0 through 7.", (object) mask);
        return 0;
      }
      bool flag = ((int) binary & (int) mask) != 0;
      binary ^= mask;
      byte mask1 = OctTileData.Flip(mask, (byte) 0);
      if (((int) binary & (int) mask1) != 0 == flag)
        binary = OctTileData.InvertFill(binary, mask1, enforceConcaveEdges, enforceConvexEdges, enforceConcaveCorners, enforceConvexCorners);
      byte mask2 = OctTileData.Flip(mask, (byte) 1);
      if (((int) binary & (int) mask2) != 0 == flag)
        binary = OctTileData.InvertFill(binary, mask2, enforceConcaveEdges, enforceConvexEdges, enforceConcaveCorners, enforceConvexCorners);
      byte mask3 = OctTileData.Flip(mask, (byte) 2);
      if (((int) binary & (int) mask3) != 0 == flag)
        binary = OctTileData.InvertFill(binary, mask3, enforceConcaveEdges, enforceConvexEdges, enforceConcaveCorners, enforceConvexCorners);
      if (enforceConcaveEdges && flag || enforceConvexEdges && !flag)
      {
        byte mask4 = OctTileData.Flip(OctTileData.Flip(mask, (byte) 0), (byte) 1);
        if (((int) binary & (int) mask4) != 0 == flag)
          binary = OctTileData.InvertFill(binary, mask4, enforceConcaveEdges, enforceConvexEdges, enforceConcaveCorners, enforceConvexCorners);
        byte mask5 = OctTileData.Flip(OctTileData.Flip(mask, (byte) 0), (byte) 2);
        if (((int) binary & (int) mask5) != 0 == flag)
          binary = OctTileData.InvertFill(binary, mask5, enforceConcaveEdges, enforceConvexEdges, enforceConcaveCorners, enforceConvexCorners);
        byte mask6 = OctTileData.Flip(OctTileData.Flip(mask, (byte) 1), (byte) 2);
        if (((int) binary & (int) mask6) != 0 == flag)
          binary = OctTileData.InvertFill(binary, mask6, enforceConcaveEdges, enforceConvexEdges, enforceConcaveCorners, enforceConvexCorners);
      }
      if (enforceConcaveCorners && flag || enforceConvexCorners && !flag)
      {
        byte mask7 = OctTileData.Flip(OctTileData.Flip(OctTileData.Flip(mask, (byte) 0), (byte) 1), (byte) 2);
        if (((int) binary & (int) mask7) != 0 == flag)
          binary = OctTileData.InvertFill(binary, mask7, enforceConcaveEdges, enforceConvexEdges, enforceConcaveCorners, enforceConvexCorners);
      }
      return binary;
    }
  }
}
