// Decompiled with JetBrains decompiler
// Type: BuiltinAssetsHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class BuiltinAssetsHelper
{
  private static bool _assetReferencesAssigend;
  private static Material _defaultMaterial;
  private static Mesh _cubeMesh;

  private static void EnsureAssetReferences()
  {
    if (BuiltinAssetsHelper._assetReferencesAssigend)
      return;
    GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
    BuiltinAssetsHelper._cubeMesh = primitive.GetComponent<MeshFilter>().sharedMesh;
    BuiltinAssetsHelper._defaultMaterial = primitive.GetComponent<Renderer>().material;
    Object.Destroy((Object) primitive);
    BuiltinAssetsHelper._assetReferencesAssigend = true;
  }

  public static Material DefaultMaterial
  {
    get
    {
      BuiltinAssetsHelper.EnsureAssetReferences();
      return BuiltinAssetsHelper._defaultMaterial;
    }
  }

  public static Mesh CubeMesh
  {
    get
    {
      BuiltinAssetsHelper.EnsureAssetReferences();
      return BuiltinAssetsHelper._cubeMesh;
    }
  }
}
