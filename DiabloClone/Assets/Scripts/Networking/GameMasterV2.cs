using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameMasterV2 : MonoBehaviour
{
  public GameObject playerPrefab;
  private GameObject currPlayer;
  public GameObject loadingCanvas;
  private MapGenAlgorithm mapGen;
  private ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();

  // Start is called before the first frame update
  void Start()
  {
    //Check if Dungeon needs to be generated
    if (PhotonNetwork.IsMasterClient)
    {
      Debug.Log("Starting Dungeon Generation from Master Client");
      roomProperties.Add("DungeonGenFinished", false);
      mapGen = GameObject.Find("Dungeon").GetComponent<MapGenAlgorithm>();
      PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
      mapGen.OnDungeonFinished += SetDungeonGenComplete;
      StartCoroutine(WaitUntilDungeonGen());
      mapGen.StartDunGen();
    }
    else
    {
      StartCoroutine(WaitUntilDungeonGen());
    }

    //Spawn in player
    currPlayer = PhotonNetwork.Instantiate(playerPrefab.name, (Vector3)PhotonNetwork.CurrentRoom.CustomProperties["StartPosition"], Quaternion.identity);
    GameObject.Find("Main Camera").GetComponent<SimpleCameraFollow>().followObject = currPlayer.transform;
    currPlayer.name = "Meep";
    //GameObject.Find("Main Camera").GetComponent<SimpleCameraFollow>().SetTarget();
  }

  void SetDungeonGenComplete()
  {
    roomProperties["DungeonGenFinished"] = true;
    roomProperties.Add("StartPosition", mapGen.spawnPositon);
    PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
  }

  IEnumerator WaitUntilDungeonGen()
  {
    loadingCanvas.SetActive(true);

    while ((bool)PhotonNetwork.CurrentRoom.CustomProperties["DungeonGenFinished"] == false)
    {
      yield return new WaitForEndOfFrame();
    }

    loadingCanvas.SetActive(false);
  }
}
