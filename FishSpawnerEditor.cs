// Decompiled with JetBrains decompiler
// Type: FishSpawnerEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FishSpawnerEditor : MonoBehaviour
{
  public float percentage = 0.5f;
  public string OldName;
  public string NewName;
  public GameObject newFishTop;
  public GameObject newFishBot;
  public GameObject newFishMid;

  [ContextMenu("Remove Clone")]
  public void RemoveClone()
  {
    int num1 = 0;
    int num2 = 0;
    foreach (Transform transform in this.transform)
    {
      ++num1;
      if (transform.name.EndsWith("(Clone)"))
      {
        ++num2;
        transform.name = transform.name.Substring(0, transform.name.LastIndexOf("(Clone)"));
      }
    }
    Debug.LogError((object) ("Edited " + (object) num2 + " out of " + (object) num1 + " children"));
  }

  [ContextMenu("Edit Errytang")]
  public void EditErrytang()
  {
    foreach (Transform transform in this.transform)
    {
      if ((double) Random.value > (double) this.percentage)
      {
        if (transform.name == "[School] " + this.OldName + " Top")
        {
          transform.name = "[School] " + this.NewName + " Top";
          transform.GetComponent<SchoolController>()._childPrefab = new GameObject[1]
          {
            this.newFishTop
          };
        }
        else if (transform.name == "[School] " + this.OldName + " Mid")
        {
          transform.name = "[School] " + this.NewName + " Mid";
          transform.GetComponent<SchoolController>()._childPrefab = new GameObject[1]
          {
            this.newFishMid
          };
        }
      }
    }
  }
}
