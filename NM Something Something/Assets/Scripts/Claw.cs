using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour {

  private BoardManager.Direction clawDirection;

  private float speed = 250.0f;

  public bool ShouldMove = false;

  // Use this for initialization
  void Start()
  {
    clawDirection = BoardManager.Direction.RIGHT;
  }

  public void SetClawDirection(BoardManager.Direction direction)
  {
    clawDirection = direction;
  }

  public BoardManager.Direction GetDirection()
  {
    return clawDirection;
  }

  public float Speed
  {
    get { return speed; }
  }
}
