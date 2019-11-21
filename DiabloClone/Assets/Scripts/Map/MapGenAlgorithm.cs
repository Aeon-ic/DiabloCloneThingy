using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenAlgorithm : MonoBehaviour
{
  //Map Size
  [SerializeField]
  int mapWidth = 20;
  [SerializeField]
  int mapHeight = 10;

  //Map Gen Variables
  [SerializeField]
  int rightWeight = 2;
  [SerializeField]
  int passthroughs = 2;

  //Tilemap Reference
  [SerializeField]
  public Tilemap tileMap = null;

  //Tile Objects
  [SerializeField]
  public TileBase floorTile = null;
  [SerializeField]
  public TileBase wallTile = null;

  //Rendering Map over time stuff
  [SerializeField]
  float tileGenTime = .25f;
  Vector3Int lastPosition;
  Vector3Int startPosition;
  Vector3Int endPosition;

  //Delegates
  public event Action OnDungeonGen = delegate { };

  //Temp
  //bool dunGenFinished = false;
  public GameObject startPrefab;
  [HideInInspector]
  public GameObject startObject;
  public GameObject endPrefab;
  //GameObject startObj;
  //GameObject endObj;
  public GenerateTileInfo tileGen;

  private void Awake()
  {
    tileGen = this.gameObject.GetComponent<GenerateTileInfo>();
  }

  public void StartDunGen()
  {
    ////Temp
    //tileMap.gameObject.GetComponent<TilemapRenderer>().enabled = true;
    //tileGen.ClearOldGen();
    //DoorGenManager.instance.DestroyDoors();
    //Destroy(staartObj);
    //Destroy(endObj);

    //Fill with Floor tiles
    for (int i = 0; i < mapWidth; i++)
    {
      for (int j = 0; j < mapHeight; j++)
      {
        tileMap.SetTile(new Vector3Int(i, j, 0), wallTile);
      }
    }

    //Set lastPosition to starting position
    lastPosition = new Vector3Int(1, UnityEngine.Random.Range(1, mapHeight - 1), 0);
    startPosition = lastPosition;
    //startObj = 
    startObject = Instantiate(startPrefab, tileMap.GetCellCenterWorld(startPosition), Quaternion.identity);

    //Start generating tiles
    StartCoroutine(GenerateTiles());
  }

  IEnumerator GenerateTiles()
  {
    //Set up a loop and loop until the program has generated as many passthroughs as required
    for (int i = 0; i < passthroughs; i++)
    {
      //Loop until the generated position is at the end width position
      while (lastPosition.x < mapWidth - 1)
      {
        //Move and generate tiles
        Debug.Log("Generating Tile at: " + lastPosition);
        tileMap.SetTile(lastPosition, floorTile);
        lastPosition = MoveTile(lastPosition);
        yield return new WaitForSecondsRealtime(tileGenTime);
      }

      //For each passthrough after the first
      if (i > 0)
      {
        //Check how many tiles this passthrough ended from the original pass
        int difference = lastPosition.y - endPosition.y;
        //Loop for the difference
        for (int j = 0; j < Mathf.Abs(difference); j++)
        {
          //Check which direction was the end was in
          if (difference > 0)
          {
            //Set every tile in between this passthrough end and the end position to floor
            Vector3Int endVerifyPosition = new Vector3Int(mapWidth - 2, lastPosition.y + (j + 1) * -1, 0);
            tileMap.SetTile(endVerifyPosition, floorTile);
            Debug.Log("Generating Tile to End Tile at: " + endVerifyPosition);
            yield return new WaitForSecondsRealtime(tileGenTime);
          }
          else
          {
            //Set every tile in between this passthrough end and the end position to floor
            Vector3Int endVerifyPosition = new Vector3Int(mapWidth - 2, lastPosition.y + (j + 1) * 1, 0);
            tileMap.SetTile(endVerifyPosition, floorTile);
            Debug.Log("Generating Tile to End Tile at: " + endVerifyPosition);
            yield return new WaitForSecondsRealtime(tileGenTime);
          }
        }
      }
      //If it's the first passthrough, set the end position to the last position
      else
      {
        endPosition = new Vector3Int(lastPosition.x - 1, lastPosition.y, lastPosition.z);
      }

      //Set the tile to generate back to the start point
      lastPosition = startPosition;

      //If it's not the first passthrough print out the current passthrough to console
      if (!(i + 1 >= passthroughs))
      {
        Debug.Log("Starting Passthrough: " + (i + 2));
      }
    }

    //Spawn object in last room to show where it ended
    //endObj = 
    Instantiate(endPrefab, tileMap.GetCellCenterWorld(endPosition), Quaternion.identity);

    Debug.Log("Dungeon Generation Finished!");
    OnDungeonGen();
    Debug.Log("OnDungeonGen called");
  }

  Vector3Int MoveTile(Vector3Int lastPosition)
  {
    //Generate a random number to compare against
    int random = UnityEngine.Random.Range(0, 10);
    //Check if it should move right
    if (random <= rightWeight)
    {
      lastPosition.x += 1;
    }
    //Otherwise move vertically
    else
    {
      //Generate a 50% chance and move up or down
      if (UnityEngine.Random.Range(1, 3) > 1)
      {
        //Check if the vertical position is going out of the map
        if (!(lastPosition.y + 1 >= mapHeight - 1))
        {
          //If not, go up
          lastPosition.y += 1;
        }
      }
      else
      {
        //Check if the vertical position is going out of the map
        if (!(lastPosition.y - 1 <= 0))
        {
          //If not, go down
          lastPosition.y -= 1;
        }
      }
    }
    return lastPosition;
  }
}
