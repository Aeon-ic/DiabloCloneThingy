using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ObjectivesSpawner : MonoBehaviour
{
  public GameObject objective;
  public float spawnTime = 3f;
  public List<GameObject> spawnPositions;

  void Start()
  {
    if (PhotonNetwork.IsMasterClient)
    {
      StartCoroutine(SpawnObjects());
    }
  }

  IEnumerator SpawnObjects()
  {
    while (true)
    {
      if(spawnPositions.Count > 0)
      {
        int spawnPosIndex = UnityEngine.Random.Range(0, spawnPositions.Count - 1);
        PhotonNetwork.Instantiate(objective.name, spawnPositions[spawnPosIndex].transform.position, Quaternion.identity);
      }
      yield return new WaitForSecondsRealtime(spawnTime);
    }
  }
}
