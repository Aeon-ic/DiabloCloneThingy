using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChickenGame : MonoBehaviour
{
  public GameObject playerPrefab;

  // Start is called before the first frame update
  void Start()
  {
    GameObject playerRef = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    GameObject.Find("Main Camera").GetComponent<SimpleCameraFollow>().followObject = playerRef.transform;
    playerRef.name = "Meep";
    GameObject.Find("Main Camera").GetComponent<SimpleCameraFollow>().SetTarget();
  }

  // Update is called once per frame
  void Update()
  {

  }
}
