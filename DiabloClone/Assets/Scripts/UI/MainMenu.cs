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

  //References
  private PUNLauncher launcher;

  private void Awake()
  {
    launcher = GameObject.Find("Server").GetComponent<PUNLauncher>();
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
}
