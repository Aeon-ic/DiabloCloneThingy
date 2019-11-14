using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class ChickenScoreLibrary : MonoBehaviourPunCallbacks
{
  [SerializeField]
  public Dictionary<string, Text> scoreBoxes = new Dictionary<string, Text>();

  public static ChickenScoreLibrary instance;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(instance);
    }
    else
    {
      Destroy(this);
    }
  }

  public override void OnPlayerLeftRoom(Player otherPlayer)
  {
    Debug.Log("Destroying: " + otherPlayer.NickName);
    instance.scoreBoxes[otherPlayer.NickName].text = "";
    instance.scoreBoxes.Remove(otherPlayer.NickName);
  }
}
