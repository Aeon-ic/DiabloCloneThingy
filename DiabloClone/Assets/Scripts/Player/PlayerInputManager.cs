using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
  public event Action OnEscapePress = delegate { };
  public event Action OnIPress = delegate { };
  public event Action OnLeftMousePress = delegate { };

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      OnEscapePress();
    }

    if (Input.GetKeyDown(KeyCode.I))
    {
      OnIPress();
    }

    //Basic key inputs testing inventory (will be removed later)
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      GameObject.Find("Player").GetComponent<PlayerInventory>().PickupItem(new Item(0, new Color(UnityEngine.Random.Range(0f,1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f))));
    }
    if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      GameObject.Find("Player").GetComponent<PlayerInventory>().PickupItem(new Item(1, new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f))));
    }
    if (Input.GetKeyDown(KeyCode.Alpha3))
    {
      GameObject.Find("Player").GetComponent<PlayerInventory>().PickupItem(new Item(2, new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f))));
    }
    if (Input.GetKeyDown(KeyCode.Alpha4))
    {
      GameObject.Find("Player").GetComponent<PlayerInventory>().PickupItem(new Item(3, new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f))));
    }
    if (Input.GetKeyDown(KeyCode.Alpha5))
    {
      GameObject.Find("Player").GetComponent<PlayerInventory>().PickupItem(new Item(4, new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f))));
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    if (Input.GetMouseButton(0))
    {
      OnLeftMousePress();
    }
  }
}
