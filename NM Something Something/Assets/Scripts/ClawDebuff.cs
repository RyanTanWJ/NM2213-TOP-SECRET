using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawDebuff : MonoBehaviour {

  private bool isActive = false;

  private BoardManager.Direction breakDirection;

  [SerializeField]
  GameObject ClawDebuffSprite;

  [SerializeField]
  GameObject DirectionIndicator;

  private void Start()
  {
    ClawDebuffSprite.SetActive(false);
    DirectionIndicator.SetActive(false);
  }

  public bool DebuffActive
  {
    get { return isActive; }
  }

  public void NewClawDebuff()
  {
    isActive = true;
    switch (Random.Range(0, 4))
    {
      case 0:
        breakDirection = BoardManager.Direction.RIGHT;
        break;
      case 1:
        breakDirection = BoardManager.Direction.LEFT;
        DirectionIndicator.transform.eulerAngles = new Vector3(0, 0, 180);
        break;
      case 2:
        breakDirection = BoardManager.Direction.UP;
        DirectionIndicator.transform.eulerAngles = new Vector3(0, 0, 90);
        break;
      default:
        breakDirection = BoardManager.Direction.DOWN;
        DirectionIndicator.transform.eulerAngles = new Vector3(0, 0, -90);
        break;
    }
    ClawDebuffSprite.SetActive(true);
    DirectionIndicator.SetActive(true);
  }

  public void AttemptBreakOut(BoardManager.Direction direction)
  {
    if (direction == breakDirection)
    {
      isActive = false;
      ClawDebuffSprite.SetActive(false);
      DirectionIndicator.SetActive(false);
    }
  }
}
