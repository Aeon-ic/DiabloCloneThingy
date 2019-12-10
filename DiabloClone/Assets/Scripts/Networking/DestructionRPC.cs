using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DestructionRPC : MonoBehaviourPun
{
  [PunRPC]
  void DestroyObject()
  {
    Destroy(this.gameObject);
  }

  //void OnTriggerEnter(Collider collider)
  //{
  //  if (collider.gameObject.CompareTag("Player"))
  //  {
  //    photonView.RPC("DestroyObject", RpcTarget.AllBuffered);
  //  }
  //}

  public void DestroyBreakable()
  {
    photonView.RPC("DestroyObject", RpcTarget.AllBuffered);
  }
}
