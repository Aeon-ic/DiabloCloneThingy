using UnityEngine;
using System;

public class PlayerInputManager : MonoBehaviour
{
  public event Action OnEscapePress = delegate { };
  public event Action OnIPress = delegate { };

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
  }
}
