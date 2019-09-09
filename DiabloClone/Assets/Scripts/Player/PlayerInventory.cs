using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
  public int inventorySize = 9;
  public int equipSlots = 5;
  [HideInInspector]
  public List<Item> inventory;
  [HideInInspector]
  public List<Item> equipment;

  public bool PickupItem(Item item)
  {
    if (inventory.Count < inventorySize)
    {
      inventory.Add(item);
      return true;
    }
    else
    {
      return false;
    }
  }

  public bool EquipItem(int inventoryIndex, int equipIndex)
  {
    if ((int) inventory[inventoryIndex].type == equipIndex)
    {
      Item swappedItem = equipment[equipIndex];
      equipment[equipIndex] = inventory[inventoryIndex];
      inventory[inventoryIndex] = swappedItem;
      return true;
    }
    else
    {
      return false;
    }
  }

  public void DisplayInventory()
  {
    Debug.Log("================================");
    foreach(Item item in inventory)
    {
      Debug.Log(item.type);
    }
    Debug.Log("==================================");
  }
}
