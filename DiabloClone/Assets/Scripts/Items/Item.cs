using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
  private Sprite icon;
  public enum itemType
  {
    Head,
    Torso,
    Leggings,
    RightHand,
    LeftHand
  }
  public Color color;
  public itemType type;

  public Item()
  {

  }

  public Item(int intType, Color newColor)
  {
    switch (intType)
    {
      case 0:
        type = itemType.Head;
        break;
      case 1:
        type = itemType.Torso;
        break;
      case 2:
        type = itemType.Leggings;
        break;
      case 3:
        type = itemType.RightHand;
        break;
      case 4:
        type = itemType.LeftHand;
        break;
      default:
        type = itemType.Head;
        break;
    }

    color = newColor;
  }
}