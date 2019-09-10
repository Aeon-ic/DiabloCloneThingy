using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Item
{
  public enum itemType
  {
    Head,
    Torso,
    Boots,
    RightHand,
    LeftHand
  }

  public Sprite icon;
  public Color color;
  public itemType type;

  public Item(int intType, Color newColor)
  {
    switch (intType)
    {
      case 0:
        type = itemType.Head;
        icon = (Sprite) AssetDatabase.LoadAssetAtPath("Assets/Icons/Icon.3_38.png", typeof(Sprite));
        break;
      case 1:
        type = itemType.Torso;
        icon = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Icons/Icon.6_94.png", typeof(Sprite));
        break;
      case 2:
        type = itemType.Boots;
        icon = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Icons/Icon.1_95.png", typeof(Sprite));
        break;
      case 3:
        type = itemType.RightHand;
        icon = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Icons/Icon.4_76.png", typeof(Sprite));
        break;
      case 4:
        type = itemType.LeftHand;
        icon = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Icons/Icon.1_88.png", typeof(Sprite));
        break;
      default:
        type = itemType.Head;
        icon = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Icons/Icon.3_38.png", typeof(Sprite));
        break;
    }

    color = newColor;
  }
}