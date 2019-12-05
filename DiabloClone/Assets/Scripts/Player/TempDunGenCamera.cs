using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TempDunGenCamera : MonoBehaviourPunCallbacks
{
  float movementSpeed = 10f;

  private void Awake()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
    if (photonView.IsMine)
    {
      if (Input.GetKey(KeyCode.UpArrow))
      {
        this.gameObject.transform.position += Vector3.forward * movementSpeed * Time.deltaTime;
      }
      else if (Input.GetKey(KeyCode.DownArrow))
      {
        this.gameObject.transform.position += -Vector3.forward * movementSpeed * Time.deltaTime;
      }
      else if (Input.GetKey(KeyCode.LeftArrow))
      {
        this.gameObject.transform.position += -Vector3.right * movementSpeed * Time.deltaTime;
      }
      else if (Input.GetKey(KeyCode.RightArrow))
      {
        this.gameObject.transform.position += Vector3.right * movementSpeed * Time.deltaTime;
      }
    }
  }
}
