using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerNavigation : MonoBehaviourPunCallbacks
{
  //Inspector Variables
  public float playerSatisfactionDistanceSq = 5f;

  //References
  private NavMeshAgent playerAgent;
  private NavMeshPath playerPath;
  private GameObject playerTarget;

  private void Awake()
  {
    if (photonView.IsMine)
    {
      playerAgent = this.gameObject.GetComponent<NavMeshAgent>();
      playerPath = playerAgent.path;
      GameObject.Find("EventSystem").GetComponent<PlayerInputManager>().OnLeftMousePress += HandleMouseDown;
      GameObject.Find("EventSystem").GetComponent<PlayerInputManager>().OnLeftMousePressRelease += HandleMouseUp;
      Debug.Log("Nav started");
    }
  }

  //public void HandleMouse()
  //{
  //  RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100f);
  //  Debug.Log("Attempting mouse handle for: " + hits.Length);

  //  if (hits.Length > 0)
  //  {
  //    foreach (RaycastHit hit in hits)
  //    {
  //      Debug.Log("Hit: " + hit.transform.gameObject.name);
  //      if (hit.transform.gameObject.CompareTag("Enemy"))
  //      {
  //        Debug.Log("Hit Enemy");
  //        NavMeshHit nearestHit = new NavMeshHit();
  //        NavMesh.SamplePosition(hit.point, out nearestHit, 10f, NavMesh.AllAreas);
  //        playerAgent.CalculatePath(nearestHit.position, playerPath);
  //        playerAgent.SetPath(playerPath);
  //        break;
  //      }
  //      else if (hit.transform.gameObject.CompareTag("Ground"))
  //      {
  //        //If it hit ground, try to calculate path to hit point, if so, go, otherwise calculate path to closest point
  //        if (!playerAgent.CalculatePath(hit.point, playerPath))
  //        {
  //          NavMeshHit nearestHit = new NavMeshHit();
  //          NavMesh.SamplePosition(hit.point, out nearestHit, 10f, NavMesh.AllAreas);
  //          playerAgent.CalculatePath(nearestHit.position, playerPath);
  //          playerAgent.SetPath(playerPath);
  //        }
  //        else
  //        {
  //          playerAgent.SetPath(playerPath);
  //        }
  //      }
  //    }
  //  }
  //}

  public void HandleMouseDown()
  {
    //Raycast to objects
    RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100f);

    //Check raycast hits
    if (hits.Length > 0)
    {
      foreach (RaycastHit hit in hits)
      {
        //If it hit ground, try to calculate path to hit point
        if (hit.transform.gameObject.CompareTag("Ground"))
        {
          if (!playerAgent.CalculatePath(hit.point, playerPath))
          {
            NavMeshHit nearestHit = new NavMeshHit();
            NavMesh.SamplePosition(hit.point, out nearestHit, 10f, NavMesh.AllAreas);
            playerAgent.CalculatePath(nearestHit.position, playerPath);
            playerAgent.SetPath(playerPath);
          }
          else
          {
            playerAgent.SetPath(playerPath);
          }
        }
      }
    }
  }

  public void HandleMouseUp()
  {
    //Raycast to objects
    RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100f);

    //Check raycast hits
    if (hits.Length > 0)
    {
      foreach (RaycastHit hit in hits)
      {
        Debug.Log(hit.transform.name);
        Debug.Log(hit.transform.tag);
        //If it hit ground, try to calculate path to hit point
        if (hit.transform.gameObject.CompareTag("Breakable"))
        {
          NavMeshHit nearestHit = new NavMeshHit();
          NavMesh.SamplePosition(hit.point, out nearestHit, 10f, NavMesh.AllAreas);
          playerAgent.CalculatePath(nearestHit.position, playerPath);
          playerAgent.SetPath(playerPath);
          playerTarget = hit.transform.gameObject;
          StartCoroutine(PlayerTargeted());
          break;
        }
      }
    }
  }

  IEnumerator PlayerTargeted()
  { 
    while (playerTarget != null)
    {
      if ((playerAgent.transform.position - playerTarget.transform.position).sqrMagnitude < playerSatisfactionDistanceSq)
      {
        playerAgent.ResetPath();
        playerAgent.transform.LookAt(playerTarget.transform.position);
        playerTarget.GetComponent<DestructionRPC>().DestroyBreakable();
        playerTarget = null;
      }
      yield return new WaitForEndOfFrame();
    }
  }
}
