using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChickenController : MonoBehaviourPun
{
  Animator chickenAnimator;

  public float moveSpeed;
  public float timeToScore = 2;

  Text scoreBox;
  int score = 0;
  float currentScoreTimer = 0;

  // Start is called before the first frame update
  void Start()
  {
    chickenAnimator = this.gameObject.GetComponent<Animator>();
    scoreBox = GameObject.Find("Score").GetComponent<Text>();
  }

  // Update is called once per frame
  void Update()
  {
    if (!photonView.IsMine)
    {
      return;
    }

    Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

    if (movement.sqrMagnitude == 0)
    {
      chickenAnimator.SetBool("Walk", false);

      if(Input.GetKey(KeyCode.Space))
      {
        chickenAnimator.SetBool("Eat", true);
        DoScoring();
      }
      else
      {
        chickenAnimator.SetBool("Eat", false);
      }
    }
    else
    {
      chickenAnimator.SetBool("Walk", true);
      chickenAnimator.SetBool("Eat", false);
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

    scoreBox.text = "Score: " + score;
  }
}
