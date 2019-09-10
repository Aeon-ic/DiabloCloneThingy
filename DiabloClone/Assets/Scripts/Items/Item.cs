using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Item
{
  public enum ItemType
  {
    Head,
    Torso,
    Boots,
    RightHand,
    LeftHand
  }

  public Sprite icon;
  public Color color;
  public ItemType type;

  public Item(int intType, Color newColor)
  {
    switch (intType)
    {
      case 0:
        type = ItemType.Head;
        icon = Resources.Load<Sprite>("Assets/Resources/Icons/Icon.3_38.png");
        break;
      case 1:
        type = ItemType.Torso;
        icon = Resources.Load<Sprite>("Assets/Resources/Icons/Icon.6_94.png");
        break;
      case 2:
        type = ItemType.Boots;
        icon = Resources.Load<Sprite>("Assets/Resources/Icons/Icon.1_95.png");
        break;
      case 3:
        type = ItemType.RightHand;
        icon = Resources.Load<Sprite>("Assets/Resources/Icons/Icon.4_76.png");
        break;
      case 4:
        type = ItemType.LeftHand;
        icon = Resources.Load<Sprite>("Assets/Resources/Icons/Icon.1_88.png");
        break;
      default:
        type = ItemType.Head;
        icon = Resources.Load<Sprite>("Assets/Resources/Icons/Icon.3_38.png");
        break;
    }

    color = newColor;
  }
}