using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour {

  private BoardManager.Direction boulderDirection;

  [SerializeField]
  private Animator animator;

  private float speed = 500.0f;

  public bool ShouldMove = false;

  // Use this for initialization
  void Start()
  {
    boulderDirection = BoardManager.Direction.RIGHT;
  }

  public void SetBoulderDirection(BoardManager.Direction direction)
  {
    boulderDirection = direction;
    if (direction == BoardManager.Direction.DOWN || direction == BoardManager.Direction.RIGHT)
    {
      animator.SetBool("RightDown", true);
      return;
    }
    animator.SetBool("RightDown", false);
  }

  public BoardManager.Direction GetDirection()
  {
    return boulderDirection;
  }

  public float Speed
  {
    get { return speed; }
  }
}
