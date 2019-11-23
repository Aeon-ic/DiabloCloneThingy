using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameMaster : MonoBehaviourPunCallbacks
{
  //Inspector Variables
  public GameObject playerPrefab;
  public GameObject dungeon;

  //References
  MapGenAlgorithm mapGen;

  //Private Variables
  public bool isFirst = false;

  //Temp
  public bool hasDungeon = false;

  // Start is called before the first frame update
  //void Awake()
  //{
  //  if (PhotonNetwork.IsMasterClient)
  //  {
  //    mapGen = dungeon.GetComponent<MapGenAlgorithm>();
  //    mapGen.OnDungeonGen += SpawnPlayer;
  //    mapGen.StartDunGen();
  //    PhotonNetwork.LocalPlayer.CustomProperties.Add("HasDungeon", true);
  //    PhotonNetwork.LocalPlayer.CustomProperties.Add("StartObject", mapGen.startObject);
  //    PhotonNetwork.LocalPlayer.CustomProperties.Add("TileInfoList", mapGen.tileGen.tileInfoList);
  //    hasDungeon = true;
  //  }
  //  else
  //  {
  //    Debug.Log("Searching for dungeon info");
  //    foreach (Player player in PhotonNetwork.PlayerListOthers)
  //    {
  //      if (player.CustomProperties.ContainsKey("HasDungeon"))
  //      {
  //        //Set current player data for others
  //        PhotonNetwork.LocalPlayer.CustomProperties.Add("HasDungeon", true);
  //        PhotonNetwork.LocalPlayer.CustomProperties.Add("StartObject", player.CustomProperties["StartObject"]);
  //        PhotonNetwork.LocalPlayer.CustomProperties.Add("TileInfoList", player.CustomProperties["TileInfoList"]);
  //        hasDungeon = true;
  //      }
  //    }

  //    if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("HasDungeon"))
  //    {
  //      //Spawn Dungeon
  //      List<TileInfo> tiles = (List<TileInfo>)PhotonNetwork.LocalPlayer.CustomProperties["TileInfoList"];
  //      foreach (TileInfo tile in tiles)
  //      {
  //        tile.RegenTile(tile.room, tile.room.transform.position, tile.tileRotation);
  //      }

  //      //Set start position
  //      mapGen.startObject = (GameObject)PhotonNetwork.LocalPlayer.CustomProperties["StartObject"];

  //      //Spawn Player
  //      SpawnPlayer();
  //    }
  //    else
  //    {
  //      Debug.Log("Could not retrieve dungeon");
  //      PhotonNetwork.Disconnect();
  //    }
  //  }
  //}

  //void SpawnPlayer()
  //{
  //  GameObject playerRef = PhotonNetwork.Instantiate(playerPrefab.name, mapGen.startObject.transform.position, Quaternion.identity);
  //  GameObject.Find("Main Camera").GetComponent<SimpleCameraFollow>().followObject = playerRef.transform;
  //  playerRef.name = "Meep";
  //  GameObject.Find("Main Camera").GetComponent<SimpleCameraFollow>().SetTarget();
  //}

  //public override void OnCreatedRoom()
  //{
  //  Debug.Log("Created Room");
  //  isFirst = true;
  //  SpawnWorld();
  //}

  //public override void OnJoinedRoom()
  //{
  //  Debug.Log("Joined Room");
  //  isFirst = false;
  //  SpawnWorld();
  //}
}
