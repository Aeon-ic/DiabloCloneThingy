using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
  //Inspector variables
  public GameObject singleplayerCanvas;
  public GameObject multiplayerCanvas;
  public GameObject settingsCanvas;
  public InputField nickField;
  public InputField roomNameField;

  public void _OnSingleplayerClick()
  {
    singleplayerCanvas.SetActive(true);
  }

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
    GameObject.Find("Server").GetComponent<PUNLauncher>().SetPlayerNickName(nickField.text); 
  }

  public void _SetRoomName()
  {
    GameObject.Find("Server").GetComponent<PUNLauncher>().SetRoomName(roomNameField.text);
  }
}
