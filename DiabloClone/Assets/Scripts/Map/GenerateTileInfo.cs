using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTileInfo : MonoBehaviour
{
  //TileInfo List
  List<TileInfo> tileInfoList = new List<TileInfo>();

  //References
  public MapGenAlgorithm mapGen;
  private Tilemap tileMap;
  TileBase floorTile;
  [SerializeField]
  GameObject tilePrefab;
  [SerializeField]
  GameObject tileParent;

  //Delegates
  public event Action OnTileInfoFinish = delegate { };

  // Start is called before the first frame update
  void Awake()
  {
    mapGen.OnDungeonGen += StartDungeonInfoGen;
    tileMap = mapGen.tileMap;
    floorTile = mapGen.floorTile;
  }

  void StartDungeonInfoGen()
  {
    GenerateTileInfoObjects();
    SetTileInfo();
    OnTileInfoFinish();
  }

  void GenerateTileInfoObjects()
  {
    for (int i = 0; i < tileMap.size.x; i++)
    {
      for (int j = 0; j < tileMap.size.y; j++)
      {
        Vector3Int currentTile = new Vector3Int(i, j, 0);
        if (tileMap.GetTile(currentTile) == floorTile)
        {
          tileInfoList.Add((Instantiate(tilePrefab, tileMap.GetCellCenterWorld(currentTile), Quaternion.identity, tileParent.transform)).GetComponent<TileInfo>());
          tileInfoList[tileInfoList.Count - 1].tileMapPosition = new Vector3Int(i, j, 0);
        }
      }
    }
  }

  void SetTileInfo()
  {
    //Loop through and assign adjacent rooms to the other one's list
    foreach (TileInfo currentTile in tileInfoList)
    {
      foreach (TileInfo compareTile in tileInfoList)
      {
        if (currentTile != compareTile)
        {
          if (Math.Abs(compareTile.tileMapPosition.x - currentTile.tileMapPosition.x) == 1 && 
            Math.Abs(compareTile.tileMapPosition.y - currentTile.tileMapPosition.y) == 0)
          {
            currentTile.borderingTiles.Add(compareTile);
          }
          if (Math.Abs(compareTile.tileMapPosition.y - currentTile.tileMapPosition.y) == 1 &&
            Math.Abs(compareTile.tileMapPosition.x - currentTile.tileMapPosition.x) == 0)
          {
            currentTile.borderingTiles.Add(compareTile);
          }
        }
      }
    }

    //Loop and assign bordering tiles amounts
    foreach (TileInfo currentTile in tileInfoList)
    {
      Vector3Int currentPos = tileMap.WorldToCell(currentTile.gameObject.transform.position);

      //Check for adjacent tiles
      if (tileMap.GetTile(currentPos + new Vector3Int(1,0,0)) == floorTile)
      {
        currentTile.borderingTilesAmount++;
      }
      if (tileMap.GetTile(currentPos + new Vector3Int(0, 1, 0)) == floorTile)
      {
        currentTile.borderingTilesAmount++;
      }
      if (tileMap.GetTile(currentPos + new Vector3Int(-1, 0, 0)) == floorTile)
      {
        currentTile.borderingTilesAmount++;
      }
      if (tileMap.GetTile(currentPos + new Vector3Int(0, -1, 0)) == floorTile)
      {
        currentTile.borderingTilesAmount++;
      }
    }

    //Loop and assign types
    foreach (TileInfo currentTile in tileInfoList)
    {
      Vector3Int currentPos = tileMap.WorldToCell(currentTile.gameObject.transform.position);

      switch (currentTile.borderingTilesAmount)
      {
        case 4:
          currentTile.tileType = TileInfo.TileType.middle;
          break;
        case 3:
          currentTile.tileType = TileInfo.TileType.side;
          break;
        case 2:
          if (tileMap.GetTile(currentPos + new Vector3Int(1, 0, 0)) == floorTile && tileMap.GetTile(currentPos + new Vector3Int(-1, 0, 0)) == floorTile)
          {
            currentTile.tileType = TileInfo.TileType.hallway;
          }
          else if(tileMap.GetTile(currentPos + new Vector3Int(0, 1, 0)) == floorTile && tileMap.GetTile(currentPos + new Vector3Int(0, -1, 0)) == floorTile)
          {
            currentTile.tileType = TileInfo.TileType.hallway;
          }
          else
          {
            currentTile.tileType = TileInfo.TileType.corner;
          }
          break;
        case 1:
          currentTile.tileType = TileInfo.TileType.end;
          break;
      }
    }

    //Loop and assign rotation heading
    foreach (TileInfo currentTile in tileInfoList)
    {
      Vector3Int currentPos = tileMap.WorldToCell(currentTile.gameObject.transform.position);

      //Based on tileType, assign rotations
      switch (currentTile.tileType)
      {
        case TileInfo.TileType.end:
          if(tileMap.GetTile(currentPos + new Vector3Int(1, 0, 0)) == floorTile)
          {
            currentTile.tileRotation = new Vector3(0f, 90f, 0f);
          }
          else if (tileMap.GetTile(currentPos + new Vector3Int(-1, 0, 0)) == floorTile)
          {
            currentTile.tileRotation = new Vector3(0f, 270f, 0f);
          }
          else if (tileMap.GetTile(currentPos + new Vector3Int(0, 1, 0)) == floorTile)
          {
            currentTile.tileRotation = new Vector3(0f, 0f, 0f);
          }
          else
          {
            currentTile.tileRotation = new Vector3(0f, 180f, 0f);
          }
          break;
        case TileInfo.TileType.hallway:
          if (tileMap.GetTile(currentPos + new Vector3Int(1, 0, 0)) == floorTile && tileMap.GetTile(currentPos + new Vector3Int(-1, 0, 0)) == floorTile)
          {
            currentTile.tileRotation = new Vector3(0f, 90f, 0f);
          }
          else if (tileMap.GetTile(currentPos + new Vector3Int(0, 1, 0)) == floorTile && tileMap.GetTile(currentPos + new Vector3Int(0, -1, 0)) == floorTile)
          {
            currentTile.tileRotation = new Vector3(0f, 0f, 0f);
          }
          break;
        case TileInfo.TileType.side:
          if (tileMap.GetTile(currentPos + new Vector3Int(1, 0, 0)) != floorTile)
          {
            currentTile.tileRotation = new Vector3(0f, 270f, 0f);
          }
          else if (tileMap.GetTile(currentPos + new Vector3Int(-1, 0, 0)) != floorTile)
          {
            currentTile.tileRotation = new Vector3(0f, 90f, 0f);
          }
          else if (tileMap.GetTile(currentPos + new Vector3Int(0, 1, 0)) != floorTile)
          {
            currentTile.tileRotation = new Vector3(0f, 180f, 0f);
          }
          else
          {
            currentTile.tileRotation = new Vector3(0f, 0f, 0f);
          }
          break;
        case TileInfo.TileType.corner:
          if (tileMap.GetTile(currentPos + new Vector3Int(1, 0, 0)) == floorTile && tileMap.GetTile(currentPos + new Vector3Int(0, 1, 0)) == floorTile)
          {
            currentTile.tileRotation = new Vector3(0f, 0f, 0f);
          }
          else if (tileMap.GetTile(currentPos + new Vector3Int(0, 1, 0)) == floorTile && tileMap.GetTile(currentPos + new Vector3Int(-1, 0, 0)) == floorTile)
          {
            currentTile.tileRotation = new Vector3(0f, 270f, 0f);
          }
          else if (tileMap.GetTile(currentPos + new Vector3Int(-1, 0, 0)) == floorTile && tileMap.GetTile(currentPos + new Vector3Int(0, -1, 0)) == floorTile)
          {
            currentTile.tileRotation = new Vector3(0f, 180f, 0f);
          }
          else if (tileMap.GetTile(currentPos + new Vector3Int(0, -1, 0)) == floorTile && tileMap.GetTile(currentPos + new Vector3Int(1, 0, 0)) == floorTile)
          {
            currentTile.tileRotation = new Vector3(0f, 90f, 0f);
          }
          break;
        case TileInfo.TileType.middle:
          switch (UnityEngine.Random.Range(1,5))
          {
            case 1:
              currentTile.tileRotation = new Vector3(0f, 0f, 0f);
              break;
            case 2:
              currentTile.tileRotation = new Vector3(0f, 90f, 0f);
              break;
            case 3:
              currentTile.tileRotation = new Vector3(0f, 180f, 0f);
              break;
            case 4:
              currentTile.tileRotation = new Vector3(0f, 270f, 0f);
              break;
          }
          break;
      }
    }
  }
}
