using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawDebuff : MonoBehaviour {

  private bool isActive = false;

  private int breakDirection = 0;

  [SerializeField]
  List<GameObject> ClawDebuffSprites;

  public bool DebuffActive
  {
    get { return isActive; }
  }

  public void NewClawDebuff()
  {
    isActive = true;
    ClawDebuffSprites[breakDirection].SetActive(false);
    breakDirection = Random.Range(0, 4);
    ClawDebuffSprites[breakDirection].SetActive(true);
  }

  public void AttemptBreakOut(int direction)
  {
    if (direction == breakDirection)
    {
      isActive = false;
      ClawDebuffSprites[direction].SetActive(false);
    }
  }
}
