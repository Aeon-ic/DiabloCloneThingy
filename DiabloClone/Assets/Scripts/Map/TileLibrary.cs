using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLibrary : MonoBehaviour
{
  //Singleton
  public static TileLibrary instance;

  //Room prefab lists
  public List<GameObject> endRoomPrefabs = new List<GameObject>();
  public List<GameObject> hallwayPrefabs = new List<GameObject>();
  public List<GameObject> cornerRoomPrefabs = new List<GameObject>();
  public List<GameObject> sideRoomPrefabs = new List<GameObject>();
  public List<GameObject> middleRoomPrefabs = new List<GameObject>();
  public GameObject hallwayDoor;

  private void Awake()
  {
    if (instance != null && instance != this)
    {
      Destroy(this.gameObject);
    }
    else
    {
      instance = this;
    }
  }

  public GameObject GenerateRoom(TileInfo.TileType tileType)
  {
    switch (tileType)
    {
      case TileInfo.TileType.end:
        return endRoomPrefabs[Random.Range(0, endRoomPrefabs.Count)];
      case TileInfo.TileType.hallway:
        return hallwayPrefabs[Random.Range(0, hallwayPrefabs.Count)];
      case TileInfo.TileType.corner:
        return cornerRoomPrefabs[Random.Range(0, cornerRoomPrefabs.Count)];
      case TileInfo.TileType.side:
        return sideRoomPrefabs[Random.Range(0, sideRoomPrefabs.Count)];
      case TileInfo.TileType.middle:
        return middleRoomPrefabs[Random.Range(0, middleRoomPrefabs.Count)];
      default:
        return middleRoomPrefabs[Random.Range(0, middleRoomPrefabs.Count)];
    }
  }
}
