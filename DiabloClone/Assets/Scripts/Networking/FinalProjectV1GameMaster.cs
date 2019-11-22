using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class FinalProjectV1GameMaster : MonoBehaviourPunCallbacks
{
  public GameObject playerPrefab;
  public List<GameObject> objectives = new List<GameObject>();

  // Start is called before the first frame update
  void Start()
  {
    GameObject playerRef = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    GameObject.Find("Main Camera").GetComponent<SimpleCameraFollow>().followObject = playerRef.transform;
    playerRef.name = "Meep";
    GameObject.Find("Main Camera").GetComponent<SimpleCameraFollow>().SetTarget();
  }
}
