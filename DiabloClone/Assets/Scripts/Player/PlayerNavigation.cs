using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavigation : MonoBehaviour
{
  //References
  private NavMeshAgent playerAgent;
  private NavMeshPath playerPath;

  private void Awake()
  {
    playerAgent = this.gameObject.GetComponent<NavMeshAgent>();
    playerPath = playerAgent.path;
    GameObject.Find("EventSystem").GetComponent<PlayerInputManager>().OnLeftMousePress += HandleMouse;
    Debug.Log("Nav started");
  }

  public void HandleMouse()
  {
    RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100f);
    Debug.Log("Attempting mouse handle for: " + hits.Length);

    if (hits.Length > 0)
    {
      foreach (RaycastHit hit in hits)
      {
        Debug.Log("Hit: " + hit.transform.gameObject.name);
        if (hit.transform.gameObject.CompareTag("Enemy"))
        {
          Debug.Log("Hit Enemy");
          NavMeshHit nearestHit = new NavMeshHit();
          NavMesh.SamplePosition(hit.point, out nearestHit, 10f, NavMesh.AllAreas);
          playerAgent.CalculatePath(nearestHit.position, playerPath);
          playerAgent.SetPath(playerPath);
          break;
        }
        else if (hit.transform.gameObject.CompareTag("Ground"))
        {
          //If it hit ground, try to calculate path to hit point, if so, go, otherwise calculate path to closest point
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
}
