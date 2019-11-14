using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PUNLauncher : MonoBehaviourPunCallbacks
{
  public string gameVersion = "1";
  public byte maxPlayers = 3;
  public string nickName = "Aeon";
  public string roomName = "Test";
  public RoomOptions roomOptions = new RoomOptions
  {
    PlayerTtl = 60000,
    EmptyRoomTtl = 0
  };

  // Start is called before the first frame update
  void Start()
  {
    SetPlayerNickName();
    roomOptions.MaxPlayers = maxPlayers;
    PhotonNetwork.GameVersion = gameVersion;

    if (!PhotonNetwork.IsConnected)
    {
      PhotonNetwork.ConnectUsingSettings();
    }
  }

  public void Connect()
  {
    PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
  }

  public override void OnRoomListUpdate(List<RoomInfo> roomList)
  {
    Dropdown dropRoomList = GameObject.Find("RoomsDropDown").GetComponentInChildren<Dropdown>();
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
  }

  public void SetRoomName(string name = "Test")
  {
    roomName = name;
  }

  public override void OnConnectedToMaster()
  {
    Debug.Log("<color=green>Connected to server</color>");

    //PhotonNetwork.JoinOrCreateRoom("Meep", roomOptions, null);
    if (!PhotonNetwork.InLobby)
    {
      PhotonNetwork.JoinLobby();
    }
  }

  public override void OnDisconnected(DisconnectCause cause)
  {
    Debug.Log("<color=red>Disconnected from server: </color>" + cause.ToString());
  }

  public override void OnJoinRandomFailed(short returnCode, string message)
  {
    Debug.Log("<color=red>Failed to join random room.</color>" + "Creating new room." + message);
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
    Debug.Log("<color=red>Failed to join random room.</color>" + message);
  }

  public override void OnJoinedRoom()
  {
    Debug.Log("<color=green>Connected to room.</color>");
    PhotonNetwork.LoadLevel("MultiplayerTest");
  }
}
