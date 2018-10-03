﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public delegate void PlayerMove(BoardManager.Direction direction);
  public static event PlayerMove PlayerMoveEvent;

  public delegate void GameOver();
  public static event GameOver GameOverEvent;

  public int x;
  public int y;
  
  // Update is called once per frame
  void Update ()
  {
    CheckForPlayerInput();
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    Debug.Log("collided with: " + collision.gameObject.name);
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
    if (Input.GetKeyDown(KeyCode.UpArrow))
    {
      PlayerMoveEvent(BoardManager.Direction.UP);
    }
    else if (Input.GetKeyDown(KeyCode.DownArrow))
    {
      PlayerMoveEvent(BoardManager.Direction.DOWN);
    }
    else if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
      PlayerMoveEvent(BoardManager.Direction.LEFT);
    }
    else if (Input.GetKeyDown(KeyCode.RightArrow))
    {
      PlayerMoveEvent(BoardManager.Direction.RIGHT);
    }
  }
}
