using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanels : MonoBehaviour
{
  public Canvas inventoryCanvas;
  [HideInInspector]
  public PlayerInventory playerInventory;
  private Transform[] inventoryButtons;
  private Transform[] equipButtons;
  private Sprite blankSprite;
  private Button clickedButton = null;
  public AudioClip successSound;
  public AudioClip selectedSound;
  public AudioClip errorSound;
  public AudioSource uiAudio;

  private void Awake()
  {
    GameObject.Find("EventSystem").GetComponent<PlayerInputManager>().OnIPress += ToggleInventory;
    GameObject.Find("EventSystem").GetComponent<PlayerInputManager>().OnEscapePress += CloseInventory;
    playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();

    //Fill inventoryButtons with all children of the Inventory panel GameObject
    Transform[] tempButtons = inventoryCanvas.transform.Find("Inventory").Find("Slots").GetComponentsInChildren<Transform>();
    inventoryButtons = new Transform[playerInventory.inventorySize];
    for (int i = 1; i < tempButtons.Length; i++)
    {
      inventoryButtons[i - 1] = tempButtons[i];
    }

    //Fill equipmentButtons with all children of the Equip panel GameObject
    Transform[] tempButtons2 = inventoryCanvas.transform.Find("Equip").GetComponentsInChildren<Transform>();
    equipButtons = new Transform[playerInventory.equipSlots];
    for (int i = 1; i < tempButtons2.Length; i++)
    {
      equipButtons[i - 1] = tempButtons2[i];
    }

    //Get blankSprite asset for when inventory or equipment slot is empty
    blankSprite = Resources.Load<Sprite>("Icons/Icon Empty");

    //Get UI Audio source
    uiAudio = this.gameObject.GetComponent<AudioSource>();
  }

  private void Update()
  {
    for (int i = 0; i < inventoryButtons.Length; i++)
    {
      try
      {
        inventoryButtons[i].gameObject.GetComponent<Image>().sprite = playerInventory.inventory[i].icon;
        inventoryButtons[i].gameObject.GetComponent<Image>().color = playerInventory.inventory[i].color;
      }
      catch
      {
        inventoryButtons[i].gameObject.GetComponent<Image>().sprite = blankSprite;
        inventoryButtons[i].gameObject.GetComponent<Image>().color = Color.white;
      }

    }

    for (int i = 0; i < playerInventory.equipment.Length; i++)
    {
      try
      {
        equipButtons[i].gameObject.GetComponent<Image>().sprite = playerInventory.equipment[i].icon;
        equipButtons[i].gameObject.GetComponent<Image>().color = playerInventory.equipment[i].color;
      }
      catch
      {
        equipButtons[i].gameObject.GetComponent<Image>().sprite = blankSprite;
        equipButtons[i].gameObject.GetComponent<Image>().color = Color.white;
      }
    }
  }

  private void ToggleInventory()
  {
    inventoryCanvas.enabled = !inventoryCanvas.enabled;

    if (!inventoryCanvas.enabled)
    {
      clickedButton = null;
    }
  }

  private void CloseInventory()
  {
    inventoryCanvas.enabled = false;
    clickedButton = null;
  }

  public void _OnClick(Button button)
  {
    //Check if there is already a button selected
    if (clickedButton != null)
    {
      if (clickedButton.transform.parent.name == "Equip")
      {
        if (playerInventory.EquipItem(int.Parse(button.name.Remove(0, 4)), int.Parse(clickedButton.name.Remove(0, 4))))
        {
          uiAudio.PlayOneShot(successSound);
        }
        else
        {
          uiAudio.PlayOneShot(errorSound);
        }
      }
      else if (clickedButton.transform.parent.name == "Slots")
      {
        if (playerInventory.EquipItem(int.Parse(clickedButton.name.Remove(0, 4)), int.Parse(button.name.Remove(0, 4))))
        {
          uiAudio.PlayOneShot(successSound);
        }
        else
        {
          uiAudio.PlayOneShot(errorSound);
        }
      }

      //Reset transparency
      Color tempColor2 = clickedButton.gameObject.GetComponent<Image>().color;
      tempColor2.a = 1f;
      clickedButton.gameObject.GetComponent<Image>().color = tempColor2;

      clickedButton = null;
      return;
    }

    //Check for shift click on an inventory item
    if (button.transform.parent.name == "Slots")
    {
      if (Input.GetKey(KeyCode.LeftShift))
      {
        playerInventory.DeleteItem(int.Parse(button.name.Remove(0, 4)));
        return;
      }
    }

    //If nothing else happened, select button for next click
    clickedButton = button;
    //Set transparency effect
    Color tempColor = clickedButton.gameObject.GetComponent<Image>().color;
    tempColor.a = .25f;
    clickedButton.gameObject.GetComponent<Image>().color = tempColor;
    uiAudio.PlayOneShot(selectedSound);
  }
}
