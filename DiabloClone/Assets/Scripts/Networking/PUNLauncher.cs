using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PUNLauncher : MonoBehaviourPunCallbacks
{
  public string gameVersion = "1";
  public byte maxPlayers = 4;
  public string nickName = "Aeon";
  public string roomName = "Test";
  public RoomOptions roomOptions = new RoomOptions
  {
    PlayerTtl = 60000,
    EmptyRoomTtl = 0
  };
  private List<RoomInfo> currRoomList = new List<RoomInfo>();
  [HideInInspector]
  public MainMenu menu;

  // Start is called before the first frame update
  void Start()
  {
    roomOptions.MaxPlayers = maxPlayers;
    PhotonNetwork.GameVersion = gameVersion;

    if (!PhotonNetwork.IsConnected)
    {
      PhotonNetwork.ConnectUsingSettings();
    }
  }

  public void Connect()
  {
    if (roomOptions.MaxPlayers == 1)
    {
      //Check if the room exists in the current room list
      foreach (RoomInfo roomInfo in currRoomList)
      {
        if (roomInfo.Name == roomName)
        {
          menu.ManageDebugText("Room already exists. Cannot play singleplayer in room.");
          return;
        }
      }

      //If it doesn't, create and join
      PhotonNetwork.CreateRoom(roomName, roomOptions, null);
    }
    else
    {
      PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
    }
  }

  public override void OnRoomListUpdate(List<RoomInfo> roomList)
  {
    currRoomList = roomList;
    Dropdown dropRoomList = this.gameObject.GetComponent<MainMenu>().roomListDropdown;
    dropRoomList.ClearOptions();
    List<Dropdown.OptionData> roomData = new List<Dropdown.OptionData>();

    if (roomList.Count == 0)
    {
      roomData.Add(new Dropdown.OptionData("No Rooms"));
    }
    else
    {
      foreach (RoomInfo roomInfo in roomList)
      {
        roomData.Add(new Dropdown.OptionData(roomInfo.Name));
      }
    }

    dropRoomList.AddOptions(roomData);

    dropRoomList.value = 0;
  }

  public void SetPlayerNickName(string name = "Aeon")
  {
    nickName = name;
    PhotonNetwork.NickName = nickName;
    menu.ManageDebugText($"Name changed to: {nickName}");
  }

  public void SetRoomName(string name = "Test")
  {
    roomName = name;
    menu.ManageDebugText($"Room Name changed to: {roomName}");
  }

  public void SetMaxPlayers(byte newMax)
  {
    roomOptions.MaxPlayers = newMax;
    menu.ManageDebugText("Max Players now: " + newMax);
  }

  public override void OnConnectedToMaster()
  {
    menu.ManageDebugText("<color=green>Connected to server</color>");

    //PhotonNetwork.JoinOrCreateRoom("Meep", roomOptions, null);
    if (!PhotonNetwork.InLobby)
    {
      PhotonNetwork.JoinLobby();
    }
  }

  public override void OnDisconnected(DisconnectCause cause)
  {
    menu.ManageDebugText("<color=red>Disconnected from server: </color>" + cause.ToString());
  }

  public override void OnJoinRandomFailed(short returnCode, string message)
  {
    menu.ManageDebugText("<color=red>Failed to join random room.</color>" + "Creating new room." + message);
    RoomOptions roomOptions = new RoomOptions
    {
      MaxPlayers = maxPlayers,
      PlayerTtl = 0,
      EmptyRoomTtl = 0
    };
    PhotonNetwork.CreateRoom(null, roomOptions);
  }

  public override void OnJoinRoomFailed(short returnCode, string message)
  {
    menu.ManageDebugText("<color=red>Failed to join random room.</color>" + message);
  }

  public override void OnJoinedRoom()
  {
    menu.ManageDebugText("<color=green>Connected to room.</color>");
    PhotonNetwork.LoadLevel("FinalProjectV1");
  }
}
