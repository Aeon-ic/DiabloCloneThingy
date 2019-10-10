using System.Collections.Generic;
using UnityEngine;

public class DoorGenManager : MonoBehaviour
{
  //Private variables
  private readonly float doorCheckDistance = .05f;

  //Delegates
  [HideInInspector]
  public List<GameObject> doors = new List<GameObject>();

  //Singleton
  public static DoorGenManager instance;

  private void Awake()
  {
    if (instance != null && instance != this)
    {
      Destroy(gameObject);
    }
    else
    {
      instance = this;
    }
  }

  public void AddDoor(GameObject newDoor)
  {
    doors.Add(newDoor);

    foreach (GameObject door in doors)
    {
      if (door != newDoor)
      {
        if (Mathf.Abs(door.transform.position.x - newDoor.transform.position.x) < doorCheckDistance &&
          Mathf.Abs(door.transform.position.y - newDoor.transform.position.y) < doorCheckDistance &&
          Mathf.Abs(door.transform.position.z - newDoor.transform.position.z) < doorCheckDistance)
        {
          Debug.Log("Deleted dupped door at: " + newDoor.transform.position);
          Destroy(newDoor);
        }
      }
    }
  }
}
