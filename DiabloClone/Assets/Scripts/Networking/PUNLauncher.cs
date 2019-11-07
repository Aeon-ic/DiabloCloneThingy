using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
  }

  public void Connect()
  {
    if(PhotonNetwork.IsConnected)
    {
      Debug.Log("Already connected. Joining random room.");
      PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
    }
    else
    {
      PhotonNetwork.ConnectUsingSettings();
    }
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
    
    PhotonNetwork.JoinOrCreateRoom("Meep", roomOptions, null);
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
      PlayerTtl = 60000,
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
  }
}
