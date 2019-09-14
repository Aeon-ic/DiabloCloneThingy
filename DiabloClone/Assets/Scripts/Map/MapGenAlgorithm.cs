using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenAlgorithm : MonoBehaviour
{
  //Map Size
  [SerializeField]
  int mapWidth;
  [SerializeField]
  int mapHeight;

  //Tilemap Reference
  [SerializeField]
  Tilemap tileMap;

  //Tile Objects
  [SerializeField]
  TileBase floorTile;
  [SerializeField]
  TileBase wallTile;

  //Rendering Map over time stuff
  [SerializeField]
  float tileGenTime = .25f;
  Vector3Int lastPosition;
  Vector3Int startPosition;



  private void Start()
  {
    //Fill with Floor tiles
    for(int i = 0; i < mapWidth; i++)
    {
      for(int j = 0; j < mapHeight; j++)
      {
        Debug.Log("Generating floor at: (" + i + ", " + j + ")");
        tileMap.SetTile(new Vector3Int(i, j, 0), floorTile);
      }
    }

    //Set lastPosition to starting position
    lastPosition = new Vector3Int(Random.Range(1,mapHeight), 1, 0);
    startPosition = lastPosition;

    //Start generating tiles
    StartCoroutine(GenerateTile());
  }

  IEnumerator GenerateTile()
  {

    yield return new WaitForSecondsRealtime(tileGenTime);
  }

  Vector3Int MoveTile(Vector3Int lastPosition)
  {
    int random = Random.Range(0, 10);
    //if(random > )
    //{

    //}
    return lastPosition;
  }
}
