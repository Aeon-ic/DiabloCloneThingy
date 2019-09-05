using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanels : MonoBehaviour
{
  public Canvas inventoryCanvas;

  private void Awake()
  {
    GameObject.Find("EventSystem").GetComponent<PlayerInputManager>().OnIPress += ToggleInventory;
    GameObject.Find("EventSystem").GetComponent<PlayerInputManager>().OnEscapePress += CloseInventory;
  }

  private void ToggleInventory()
  {
    inventoryCanvas.enabled = !inventoryCanvas.enabled;
  }

  private void CloseInventory()
  {
    inventoryCanvas.enabled = false;
  }
}
