using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public delegate void PlayerMove(BoardManager.Direction direction);
  public static event PlayerMove PlayerMoveEvent;

  public delegate void GameOver();
  public static event GameOver GameOverEvent;

  public Vector2Int BoardPosition;
  
  // Update is called once per frame
  void Update ()
  {
    CheckForPlayerInput();
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    switch (collision.gameObject.tag)
    {
      case "Hazard":
        GameOverEvent();
        break;
      case "Nuisance":
        break;
      default:
        break;
    }
  }

  private void CheckForPlayerInput()
  {
    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
    {
      PlayerMoveEvent(BoardManager.Direction.UP);
    }
    else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
    {
      PlayerMoveEvent(BoardManager.Direction.DOWN);
    }
    else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
    {
      PlayerMoveEvent(BoardManager.Direction.LEFT);
    }
    else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
    {
      PlayerMoveEvent(BoardManager.Direction.RIGHT);
    }
  }
}
