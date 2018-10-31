using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sushi : MonoBehaviour {

  private BoardManager.Direction sushiDirection;

  [SerializeField]
  private Animator animator;

  private float speed = 500.0f;

  public bool ShouldMove = false;

  // Use this for initialization
  void Start()
  {
    sushiDirection = BoardManager.Direction.RIGHT;
  }

  public void SetBoulderDirection(BoardManager.Direction direction)
  {
    sushiDirection = direction;
    if (direction == BoardManager.Direction.DOWN || direction == BoardManager.Direction.RIGHT)
    {
      animator.SetBool("RightDown", true);
      return;
    }
    animator.SetBool("RightDown", false);
  }

  public BoardManager.Direction GetDirection()
  {
    return sushiDirection;
  }

  public float Speed
  {
    get { return speed; }
  }
}
