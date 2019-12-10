using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.AI;

public class GameMasterV2 : MonoBehaviourPunCallbacks
{
  public GameObject playerPrefab;
  private GameObject currPlayer;
  public GameObject loadingCanvas;
  private MapGenAlgorithm mapGen;
  public GameObject dungeonParent;
  private ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();

  // Start is called before the first frame update
  void Awake()
  {
    Debug.Log(PhotonNetwork.IsMasterClient);
    //Check if Dungeon needs to be generated
    if (PhotonNetwork.IsMasterClient)
    {
      Debug.Log("Starting Dungeon Generation from Master Client");
      roomProperties.Add("DungeonGenFinished", false);
      mapGen = GameObject.Find("DungeonManager").GetComponent<MapGenAlgorithm>();
      PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
      mapGen.OnDungeonFinished += SetDungeonGenComplete;
      StartCoroutine(WaitUntilDungeonGen());
      mapGen.StartDunGen();
    }
    else
    {
      StartCoroutine(WaitUntilDungeonGen());
    }
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

    if (!PhotonNetwork.IsMasterClient)
    {
      while ((bool)PhotonNetwork.CurrentRoom.CustomProperties["DungeonGenFinished"] == false)
      {
        yield return new WaitForEndOfFrame();
      }

      bool photonViewsGen = false;
      int generatedPhotonViews = 0;
      while (!photonViewsGen)
      {
        try
        {
          HashSet<GameObject> tempPhotonRef = PhotonNetwork.FindGameObjectsWithComponent(typeof(PhotonView));
          generatedPhotonViews = tempPhotonRef.Count;
          Debug.Log("Found: " + generatedPhotonViews);
          Debug.Log("Needed: " + ((int)PhotonNetwork.CurrentRoom.CustomProperties["RoomPhotonViews"] + (PhotonNetwork.PlayerList.Length - 1)));
          if (generatedPhotonViews == ((int)PhotonNetwork.CurrentRoom.CustomProperties["RoomPhotonViews"] + (PhotonNetwork.PlayerList.Length - 1)))
          {
            photonViewsGen = true;
          }
        }
        catch
        {

        }
        yield return new WaitForEndOfFrame();
      }
    }
    else
    {
      while ((bool)PhotonNetwork.CurrentRoom.CustomProperties["DungeonGenFinished"] == false)
      {
        yield return new WaitForEndOfFrame();
      }
    }

    Debug.Log("Level Loaded");
    Debug.Log("Building Nav Mesh");

    //Build NavMesh
    HashSet<GameObject> dungeonTiles = PhotonNetwork.FindGameObjectsWithComponent(typeof(PhotonView));
    foreach (GameObject dungeonTile in dungeonTiles)
    {
      Debug.Log(dungeonTile.name);
      if (dungeonTile.CompareTag("Player"))
      {
        continue;
      }

      try
      {
        dungeonTile.GetComponent<PlayerNavigation>();
      }
      catch
      {
        dungeonTile.transform.SetParent(dungeonParent.transform);
      }
    }
    dungeonParent.GetComponent<NavMeshSurface>().BuildNavMesh();

    if (PhotonNetwork.IsMasterClient)
    {
      HashSet<GameObject> roomPhotonViews = PhotonNetwork.FindGameObjectsWithComponent(typeof(PhotonView));
      roomProperties.Add("RoomPhotonViews", roomPhotonViews.Count);
      PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }

    //Spawn Player
    SpawnPlayer();

    loadingCanvas.SetActive(false);
  }

  void SpawnPlayer()
  {
    //Spawn in player
    currPlayer = PhotonNetwork.Instantiate(playerPrefab.name, (Vector3)PhotonNetwork.CurrentRoom.CustomProperties["StartPosition"] + new Vector3(0f, 1f, 0f), Quaternion.identity);
    GameObject.Find("Main Camera").GetComponent<SimpleCameraFollow>().followObject = currPlayer.transform;
    currPlayer.name = "Meep";
    //GameObject.Find("Main Camera").GetComponent<SimpleCameraFollow>().SetTarget();
  }
}
