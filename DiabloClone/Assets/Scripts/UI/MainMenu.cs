using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
  //Inspector variables
  //public GameObject singleplayerCanvas;
  public GameObject multiplayerCanvas;
  public GameObject settingsCanvas;
  public InputField nickField;
  public InputField roomNameField;
  public Toggle singleplayerToggle;
  public Dropdown roomListDropdown;
  public Text debugText;
  public Button joinButton;

  //Private Variables
  private List<string> debugTextLines = new List<string>();

  //References
  private PUNLauncher launcher;

  private void Awake()
  {
    launcher = GameObject.Find("Server").GetComponent<PUNLauncher>();
    launcher.menu = this;
  }

  //public void _OnSingleplayerClick()
  //{
  //  singleplayerCanvas.SetActive(true);
  //}

  public void _OnMultiplayerClick()
  {
    multiplayerCanvas.SetActive(true);
  }

  public void _OnSettingsClick()
  {
    settingsCanvas.SetActive(true);
  }

  public void _OnQuitClick()
  {
    Application.Quit();
  }

  public void _SetNickName()
  {
    launcher.SetPlayerNickName(nickField.text); 
  }

  public void _SetRoomName()
  {
    launcher.SetRoomName(roomNameField.text);
  }

  public void _SinglePlayerToggle()
  {
    if (singleplayerToggle.isOn)
    {
      launcher.SetMaxPlayers(1);
    }
    else
    {
      launcher.SetMaxPlayers(launcher.maxPlayers);
    }
  }

  public void _OnMultiplayerBack()
  {
    multiplayerCanvas.SetActive(false);
  }

  public void ManageDebugText(string newLine)
  {
    //Add Line
    debugTextLines.Add(newLine);
    debugText.verticalOverflow = VerticalWrapMode.Truncate;

    //Check if it is below bottom of textbox
    if (debugTextLines.Count > (debugText.GetPixelAdjustedRect().height / (debugText.fontSize + 2 * debugText.lineSpacing)))
    {
      //Trim top
      debugTextLines.RemoveAt(0);
    }

    //Display text
    string concatDebugText = "";
    foreach(string line in debugTextLines)
    {
      if (concatDebugText == "")
      {
        concatDebugText = line;
      }
      else
      {
        concatDebugText += "\n" + line;
      }
    }

    debugText.text = concatDebugText;
  }
}
