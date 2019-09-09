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

  bool PickupItem(Item item)
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
}
