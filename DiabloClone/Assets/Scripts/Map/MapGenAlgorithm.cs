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

  //Map Gen Variables
  [SerializeField]
  int rightWeight;
  [SerializeField]
  int passthroughs;

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
    for (int i = 0; i < mapWidth; i++)
    {
      for (int j = 0; j < mapHeight; j++)
      {
        tileMap.SetTile(new Vector3Int(i, j, 0), wallTile);
      }
    }

    //Set lastPosition to starting position
    lastPosition = new Vector3Int(1, Random.Range(1, mapHeight - 1), 0);
    startPosition = lastPosition;

    //Start generating tiles
    StartCoroutine(GenerateTile());
  }

  IEnumerator GenerateTile()
  {
    for (int i = 0; i < passthroughs; i++)
    {
      while (lastPosition.x < mapWidth - 1)
      {
        Debug.Log("Generating Tile at: " + lastPosition);
        tileMap.SetTile(lastPosition, floorTile);
        lastPosition = MoveTile(lastPosition);
        yield return new WaitForSecondsRealtime(tileGenTime);
      }
      lastPosition = startPosition;
      if (!(i + 1 >= passthroughs))
      {
        Debug.Log("Starting Passthrough: " + (i + 2));
      }
    }
    Debug.Log("Dungeon Generation Finished!");
  }

  Vector3Int MoveTile(Vector3Int lastPosition)
  {
    int random = Random.Range(0, 10);
    if (random > rightWeight)
    {
      lastPosition.x += 1;
    }
    else
    {
      if (Random.Range(1, 3) > 1)
      {
        if (!(lastPosition.y + 1 >= mapHeight - 1))
        {
          lastPosition.y += 1;
        }
      }
      else
      {
        if (!(lastPosition.y - 1 <= 0))
        {
          lastPosition.y -= 1;
        }
      }
    }
    return lastPosition;
  }
}
