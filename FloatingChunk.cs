// Decompiled with JetBrains decompiler
// Type: CodeHatch.Blocks.Collapsing.FloatingChunk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Networking;
using CodeHatch.Engine.Serialization;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace CodeHatch.Blocks.Collapsing
{
  public class FloatingChunk : ISerializable
  {
    private Vector3Int[] _BlockCoords;
    private HashSet<Vector3Int> _BlockHashset;

    public Vector3Int[] BlockCoords
    {
      get
      {
        if (this._BlockCoords == null && this._BlockHashset != null)
          this._BlockCoords = this._BlockHashset.ToArray<Vector3Int>();
        return this._BlockCoords;
      }
      set
      {
        this._BlockCoords = value;
        this._BlockHashset = (HashSet<Vector3Int>) null;
      }
    }

    public HashSet<Vector3Int> BlockHashset
    {
      get
      {
        if (this._BlockCoords != null && this._BlockHashset == null)
          this._BlockHashset = new HashSet<Vector3Int>((IEnumerable<Vector3Int>) this._BlockCoords, (IEqualityComparer<Vector3Int>) new Vector3IntEqualityComparer());
        return this._BlockHashset;
      }
      set
      {
        this._BlockHashset = value;
        this._BlockCoords = (Vector3Int[]) null;
      }
    }

    public string Identifier => nameof (FloatingChunk);

    public void Serialize(IStream stream) => stream.Write<Vector3Int[]>(this.BlockCoords);

    public void Deserialize(IStream stream) => this.BlockCoords = stream.Read<Vector3Int[]>();
  }
}
