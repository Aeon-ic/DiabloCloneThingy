using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TileInfo : MonoBehaviour
{
  //TileInfo
  public List<TileInfo> borderingTiles = new List<TileInfo>();
  public Vector3Int tileMapPosition;
  public int borderingTilesAmount;
  public TileType tileType;
  public Vector3 tileRotation;
  public GameObject room;

  //Private
  private Vector3 doorFix = new Vector3(-.005f, 1f, -.005f);

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
    GameObject.Find("DungeonManager").GetComponent<GenerateTileInfo>().OnTileInfoFinish += GenerateTile;
  }

  public void GenerateTile()
  {
    //Create the room with a returned room from the TileLibrary
    room = PhotonNetwork.Instantiate(TileLibrary.instance.GenerateRoom(tileType).name, gameObject.transform.position, Quaternion.identity);
    room.transform.rotation = Quaternion.Euler(tileRotation);

    //Check if door is needed in the hallway
    if (tileType == TileType.hallway)
    {
      //Check adjacencies
      foreach (TileInfo adjacentTile in borderingTiles)
      {
        //If the adjacent room borders more than 3 tiles
        if (adjacentTile.borderingTilesAmount >= 3)
        {
          //Check if it is a side room
          if (adjacentTile.tileType == TileType.side)
          {
            //If it's a side room, check all adjacencies of the side room to see if it is connected to a middle or another side
            bool sideTileIsConnectingHallways = true;
            foreach (TileInfo adjacentSideAdjacencies in adjacentTile.borderingTiles)
            {
              if (adjacentSideAdjacencies.tileType == TileType.middle || adjacentSideAdjacencies.tileType == TileType.side)
              {
                sideTileIsConnectingHallways = false;
              }
            }
            //If it is just connecting hallways and dead ends, skip this loop item
            if (sideTileIsConnectingHallways)
            {
              continue;
            }
          }
          //Generate door
          GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
        }
      }
    }
    //Check if it is two sides that are facing opposite ways that are not pointed at each other
    else if (tileType == TileType.side)
    {
      //Check adjacent rooms
      foreach (TileInfo adjacentTile in borderingTiles)
      {
        //See if it's a side room
        if (adjacentTile.tileType == TileType.side)
        {
          //Check if they are in the x direction
          if (adjacentTile.tileMapPosition.x - tileMapPosition.x != 0)
          {
            //Check if the rotations are opposing each other
            if (tileRotation.y == 0f && adjacentTile.tileRotation.y == 180f)
            {
              GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
            }
            else if (tileRotation.y == 180f && adjacentTile.tileRotation.y == 0f)
            {
              GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
            }
          }
          //Check if they are in the y direction
          else if (adjacentTile.tileMapPosition.y - tileMapPosition.y != 0)
          {
            //Check if the rotation are opposing each other
            if (tileRotation.y == 270f && adjacentTile.tileRotation.y == 90f)
            {
              GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
            }
            else if (tileRotation.y == 90f && adjacentTile.tileRotation.y == 270f)
            {
              GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
            }
          }
        }

        //Check for corners
        if (adjacentTile.tileType == TileType.corner)
        {
          //Special corner and side connections for doors
          if (Mathf.Abs(tileMapPosition.x - adjacentTile.tileMapPosition.x) > 0)
          {
            if (tileRotation.y == 180f && adjacentTile.tileRotation.y == 270f)
            {
              GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
            }
            else if (tileRotation.y == 180f && adjacentTile.tileRotation.y == 0f)
            {
              GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
            }
            else if (tileRotation.y == 0f && adjacentTile.tileRotation.y == 90f)
            {
              GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
            }
            else if (tileRotation.y == 0f && adjacentTile.tileRotation.y == 180f)
            {
              GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
            }
          }
          else if (Mathf.Abs(tileMapPosition.y - adjacentTile.tileMapPosition.y) > 0)
          {
            if (tileRotation.y == 90f && adjacentTile.tileRotation.y == 180f)
            {
              GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
            }
            else if (tileRotation.y == 90f && adjacentTile.tileRotation.y == 270f)
            {
              GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
            }
            else if (tileRotation.y == 270f && adjacentTile.tileRotation.y == 90f)
            {
              GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
            }
            else if (tileRotation.y == 270f && adjacentTile.tileRotation.y == 0f)
            {
              GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
            }
          }
        }
      }
    }
  }

  void GenerateDoor(Vector3 blockedRoomPos, TileType type)
  {
    if (type != TileType.side)
    {
      DoorGenManager.instance.AddDoor(PhotonNetwork.Instantiate(TileLibrary.instance.hallwayDoor.name, ((blockedRoomPos - gameObject.transform.position) / 2) + gameObject.transform.position + doorFix, 
        Quaternion.Euler(tileRotation)));
    }
    else
    {
      DoorGenManager.instance.AddDoor(PhotonNetwork.Instantiate(TileLibrary.instance.hallwayDoor.name, ((blockedRoomPos - gameObject.transform.position) / 2) + gameObject.transform.position + doorFix, 
        Quaternion.Euler(tileRotation + new Vector3(0f, 90f, 0f))));
    }
  }

  //public void RegenTile(GameObject tileFab, Vector3 position, Vector3 rotation)
  //{
  //  //Create the room with a returned room from the TileLibrary
  //  room = Instantiate(tileFab, position, Quaternion.identity, gameObject.transform);
  //  room.transform.rotation = Quaternion.Euler(rotation);

  //  //Check if door is needed in the hallway
  //  if (tileType == TileType.hallway)
  //  {
  //    //Check adjacencies
  //    foreach (TileInfo adjacentTile in borderingTiles)
  //    {
  //      //If the adjacent room borders more than 3 tiles
  //      if (adjacentTile.borderingTilesAmount >= 3)
  //      {
  //        //Check if it is a side room
  //        if (adjacentTile.tileType == TileType.side)
  //        {
  //          //If it's a side room, check all adjacencies of the side room to see if it is connected to a middle or another side
  //          bool sideTileIsConnectingHallways = true;
  //          foreach (TileInfo adjacentSideAdjacencies in adjacentTile.borderingTiles)
  //          {
  //            if (adjacentSideAdjacencies.tileType == TileType.middle || adjacentSideAdjacencies.tileType == TileType.side)
  //            {
  //              sideTileIsConnectingHallways = false;
  //            }
  //          }
  //          //If it is just connecting hallways and dead ends, skip this loop item
  //          if (sideTileIsConnectingHallways)
  //          {
  //            continue;
  //          }
  //        }
  //        //Generate door
  //        GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
  //      }
  //    }
  //  }
  //  //Check if it is two sides that are facing opposite ways that are not pointed at each other
  //  else if (tileType == TileType.side)
  //  {
  //    //Check adjacent rooms
  //    foreach (TileInfo adjacentTile in borderingTiles)
  //    {
  //      //See if it's a side room
  //      if (adjacentTile.tileType == TileType.side)
  //      {
  //        //Check if they are in the x direction
  //        if (adjacentTile.tileMapPosition.x - tileMapPosition.x != 0)
  //        {
  //          //Check if the rotations are opposing each other
  //          if (tileRotation.y == 0f && adjacentTile.tileRotation.y == 180f)
  //          {
  //            GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
  //          }
  //          else if (tileRotation.y == 180f && adjacentTile.tileRotation.y == 0f)
  //          {
  //            GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
  //          }
  //        }
  //        //Check if they are in the y direction
  //        else if (adjacentTile.tileMapPosition.y - tileMapPosition.y != 0)
  //        {
  //          //Check if the rotation are opposing each other
  //          if (tileRotation.y == 270f && adjacentTile.tileRotation.y == 90f)
  //          {
  //            GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
  //          }
  //          else if (tileRotation.y == 90f && adjacentTile.tileRotation.y == 270f)
  //          {
  //            GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
  //          }
  //        }
  //      }

  //      //Check for corners
  //      if (adjacentTile.tileType == TileType.corner)
  //      {
  //        //Special corner and side connections for doors
  //        if (Mathf.Abs(tileMapPosition.x - adjacentTile.tileMapPosition.x) > 0)
  //        {
  //          if (tileRotation.y == 180f && adjacentTile.tileRotation.y == 270f)
  //          {
  //            GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
  //          }
  //          else if (tileRotation.y == 180f && adjacentTile.tileRotation.y == 0f)
  //          {
  //            GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
  //          }
  //          else if (tileRotation.y == 0f && adjacentTile.tileRotation.y == 90f)
  //          {
  //            GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
  //          }
  //          else if (tileRotation.y == 0f && adjacentTile.tileRotation.y == 180f)
  //          {
  //            GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
  //          }
  //        }
  //        else if (Mathf.Abs(tileMapPosition.y - adjacentTile.tileMapPosition.y) > 0)
  //        {
  //          if (tileRotation.y == 90f && adjacentTile.tileRotation.y == 180f)
  //          {
  //            GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
  //          }
  //          else if (tileRotation.y == 90f && adjacentTile.tileRotation.y == 270f)
  //          {
  //            GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
  //          }
  //          else if (tileRotation.y == 270f && adjacentTile.tileRotation.y == 90f)
  //          {
  //            GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
  //          }
  //          else if (tileRotation.y == 270f && adjacentTile.tileRotation.y == 0f)
  //          {
  //            GenerateDoor(adjacentTile.gameObject.transform.position, tileType);
  //          }
  //        }
  //      }
  //    }
  //  }
  //}
}
