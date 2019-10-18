using UnityEngine;

public class TempDunGenCamera : MonoBehaviour
{
  float movementSpeed = 10f;

  // Update is called once per frame
  void Update()
  {
    if(Input.GetKey(KeyCode.UpArrow))
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
