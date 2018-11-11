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
  [SerializeField]
  private PauseMenu pause;

  [SerializeField]
  private AudioSource deathCry;
  [SerializeField]
  private AudioSource gameplayMusic;

  private bool gameOver = false;

  private void OnEnable()
  {
    pause.OffMenu();
  }

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
        //Debug.Log(collision.gameObject.name + " killed you.");
        gameOver = true;
        StartCoroutine(OnDeath());
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
    if (pause.IsPaused || gameOver)
    {
      return;
    }
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

  IEnumerator OnDeath()
  {
    gameplayMusic.Stop();
    float delay = deathCry.clip.length;
    deathCry.Play();
    Time.timeScale = 0;
    while (delay>0)
    {
      yield return new WaitForEndOfFrame();
      delay -= Time.unscaledDeltaTime;
    }
    Time.timeScale = 1.0f;
    GameOverEvent();
  }
}
