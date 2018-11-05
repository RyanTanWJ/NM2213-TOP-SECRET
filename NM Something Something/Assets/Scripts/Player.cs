using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public delegate void PlayerMove(BoardManager.Direction direction);
  public static event PlayerMove PlayerMoveEvent;

  public delegate void GameOver();
  public static event GameOver GameOverEvent;

  public delegate void Debuff(GameObject gameObject);
  public static event Debuff DebuffEvent;

  public Vector2Int BoardPosition;

  [SerializeField]
  private ClawDebuff clawDebuff;
  [SerializeField]
  private SpriteRenderer self;

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
        Debug.Log(collision.gameObject.name + " killed you.");
        GameOverEvent();
        break;
      case "Nuisance":
        DebuffEvent(collision.gameObject);
        clawDebuff.NewClawDebuff();
        self.enabled = false;
        break;
      default:
        break;
    }
  }

  private void CheckForPlayerInput()
  {
    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
    {
      if (clawDebuff.DebuffActive)
      {
        clawDebuff.AttemptBreakOut(0);
        ReEnablePlayerSprite();
        return;
      }
      PlayerMoveEvent(BoardManager.Direction.UP);
    }
    else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
    {
      if (clawDebuff.DebuffActive)
      {
        clawDebuff.AttemptBreakOut(1);
        ReEnablePlayerSprite();
        return;
      }
      PlayerMoveEvent(BoardManager.Direction.DOWN);
    }
    else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
    {
      if (clawDebuff.DebuffActive)
      {
        clawDebuff.AttemptBreakOut(2);
        ReEnablePlayerSprite();
        return;
      }
      PlayerMoveEvent(BoardManager.Direction.LEFT);
    }
    else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
    {
      if (clawDebuff.DebuffActive)
      {
        clawDebuff.AttemptBreakOut(3);
        ReEnablePlayerSprite();
        return;
      }
      PlayerMoveEvent(BoardManager.Direction.RIGHT);
    }
  }

  private void ReEnablePlayerSprite()
  {
    if (!clawDebuff.DebuffActive)
    {
      self.enabled = true;
    }
  }
}
