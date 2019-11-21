using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
  public Transform followObject;
  public Vector3 cameraOffset;

  // Start is called before the first frame update
  public void SetTarget()
  {
    //cameraOffset = followObject.position - this.gameObject.transform.position;
  }

  // Update is called once per frame
  void LateUpdate()
  {
    if (followObject != null)
    {
      this.gameObject.transform.position = followObject.position + cameraOffset;
    }
  }
}
