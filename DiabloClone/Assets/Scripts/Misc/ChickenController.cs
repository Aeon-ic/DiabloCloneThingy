using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ChickenController : MonoBehaviourPunCallbacks, IPunObservable
{
  Animator chickenAnimator;

  public float moveSpeed;
  public float timeToScore = 2;
  public bool isEating = false;
  public bool isWalking = false;

  int score = 0;
  float currentScoreTimer = 0;

  // Start is called before the first frame update
  void Start()
  {
    chickenAnimator = this.gameObject.GetComponent<Animator>();
    if (photonView.IsMine)
    {
      ChickenScoreLibrary.instance.scoreBoxes.Add(photonView.Owner.NickName, GameObject.Find("Score").GetComponent<Text>());
    }
    else
    {
      string targetBox = "Score" + (ChickenScoreLibrary.instance.scoreBoxes.Count + 1);
      ChickenScoreLibrary.instance.scoreBoxes.Add(photonView.Owner.NickName, GameObject.Find(targetBox).GetComponent<Text>());
    }
  }

  // Update is called once per frame
  void Update()
  {
    ChickenScoreLibrary.instance.scoreBoxes[photonView.Owner.NickName].text = photonView.Owner.NickName + ": " + score;
    if (!photonView.IsMine)
    {
      return;
    }

    Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

    if (movement.sqrMagnitude == 0)
    {
      chickenAnimator.SetBool("Walk", false);
      isWalking = false;

      if(Input.GetKey(KeyCode.Space))
      {
        chickenAnimator.SetBool("Eat", true);
        isEating = true;
        DoScoring();
      }
      else
      {
        chickenAnimator.SetBool("Eat", false);
        isEating = false;
      }
    }
    else
    {
      chickenAnimator.SetBool("Walk", true);
      isWalking = true;
      chickenAnimator.SetBool("Eat", false);
      isEating = false;
      this.gameObject.transform.position += movement.normalized * (moveSpeed * Time.deltaTime);
      this.gameObject.transform.rotation = Quaternion.Euler(0, Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg, 0);
    }
  }

  void DoScoring()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      currentScoreTimer = timeToScore;
    }
    else
    {
      currentScoreTimer -= Time.deltaTime;
      if (currentScoreTimer <= 0)
      {
        score += 1;
        currentScoreTimer += timeToScore;
      }
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (!photonView.IsMine)
    {
      return;
    }

    ChickenController otherChicken = other.gameObject.GetComponent<ChickenController>();
    if (otherChicken != null)
    {
      if (otherChicken.isEating && isWalking)
      {
        PhotonView otherView = other.gameObject.GetComponent<PhotonView>();
        otherView.RPC("ReduceScore", RpcTarget.Others, otherView.ViewID);
        Debug.Log("Sending RPC to: " + otherView.ViewID);
      }
    }
  }

  [PunRPC] public void ReduceScore(int ID)
  {
    Debug.Log("Called from: " + ID);
    if (ID == this.photonView.ViewID)
    {
      score = score / 2;
    }
  }

  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
  {
    if (stream.IsWriting)
    {
      stream.SendNext(isEating);
      stream.SendNext(score);
    }
    else
    {
      this.isEating = (bool)stream.ReceiveNext();
      this.score = (int)stream.ReceiveNext();
    }
  }

  //public override void OnPlayerLeftRoom(Player otherPlayer)
  //{
  //  if (photonView.IsMine) return;
  //  //base.OnPlayerLeftRoom(otherPlayer);
  //  Debug.Log("Destroying: " + otherPlayer.NickName);
  //  ChickenScoreLibrary.instance.scoreBoxes[otherPlayer.NickName].text = "";
  //  ChickenScoreLibrary.instance.scoreBoxes.Remove(otherPlayer.NickName);
  //  PhotonNetwork.Destroy(photonView);
  //}
}
