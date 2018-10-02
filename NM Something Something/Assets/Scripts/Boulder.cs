using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour {

  private BoardManager.Direction boulderDirection;

  [SerializeField]
  private Animator animator;

  [SerializeField]
  private float secPerTile = 1.0f;
  private float secSinceLastMove = 0.0f;

  public int x;
  public int y;

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

  public void UpdateLastMove(float timeLapse)
  {
    secSinceLastMove += timeLapse;
  }

  public bool ReadyToMove()
  {
    return secSinceLastMove >= secPerTile;
  }

  public void ResetMoveTimer()
  {
    secSinceLastMove = 0.0f;
  }

  public bool CheckCollision(Player player)
  {
    return ((x == player.x) && (y == player.y));
  }
}
