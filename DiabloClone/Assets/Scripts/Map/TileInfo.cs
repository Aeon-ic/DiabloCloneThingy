using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileInfo : MonoBehaviour
{
  //TileInfo
  public List<TileInfo> borderingTiles = new List<TileInfo>();
  public int borderingTilesAmount;
  public TileType tileType;
  public Vector3 tileRotation;
  public GameObject room;

  //TempRoomPrefabs
  public GameObject endPrefab;
  public GameObject hallwayPrefab;
  public GameObject cornerPrefab;
  public GameObject sidePrefab;
  public GameObject middlePrefab;

  public enum TileType
  {
    end = 0,
    hallway = 1,
    corner = 2,
    side = 3,
    middle = 4
  }

  private void Awake()
  {
    this.gameObject.transform.GetComponentInParent<GenerateTileInfo>().OnTileInfoFinish += GenerateTile;
    endPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Rooms/TempEndRoom.prefab", typeof(GameObject)) as GameObject;
    hallwayPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Rooms/TempHallway.prefab", typeof(GameObject)) as GameObject;
    cornerPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Rooms/TempCornerRoom.prefab", typeof(GameObject)) as GameObject;
    sidePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Rooms/TempSideRoom.prefab", typeof(GameObject)) as GameObject;
    middlePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Rooms/TempMiddle.prefab", typeof(GameObject)) as GameObject;
  }

  void GenerateTile()
  {
    switch(tileType)
    {
      case TileType.end:
        room = Instantiate(endPrefab, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform);
        break;
      case TileType.hallway:
        room = Instantiate(hallwayPrefab, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform);
        break;
      case TileType.corner:
        room = Instantiate(cornerPrefab, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform);
        break;
      case TileType.side:
        room = Instantiate(sidePrefab, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform);
        break;
      case TileType.middle:
        room = Instantiate(middlePrefab, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform);
        break;
    }

    room.transform.rotation = Quaternion.Euler(tileRotation);
  }
}
