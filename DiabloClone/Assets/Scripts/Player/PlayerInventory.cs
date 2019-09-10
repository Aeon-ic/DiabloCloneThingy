using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
  public int inventorySize = 9;
  public int equipSlots = 5;
  [HideInInspector]
  public List<Item> inventory = new List<Item>();
  [HideInInspector]
  public Item[] equipment;

  private void Awake()
  {
    equipment = new Item[equipSlots];
  }

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
    //Check if the index is out of bounds
    if(inventoryIndex >= inventory.Count)
    {
      return false;
    }

    if ((int)inventory[inventoryIndex].type == equipIndex)
    {
      Item swappedItem = equipment[equipIndex];
      equipment[equipIndex] = inventory[inventoryIndex];
      if (swappedItem != null)
      {
        inventory[inventoryIndex] = swappedItem;
      }
      else
      {
        inventory.RemoveAt(inventoryIndex);
      }

      return true;
    }
    else
    {
      return false;
    }
  }

  public bool DeleteItem(int inventoryIndex)
  {
    try
    {
      inventory.RemoveAt(inventoryIndex);
      return true;
    }
    catch
    {
      return false;
    }
  }
}
